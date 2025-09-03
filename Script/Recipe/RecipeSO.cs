using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/RecipeSO")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public List<KitchenObjectSO> kitchenObjectSO;
}
