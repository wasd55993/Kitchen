using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DecoctionInformation
{
    public KitchenObject input;
    public KitchenObject output;
    public int decoctionTime;
}

[CreateAssetMenu(menuName = "KitchenObject/DecoctionListSO")]
public class DecoctionListSO : ScriptableObject
{
    public List<DecoctionInformation> decoctionList;

    public bool TryGetDecoctionOutput(KitchenObject input, out DecoctionInformation decoctionInformation)
    {
        //判断该食材是否可以被煎制，并返回找到或未找到
        foreach (DecoctionInformation decoction in decoctionList)
        {
            if (decoction.input.GetKitchenObjectSO() == input.GetKitchenObjectSO())
            {
                decoctionInformation = decoction;
                return true;
            }
        }

        decoctionInformation = null;
        return false;
    }

    /// <summary>
    /// 获取煎制后的食材的预制体
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public KitchenObject GetDecoctionOutput(KitchenObject input)
    {
        //判断该食材是否可以被煎制，并返回煎制后的食材
        foreach (DecoctionInformation decoction in decoctionList)
        {
            if (decoction.input.GetKitchenObjectSO() == input.GetKitchenObjectSO())
            {
                return decoction.output;
            }
        }

        return null;
    }
}
