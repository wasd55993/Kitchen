using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/RecipeListSO")]
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> Recipes;
}
