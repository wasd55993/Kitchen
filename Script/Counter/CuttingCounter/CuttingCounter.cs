using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter, IPunObservable
{
    [SerializeField] private InciseListSO inciseListSO;
    private int currentCuttingCount;//当前切食材的次数

    private InciseProgressUI progressUI;

    [SerializeField] private CuttingCounterVisual cuttingCounterVisual;

    //声音播放
    public static event EventHandler OnCutting;

    protected override void Update()
    {
        base.Update();
    }

    public override void Interaction(PlayerControl player)
    {
        if (player.IsHaveKitchen() && !this.IsHaveKitchen())
        {
            currentCuttingCount = 0;
            IngredientTransfer(player, this);
        }
        else if (!player.IsHaveKitchen() && this.IsHaveKitchen())
        {
            IngredientTransfer(this, player);
        }
    }

    /// <summary>
    /// 制作食材
    /// </summary>
    /// <param name="player"></param>
    public override void InteractionMake(PlayerControl player)
    {
        if ( this.IsHaveKitchen() )
        {
            if (inciseListSO.TryGetInciseOutput(GetKitchenObject(),out CuttingInformation cuttingInformation))
            {
                progressUI = GetKitchenObject().GetInciseProgressUI();

                progressUI.Show();

                Cut();

                progressUI.UpdateProgress((float)currentCuttingCount / cuttingInformation.cuttingCount);
                
                if (currentCuttingCount >= cuttingInformation.cuttingCount)
                {
                    KitchenObject output = inciseListSO.GetInciseOutput(GetKitchenObject());
                    //清除原食材
                    KitchenObjectPool.Instance.ReturnPool(GetKitchenObject().GetKitchenObjectSO(), GetKitchenObject());

                    //添加切制后的食材
                    KitchenObject incise = KitchenObjectPool.Instance.GetObjectFromPool(
                        output.GetKitchenObjectSO(),
                        GetPlacement().position,
                        Quaternion.identity
                        );

                    SetKitchenObject(incise);
                }
            }
        }
    }

    private void Cut()
    {
        currentCuttingCount++;
        OnCutting?.Invoke(this,EventArgs.Empty);
        cuttingCounterVisual.PlayAnimator();
    }

    // 新增：切割状态同步
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info); // 调用父类同步食材状态

        if (stream.IsWriting)
        {
            // 发送端：同步当前切割次数
            stream.SendNext(currentCuttingCount);
        }
        else
        {
            // 接收端：更新切割次数和UI
            currentCuttingCount = (int)stream.ReceiveNext();

            if (inciseListSO.TryGetInciseOutput(GetKitchenObject(), out CuttingInformation cuttingInformation))
            {
                progressUI = GetKitchenObject().GetInciseProgressUI();

                progressUI.Show();

                progressUI.UpdateProgress((float)currentCuttingCount / cuttingInformation.cuttingCount);

                if (currentCuttingCount >= cuttingInformation.cuttingCount)
                {
                    KitchenObject output = inciseListSO.GetInciseOutput(GetKitchenObject());
                    //清除原食材
                    KitchenObjectPool.Instance.ReturnPool(GetKitchenObject().GetKitchenObjectSO(), GetKitchenObject());

                    //添加切制后的食材
                    KitchenObject incise = KitchenObjectPool.Instance.GetObjectFromPool(
                        output.GetKitchenObjectSO(),
                        GetPlacement().position,
                        Quaternion.identity
                        );

                    SetKitchenObject(incise);
                }
            }
        }
    }
}
