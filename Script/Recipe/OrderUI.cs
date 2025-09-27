using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private Transform recipeParent;
    [SerializeField] private Recipe recipeUI;

    private void Start()
    {
        //recipeUI.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
    }

    private void OrderManager_OnCreateOrder(object sender, EventArgs e)
    {
        //UpdateUI();
    }

    private void UpdateUI()
    {
        //RecipePool.Instance.GetRecipeFromPool();
    }
}
