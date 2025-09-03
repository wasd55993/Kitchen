using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatPattyCooked : KitchenObject
{
    [SerializeField] private DecoctionProgressUI progressUI;

    public override DecoctionProgressUI GetDecoctionProgressUI()
    {
        return progressUI;
    }
}
