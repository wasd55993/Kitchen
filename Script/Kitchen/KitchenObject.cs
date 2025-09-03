using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public virtual InciseProgressUI GetInciseProgressUI()
    {
        return null;
    }

    public virtual DecoctionProgressUI GetDecoctionProgressUI()
    {
        return null;
    }
}
