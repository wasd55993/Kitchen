using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    [SerializeField] private RecipeSO recipeSO;

    public RecipeSO GetRecipeSO()
    {
        return recipeSO;
    }
}
