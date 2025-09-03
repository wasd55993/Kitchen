using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentFoodUI : MonoBehaviour
{
    [SerializeField] private FoodVisual foodVisual;

    private void Start()
    {
        foodVisual.HideFoodImage();
    }

    public void ShowCurrentFoodUI(KitchenObjectSO kitchenObjectSO)
    {
        FoodVisual newFoodVisual = GameObject.Instantiate(foodVisual,transform);
        newFoodVisual.ShowFoodImage(kitchenObjectSO.kitchenSprite);
    }
}
