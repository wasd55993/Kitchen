using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObject plateObject;

    private const int plateMaxCount = 5;//当前场景最大盘子数量]
    private static int currentPlateCount = 1;

    private List<KitchenObject> plates = new List<KitchenObject>();//柜台上盘子数量

    public override void Interaction(PlayerControl player)
    {
        if (player.IsHaveKitchen() && !this.IsHaveKitchen())
        {
            IngredientTransfer(player, this);
        }
        else if (!player.IsHaveKitchen() && this.IsHaveKitchen())
        {
            IngredientTransfer(this, player);
            TakePlates();
        }
    }


    private void Update()
    {
        if (currentPlateCount < plateMaxCount)
        {
            CreatePlates();
        }
    }

    /// <summary>
    /// 把盘子生成在在柜台上，公用这些盘子
    /// </summary>
    public void CreatePlates()
    {
        KitchenObject plateVisual = KitchenObjectPool.Instance.GetObjectFromPool(
            plateObject.GetKitchenObjectSO(),
            GetPlacement().position,
            Quaternion.identity
            );

        //添加盘子
        plates.Add(plateVisual);
        SetKitchenObject(plateVisual);
        plateVisual.transform.localPosition = Vector3.zero + Vector3.up * 0.1f * plates.Count;

        currentPlateCount = plates.Count;
    }

    /// <summary>
    /// 玩家从柜台拿走盘子时，更新柜台的持有盘子对象
    /// </summary>
    public void TakePlates()
    {
        if (plates.Count == 0) return;

        plates.RemoveAt(plates.Count - 1);

        if (plates.Count > 0)
        {
            KitchenObject canGivePlate = plates[plates.Count - 1];
            // 还有剩余盘子，设置最后一个为当前持有对象
            SetKitchenObject(canGivePlate);
        }
        else
        {
            // 没有盘子了，清空持有对象
            ResetKitchenObject();
        }

        for (int i = 0; i < plates.Count; i++)
        {
            plates[i].transform.localPosition = Vector3.zero + Vector3.up * 0.1f * (i+1);
        }
    }
}
