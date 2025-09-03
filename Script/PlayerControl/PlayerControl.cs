using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerControl : KitchenObjectHolder
{
    //单例模式
    public static PlayerControl Instance { get; private set; }

    [Header("获取控制输入")]
    //移动方向获取
    [SerializeField] private GameInput gameInput;

    [Header("基本属性")]
    //基本属性
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float rotateSpeed = 25f;

    [Header("移动控制")]
    //移动控制
    [SerializeField] private bool isMove = false;

    [Header("交互控制")]
    [SerializeField] private LayerMask counterMask;
    private BaseCounter selectCounter;//柜台

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        gameInput.OnInteractionEvent += GameInput_OnInteractionEvent;
        gameInput.OnMakeEvent += GameInput_OnMakeEvent;
    }
    private void OnDisable()
    {
        gameInput.OnInteractionEvent -= GameInput_OnInteractionEvent;
        gameInput.OnMakeEvent -= GameInput_OnMakeEvent;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInteraction();
    }

    private void FixedUpdate()
    {
        PlayerMove();
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
        Vector3 moveDir = gameInput.GetMoveInputValue();

        isMove = moveDir != Vector3.zero;

        transform.position += moveDir * Time.deltaTime * moveSpeed;

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
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 1f, counterMask))
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

    //事件处理
    private void GameInput_OnInteractionEvent(object sender, EventArgs e)
    {
        selectCounter?.Interaction(this);
    }

    private void GameInput_OnMakeEvent(object sender, EventArgs e)
    {
        selectCounter?.InteractionMake(this);
    }
}
