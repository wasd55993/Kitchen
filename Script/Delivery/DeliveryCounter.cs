using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interaction(PlayerControl player)
    {
        if (player.IsHaveKitchen() &&
            player.GetKitchenObject().TryGetComponent<Plate>(out Plate plate))
        {
            
        }
    }
}
