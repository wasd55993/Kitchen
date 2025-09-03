using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interaction(PlayerControl player)
    {
        if (player.IsHaveKitchen() && !this.IsHaveKitchen())
        { 
            IngredientTransfer(player, this);
            KitchenObjectPool.Instance.ReturnPool(GetKitchenObject().GetKitchenObjectSO(), GetKitchenObject());
            ResetKitchenObject();
        }
    }
}
