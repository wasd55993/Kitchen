using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerControl : KitchenObjectHolder
{
    [Header("获取控制输入")]
    //移动方向获取
    private GameInput gameInput;

    [Header("基本属性")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float rotateSpeed = 25f;

    [Header("位置、旋转、持有信息")]
    private Vector3 currentPositon;
    private Quaternion currentRotation;

    [Header("远程同步参数")]
    [SerializeField] private float syncSmoothSpeed = 15f;

    [Header("移动控制")]
    //移动控制
    [SerializeField] private bool isMove = false;

    [Header("交互控制")]
    [SerializeField] private LayerMask counterMask;
    private BaseCounter selectCounter;//柜台

    [SerializeField] private GameObject footSEPre;
    private GameObject footSE;
    private void Awake()
    {
        footSE = GameObject.Instantiate(footSEPre);
        currentPositon = transform.position;
        currentRotation = transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        GameInput.Instance.OnInteractionEvent += GameInput_OnInteractionEvent;
        GameInput.Instance.OnMakeEvent += GameInput_OnMakeEvent;
    }
    private void OnDisable()
    {
        GameInput.Instance.OnInteractionEvent -= GameInput_OnInteractionEvent;
        GameInput.Instance.OnMakeEvent -= GameInput_OnMakeEvent;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            PlayerInteraction();
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            PlayerMove();//本地移动
        }
        else
        {
            UpdateLogic();//非本地，更新状态
        }
    }

    //提供别的类获取IsMove,但是只能在这个类修改
    public bool IsMove
    {
        get { return isMove; }
        private set { isMove = value; }
    }

    //玩家移动
    private void PlayerMove()
    {
        Vector3 moveDir = GameInput.Instance.GetMoveInputValue();

        isMove = moveDir != Vector3.zero;

        transform.position += moveDir * Time.deltaTime * moveSpeed;
        footSE.transform.position = transform.position;

        //人物朝向
        if (moveDir != Vector3.zero)
        {
            //计算人物朝向
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }

    //交互显示
    private void PlayerInteraction()
    {
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.5f;
        if (Physics.Raycast(raycastOrigin, transform.forward, out RaycastHit hitInfo, 1f, counterMask))
        {
            //判断是否与柜台交互而不是其他
            if (hitInfo.collider.TryGetComponent<BaseCounter>(out BaseCounter counter))
            {
                //选中柜台
                SetSelectCounter(counter);
            }
            else
            {
                SetSelectCounter(null);
            }
        }
        else 
        {
            SetSelectCounter(null);
        }
    }
    //面向柜台，显示选中的材质
    public void SetSelectCounter(BaseCounter counter)
    {
        if (counter != selectCounter)
        {
            //禁用刚才可以互动的柜台
            selectCounter?.CancelSelect();
            //启用当前可以互动的柜台
            counter?.SelectCounter();
        }
        //把当前柜台设置为可以互动的柜台
        selectCounter = counter;
    }

    /// <summary>
    /// 设置控制系统
    /// </summary>
    public void SetGameInput()
    {
        gameInput = GameObject.Find("GameInput").GetComponent<GameInput>();
    }

    //事件处理
    private void GameInput_OnInteractionEvent(object sender, EventArgs e)
    {
        selectCounter?.Interaction(this);
    }

    private void GameInput_OnMakeEvent(object sender, EventArgs e)
    {
        selectCounter?.InteractionMake(this);
    }



    /// <summary>
    /// 更新非本地玩家位置、旋转状态
    /// </summary>
    public void UpdateLogic()
    {
        transform.position = Vector3.Lerp(transform.position,currentPositon,Time.deltaTime * syncSmoothSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation,currentRotation,Time.deltaTime * syncSmoothSpeed * 1.2f);
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            if (placement.childCount == 1 && placement.GetChild(0).TryGetComponent<KitchenObject>(out KitchenObject targetKitchenObject))
            {
                int ID = targetKitchenObject.GetKitchenObjectSO().GetKitchenObjectID();
                stream.SendNext(ID);
            }
            else
            {
                stream.SendNext(-1);
            }
        }
        else
        {
            currentPositon =(Vector3)stream.ReceiveNext();
            currentRotation =(Quaternion)stream.ReceiveNext();

            int targetID = (int)stream.ReceiveNext();

            if (targetID != -1 && placement.childCount < 1)
            {
                KitchenObject kobj = FindTargetKitchenObject(targetID);
                if (kobj != null)
                {
                    AddIngredient(kobj);
                }
            }
            else if (targetID == -1 && placement.childCount != 0)
            {
                if (placement.GetChild(0).TryGetComponent<KitchenObject>(out KitchenObject targetKitchenObject))
                {
                    KitchenObjectPool.Instance.ReturnPool(
                        targetKitchenObject.GetKitchenObjectSO(),
                        targetKitchenObject
                    );
                    ClearIngredient();
                }
            }
        }
    }
}
