using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderManager : MonoBehaviour
{
    //单例模式
    public static OrderManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private float roderInterval = 2f;
    [SerializeField] private int orderMaxCount = 5;
    [SerializeField] private Transform parent;

    private List<Recipe> orderRecipeList = new List<Recipe>();

    private float intervalTimer = 0;//菜单生成计时
    private int orderCount = 0;//记录菜单数量
    private int successCount = 0;//上菜成功数量
    private int faileCount = 0;//上菜失败数量
    private bool isCreateOrder = false;

    //游戏结束UI
    [SerializeField] private GameOverUI gameOverUI;

    //声音播放
    public event EventHandler OnRecipeTrue;
    public event EventHandler OnRecipeFail;

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
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
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
            int index = UnityEngine.Random.Range(0, recipeListSO.Recipes.Count);

            orderRecipeList.Add(RecipePool.Instance.GetRecipeFromPool(
                recipeListSO.Recipes[index],
                parent.position,
                Quaternion.identity
                ));

            orderRecipeList[orderRecipeList.Count - 1].transform.parent = parent.transform;
        }
    }

    /// <summary>
    /// 上菜时比对菜单
    /// </summary>
    /// <param name="plate">需要上菜的盘子</param>
    public void DeliveryRecipes(Plate plate)
    {
        Recipe newRecipe = null;

        foreach (Recipe recipe in orderRecipeList)
        {
            if (Contrast(recipe, plate))
            {
                
                newRecipe = recipe;
                break;
            }
        }

        if (newRecipe == null)
        {
            OnRecipeFail?.Invoke(this,EventArgs.Empty);//上菜错误事件通知
            PlatesCounter.Instance.SetCurrentPlateCount();//减少全局餐盘数量
            faileCount++;//上菜成功数量-1
            Debug.Log("上菜失败");
        }
        else 
        {
            OnRecipeTrue?.Invoke(this,EventArgs.Empty);//上菜正确事件通知
            RecipePool.Instance.ReturnPool(newRecipe.GetRecipeSO(), newRecipe);//回收菜单
            PlatesCounter.Instance.SetCurrentPlateCount();//减少全局餐盘数量
            orderRecipeList.Remove(newRecipe);//移除当前菜单
            orderCount--;//菜单数量减少
            successCount++;//上菜成功数量+1
            Debug.Log("上菜成功");
        }
    }

    /// <summary>
    /// 和盘子中的食材进行比对
    /// </summary>
    /// <param name="recipe">菜单</param>
    /// <param name="plate">盘子</param>
    public bool Contrast(Recipe recipe, Plate plate)
    {
        //获取菜单中的食材
        List<KitchenObjectSO> recipeKitchenObjectSO = recipe.GetRecipeSO().kitchenObjectSO;
        //获取盘子中的食材
        List<KitchenObjectSO> plateKitchenObjectSO = plate.GetIngredients();
        

        //菜单与盘子食材不一致：1、长度不等
        //                      2、菜单中没有制作的菜品
        if (recipeKitchenObjectSO.Count != plateKitchenObjectSO.Count) return false;

        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObjectSO)
        {
            if (!recipeKitchenObjectSO.Contains(kitchenObjectSO))
            {
                Debug.Log(kitchenObjectSO);
                return false;
            }
        }

        return true;
    }

    public void StartCreatOrder()
    {
        isCreateOrder = true;
    }

    //游戏结束事件响应
    private void GameManager_OnGameOver(object sender, EventArgs e)
    {
        gameOverUI.SetRecipeCount(successCount,faileCount);
    }
}
