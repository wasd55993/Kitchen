using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountainerCounter : BaseCounter
{
    //动画播放
    [SerializeField] private CountainerCounterVisual countainerCounterVisual;

    //要生成的物体实例
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interaction(PlayerControl player)
    {
        if (player.GetKitchenObject() != null) return;

        countainerCounterVisual.PlayAnimator();

        KitchenObject kitchenObject = KitchenObjectPool.Instance.GetObjectFromPool(
            kitchenObjectSO,
            GetPlacement().position,
            Quaternion.identity
            );

        SetKitchenObject(kitchenObject);
        IngredientTransfer(this, player);
    }
}
