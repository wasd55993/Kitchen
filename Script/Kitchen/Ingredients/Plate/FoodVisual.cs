using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodVisual : MonoBehaviour
{
    [SerializeField] private Image foodImage;

    public void ShowFoodImage(Sprite sprite)
    { 
        gameObject.SetActive(true);
        foodImage.sprite = sprite;
    }
    public void HideFoodImage()
    {
        gameObject.SetActive(false);
    }
}
