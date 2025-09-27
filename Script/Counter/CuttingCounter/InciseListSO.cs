using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[Serializable]
public class CuttingInformation
{
    public KitchenObject input;
    public KitchenObject output;
    public int cuttingCount;
}

[CreateAssetMenu(menuName = "KitchenObject/InciseListSO")]
public class InciseListSO : ScriptableObject
{
    public List<CuttingInformation> inciseList;

    public bool TryGetInciseOutput(KitchenObject input, out CuttingInformation cuttingInformation)
    {
        if (input == null)
        {
            cuttingInformation = null;
            return false;
        }
        //判断该食材是否可以被切制，并返回找到或未找到
        foreach (CuttingInformation incise in inciseList)
        {
            if (incise.input.GetKitchenObjectSO() == input.GetKitchenObjectSO())
            {
                cuttingInformation = incise;
                return true;
            }
        }

        cuttingInformation = null;
        return false;
    }

    /// <summary>
    /// 获取切制后的食材的预制体
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public KitchenObject GetInciseOutput(KitchenObject input)
    {
        //判断该食材是否可以被切制，并返回切制后的食材
        foreach (CuttingInformation incise in inciseList)
        {
            if (incise.input.GetKitchenObjectSO() == input.GetKitchenObjectSO())
            {
                return incise.output;
            }
        }

        return null;
    }
}
