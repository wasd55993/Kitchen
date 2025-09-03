using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KitchenObject/KitchenObjectSO")]
public class KitchenObjectSO : ScriptableObject
{
    [SerializeField] public string kitchenName;
    [SerializeField] public GameObject kitchenPrefab;
    [SerializeField] public Sprite kitchenSprite;
}
