using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectHolder : MonoBehaviour
{
    //食材的位置
    [SerializeField] private Transform placement;
    //当前的食材
    private KitchenObject currentKitchenObject;

    //判断当前是否有食材
    public bool IsHaveKitchen() { return currentKitchenObject != null; }

    //设置和获取当前食材的数据
    public KitchenObject GetKitchenObject()
    {
        return currentKitchenObject;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        currentKitchenObject = kitchenObject;
        currentKitchenObject.transform.parent = placement;
        currentKitchenObject.transform.localPosition = Vector3.zero;
    }
    public void ResetKitchenObject()
    {
        currentKitchenObject = null;
    }
    //获取食材位置
    public Transform GetPlacement()
    {
        return placement;
    }


    //食材转移
    public void IngredientTransfer(KitchenObjectHolder currentHolder,KitchenObjectHolder targetHolder)
    {
        //当前持有食材的玩家或柜台没有食材，无法转移
        if (currentHolder.GetKitchenObject() == null)
        {
            return;
        }
        //同一时间只能存在一个食材，同一个柜台或玩家只能持有一个食材
        if (targetHolder.GetKitchenObject() != null)
        {
            return;
        }

        targetHolder.AddIngredient(currentHolder.GetKitchenObject());
        currentHolder.ClearIngredient();
    }

    //对应玩家或柜台添加食材
    public void AddIngredient(KitchenObject kitchenObject)
    {
        kitchenObject.transform.SetParent(placement);
        kitchenObject.transform.localPosition = Vector3.zero;
        currentKitchenObject = kitchenObject;
    }
    //原柜台或玩家清除食材
    public void ClearIngredient()
    {
        currentKitchenObject = null;
    }
}
