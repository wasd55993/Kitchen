using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatPattyUnCooked : KitchenObject
{
    [SerializeField] private DecoctionProgressUI progressUI;

    public override DecoctionProgressUI GetDecoctionProgressUI()
    {
        return progressUI;
    }
}
