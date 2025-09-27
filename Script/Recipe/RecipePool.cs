using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePool : MonoBehaviour
{
    [System.Serializable]
    public class RecipeConfig
    {
        public RecipeSO recipeSO;
        public int poolInitialSize = 5;
        public Transform parent;
    }

    //所有需要对象池的对象的配置表
    public List<RecipeConfig> poolConfigs;

    //对象池字典，Key为RecipeSO，Value为该类型对象预制体的队列
    private Dictionary<RecipeSO, Queue<Recipe>> poolDictionary;

    //单例模式，提供全局访问点
    public static RecipePool Instance { get; private set; }

    //单例模式初始化
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;

        //跨场景不销毁，全局就一个
        DontDestroyOnLoad(gameObject);

        //初始化对象池
        InitializePools();
    }

    /// <summary>
    /// 所有对象池初始化
    /// </summary>
    private void InitializePools()
    {
        poolDictionary = new Dictionary<RecipeSO, Queue<Recipe>>();

        foreach(RecipeConfig config in poolConfigs)
        {
            CreatePool(config);
        }
    }

    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="recipeConfig"></param>
    private void CreatePool(RecipeConfig recipeConfig)
    {
        //安全性检查，防止没有该物体也进行对象池的创建
        if (recipeConfig.recipeSO == null || recipeConfig.recipeSO.kitchenObjectSO == null) return;

        //创建菜单实例队列
        Queue<Recipe> recipes = new Queue<Recipe>();

        //创建对象池，长度为规定的长度
        for (int i = 0; i < recipeConfig.poolInitialSize; i++)
        {
            Recipe recipe = InstantiateRecipe(
                recipeConfig.recipeSO,
                Vector3.zero,
                Quaternion.identity
                );

            //禁用该对象，需要使用时再拿出使用
            recipe.gameObject.SetActive( false );

            //设置父物体
            if (recipeConfig.parent != null)
            {
                recipe.transform.SetParent(recipeConfig.parent);
            }

            //添加到队列尾部，等待使用
            recipes.Enqueue(recipe);
        }

        //把创建好的队列放到字典中
        poolDictionary.Add(recipeConfig.recipeSO, recipes);
    }

    /// <summary>
    /// 实例化菜单对象
    /// </summary>
    /// <param name="RecipeSO"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private Recipe InstantiateRecipe(RecipeSO recipeSO, Vector3 position, Quaternion rotation)
    {
        //创建菜单实例
        GameObject prefabObject = GameObject.Instantiate(recipeSO.recipePrefab,position,rotation);
        //拿到实例
        Recipe recipeObject = prefabObject.GetComponent<Recipe>();

        if (recipeObject == null)
        {
            Debug.Log("物体没有recipeObjectt实例");
            Destroy(prefabObject);
            //实例化失败
            return null;
        }

        return recipeObject;
    }

    /// <summary>
    /// 从对象池中拿去物品
    /// </summary>
    /// <param name="recipeSO"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public Recipe GetRecipeFromPool(RecipeSO recipeSO, Vector3 position, Quaternion rotation)
    {
        //安全性检查，放置传入空物体
        if (recipeSO == null)
        {
            return null;
        }

        //对象池字典是否有该物体记录，没有需要添加
        if (!poolDictionary.ContainsKey(recipeSO))
        {
            //不存在需要动态添加物品入对象池中,不用协程
            CreateDynamicPool(recipeSO);
        }

        Recipe recipe;

        Queue<Recipe> newRecipes = poolDictionary[recipeSO];

        if (newRecipes.Count > 0)
        {
            recipe = newRecipes.Dequeue();
        }
        else
        {
            recipe = InstantiateRecipe(recipeSO, position, rotation);
        }

        recipe.gameObject.SetActive(true);
        recipe.transform.position = position;
        recipe.transform.rotation = rotation;

        return recipe;
    }
    /// <summary>
    /// 动态增加对象池
    /// </summary>
    /// <param name="kitchenObjectSO"></param>
    private void CreateDynamicPool(RecipeSO recipeSO)
    {
        RecipeConfig dynamicPool = new RecipeConfig
        {
            recipeSO = recipeSO,
            poolInitialSize = 5,
            parent = transform
        };

        CreatePool(dynamicPool);
    }


    /// <summary>
    /// 禁用物品，将对象收回对象池中
    /// </summary>
    public void ReturnPool(RecipeSO recipeSO, Recipe recipe)
    {
        //安全检查，已经消失的不需要回收
        if (recipeSO == null || recipe == null) return;

        //检查对象池列表中是否存在该对象的池
        if (!poolDictionary.ContainsKey(recipeSO))
        {
            //直接删除，不需要回收
            Destroy(recipe.gameObject);
            return;
        }

        //场景中隐藏该物体
        recipe.gameObject.SetActive(false);
        //获取队首物体
        Recipe firstInPool = poolDictionary[recipeSO].Peek();
        //设置父物体 不存在则不设置父级
        recipe.transform.SetParent(firstInPool != null ? firstInPool.transform.parent : null);

        //把对象放回队尾，等待使用
        poolDictionary[recipeSO].Enqueue(recipe);
    }

    /// <summary>
    /// 场景切换时清空对象池
    /// </summary>
    public void ClearAllPools()
    {
        // 遍历字典中的所有值（即所有对象池队列）
        foreach (var pool in poolDictionary.Values)
        {
            // 循环直到队列为空
            while (pool.Count > 0)
            {
                // 从队列头部取出对象
                Recipe obj = pool.Dequeue();
                // 检查对象是否有效
                if (obj != null)
                {
                    // 销毁游戏对象，释放资源
                    Destroy(obj.gameObject);
                }
            }
        }

        // 清空字典，移除所有对象池引用
        poolDictionary.Clear();
    }
}
