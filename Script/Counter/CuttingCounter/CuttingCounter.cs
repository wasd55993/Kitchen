using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private InciseListSO inciseListSO;
    private int currentCuttingCount;//当前切食材的次数

    private InciseProgressUI progressUI;

    [SerializeField] private CuttingCounterVisual cuttingCounterVisual;

    public override void Interaction(PlayerControl player)
    {
        if (player.IsHaveKitchen() && !this.IsHaveKitchen())
        {
            currentCuttingCount = 0;
            IngredientTransfer(player, this);
            progressUI = GetKitchenObject().GetInciseProgressUI();
        }
        else if (!player.IsHaveKitchen() && this.IsHaveKitchen())
        {
            IngredientTransfer(this, player);
        }
    }

    public override void InteractionMake(PlayerControl player)
    {
        if ( this.IsHaveKitchen() )
        {
            if (inciseListSO.TryGetInciseOutput(GetKitchenObject(),out CuttingInformation cuttingInformation))
            {
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
        cuttingCounterVisual.PlayAnimator();
    }
}
