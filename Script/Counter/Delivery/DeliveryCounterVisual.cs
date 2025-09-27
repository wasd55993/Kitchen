using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounterVisual : MonoBehaviour
{
    private Animator animator;

    private const string ISTRUE = "isTrue";
    private const string ISFALSE = "isFalse";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        OrderManager.Instance.OnRecipeTrue += OrderManager_OnRecipeTrue;
        OrderManager.Instance.OnRecipeFail += OrderManager_OnRecipeFail;
    }

    private void OnDisable()
    {
        OrderManager.Instance.OnRecipeTrue -= OrderManager_OnRecipeTrue;
        OrderManager.Instance.OnRecipeFail -= OrderManager_OnRecipeFail;
    }

    /// <summary>
    /// 上错菜事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OrderManager_OnRecipeFail(object sender, EventArgs e)
    {
        animator.SetTrigger(ISFALSE);
    }

    /// <summary>
    /// 上对菜事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OrderManager_OnRecipeTrue(object sender, EventArgs e)
    {
        animator.SetTrigger(ISTRUE);
    }
}
