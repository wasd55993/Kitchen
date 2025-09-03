using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interaction(PlayerControl player)
    {
        //玩家把物品放到柜台上，1、玩家有、柜台没有、直接放到柜台上
        //                      2、玩家没有，柜台有，直接给玩家
        //                      3、玩家和柜台都有，玩家拿盘子，把柜台东西放置到盘子
        //                      4、玩家和柜台都有，柜台拿盘子，把玩家东西放置到盘子上
        if (player.IsHaveKitchen() && !this.IsHaveKitchen())
        {
            IngredientTransfer(player, this);
        }
        else if (!player.IsHaveKitchen() && this.IsHaveKitchen())
        {
            IngredientTransfer(this, player);
        }
        else if (player.IsHaveKitchen() &&
            this.IsHaveKitchen() &&
            player.GetKitchenObject().TryGetComponent<Plate>(out Plate plateInPlayer))
        {
            if (plateInPlayer.AddIngredientsToPlate(GetKitchenObject().GetKitchenObjectSO()))
            { 
                KitchenObjectPool.Instance.ReturnPool(GetKitchenObject().GetKitchenObjectSO(), GetKitchenObject());

            }
        }
        else if (player.IsHaveKitchen() &&
            this.IsHaveKitchen() &&
            this.GetKitchenObject().TryGetComponent<Plate>(out Plate plateInCounter)
            )
        {
            if(plateInCounter.AddIngredientsToPlate(player.GetKitchenObject().GetKitchenObjectSO()))
            { 
                KitchenObjectPool.Instance.ReturnPool(player.GetKitchenObject().GetKitchenObjectSO(), player.GetKitchenObject());
                player.ResetKitchenObject();

            }
        }
    }
}
