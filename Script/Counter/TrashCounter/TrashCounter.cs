using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    //声音播放
    public static event EventHandler OnTrash;

    public override void Interaction(PlayerControl player)
    {
        if (player.IsHaveKitchen() && !this.IsHaveKitchen())
        { 
            IngredientTransfer(player, this);
            KitchenObjectPool.Instance.ReturnPool(GetKitchenObject().GetKitchenObjectSO(), GetKitchenObject());
            ResetKitchenObject();
            OnTrash?.Invoke(this,EventArgs.Empty);
        }
    }
}
