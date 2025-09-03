using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    //单例模式
    public static OrderManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private float roderInterval = 2f;
    [SerializeField] private int orderMaxCount = 5;

    private List<RecipeSO> orderRecipeSOList = new List<RecipeSO>();

    private float intervalTimer = 0;//菜单生成计时
    private int orderCount = 0;//记录菜单数量
    private bool isCreateOrder = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this; 
    }

    private void Start()
    {
        isCreateOrder = true;
    }

    private void Update()
    {
        if (isCreateOrder)
        {
            CreateOrder();
        }
    }

    /// <summary>
    /// 创建菜单
    /// </summary>
    private void CreateOrder()
    {
        if (orderCount >= orderMaxCount){ return; }
        
        intervalTimer += Time.deltaTime;
        if (intervalTimer >= roderInterval)
        {
            intervalTimer = 0;

            orderCount++;
            int index = Random.Range(0, recipeListSO.Recipes.Count);
            orderRecipeSOList.Add(recipeListSO.Recipes[index]);
        }
    }

    /// <summary>
    /// 上菜时比对菜单
    /// </summary>
    /// <param name="plate">需要上菜的盘子</param>
    public void DeliveryRecipes(Plate plate)
    {
        RecipeSO newRecipeSO = null;
        foreach (RecipeSO recipe in orderRecipeSOList)
        {
            if (Contrast(recipe, plate))
            {
                newRecipeSO = recipe;
                break;
            }
        }

        if (newRecipeSO == null)
        {
            Debug.Log("上菜失败");
        }
        else 
        {
            orderRecipeSOList.Remove(newRecipeSO);
            Debug.Log("上菜成功");
        }
    }

    /// <summary>
    /// 和盘子中的食材进行比对
    /// </summary>
    /// <param name="recipe">菜单</param>
    /// <param name="plate">盘子</param>
    public bool Contrast(RecipeSO recipe, Plate plate)
    {
        //获取菜单中的食材
        List<KitchenObjectSO> recipeKitchenObjectSO = recipe.kitchenObjectSO;
        //获取盘子中的食材
        List<KitchenObjectSO> plateKitchenObjectSO = plate.GetIngredients();
        
        //菜单与盘子食材不一致：1、长度不等
        //                      2、菜单中没有制作的菜品
        if(recipeKitchenObjectSO.Count != plateKitchenObjectSO.Count) return false;

        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObjectSO)
        {
            if (!recipeKitchenObjectSO.Contains(kitchenObjectSO))
            {
                return false;
            }
        }

        return true;
    }
}
