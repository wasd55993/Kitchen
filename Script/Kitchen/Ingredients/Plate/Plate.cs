using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> placeableList;//可以作为汉堡包制作食材的食材
    
    private List<KitchenObjectSO> ingredients = new List<KitchenObjectSO>();//汉堡包食材列表

    //食材显示
    [SerializeField] private PlateCompeleteVisual plateCompeleteVisual;
    //当前已放置食材显示
    [SerializeField] private CurrentFoodUI currentFoodUI;

    /// <summary>
    /// 把食材添加到盘子上
    /// </summary>
    /// <param name="kitchenObjectSO"></param>
    /// <returns></returns>
    public bool AddIngredientsToPlate(KitchenObjectSO kitchenObjectSO)
    {
        if (ingredients.Contains(kitchenObjectSO) || !placeableList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            ingredients.Add(kitchenObjectSO);
            plateCompeleteVisual.ShowFoodObject(kitchenObjectSO);
            currentFoodUI.ShowCurrentFoodUI(kitchenObjectSO);
            return true;
        }
    }

    public List<KitchenObjectSO> GetIngredients()
    {
        return ingredients;
    }
}