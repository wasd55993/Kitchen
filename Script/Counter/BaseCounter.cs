using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : KitchenObjectHolder
{
    //选中的柜台物体
    [SerializeField] private GameObject selectCounter;

    //柜台交互
    public virtual void Interaction(PlayerControl player){}
    public virtual void InteractionMake(PlayerControl player) { }

    //启用选中柜台的显示物体
    public void SelectCounter()
    {
        selectCounter.SetActive(true);
    }
    //禁用选中柜台的显示材质
    public void CancelSelect()
    {
        selectCounter.SetActive(false);
    }
}
