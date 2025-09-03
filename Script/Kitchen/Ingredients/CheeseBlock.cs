using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseBlock : KitchenObject
{
    [SerializeField] private InciseProgressUI progressUI;

    public override InciseProgressUI GetInciseProgressUI()
    {
        return progressUI;
    }
}
