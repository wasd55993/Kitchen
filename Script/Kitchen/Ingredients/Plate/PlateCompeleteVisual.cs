using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompeleteVisual : MonoBehaviour
{
    [System.Serializable]
    public class FoodMap
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject foodObject;
    }

    [SerializeField]private List<FoodMap> foodList;

    public void ShowFoodObject(KitchenObjectSO kitchenObjectSO)
    {
        foreach (FoodMap food in foodList) 
        {
            if (food.kitchenObjectSO == kitchenObjectSO)
            {
                food.foodObject.SetActive(true);
            }
        }
    }
}
