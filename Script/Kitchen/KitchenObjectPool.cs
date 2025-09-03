using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class KitchenObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolConfig
    {
        public KitchenObjectSO kitchenObjectSO;
        public int poolInitialSize = 20;
        public Transform parent;
    }

    //所有需要对象池的对象的配置表
    public List<PoolConfig> poolConfigs;

    //对象池字典，Key为KitchenObjectSO，Value为该类型对象预制体的队列
    private Dictionary<KitchenObjectSO, Queue<KitchenObject>> poolDictionary;

    //单例模式，提供全局访问点
    public static KitchenObjectPool Instance { get; private set; }

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
        //创建空字典存储所有的对象池
        poolDictionary = new Dictionary<KitchenObjectSO, Queue<KitchenObject>>();

        foreach (PoolConfig poolConfig in poolConfigs)
        {
            CreatePool(poolConfig);
        }
    }
    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="poolConfig"></param>
    private void CreatePool(PoolConfig poolConfig)
    {
        //安全性检查，防止没有该物体也进行对象池的创建
        if (poolConfig.kitchenObjectSO == null || poolConfig.kitchenObjectSO.kitchenPrefab == null) return;

        //创建对象预制体队列
        Queue<KitchenObject> objectPool = new Queue<KitchenObject>();

        //创建物品对象,数量为配置表中池子长度
        for (int i = 0; i < poolConfig.poolInitialSize; ++i)
        {
            KitchenObject obj = InstantiateObject(
                poolConfig.kitchenObjectSO,
                Vector3.zero,
                Quaternion.identity
                ); 

            //禁用物体，需要使用时再启用
            obj.gameObject.SetActive( false );

            //设置父物体
            if (poolConfig.parent != null)
            {
                obj.transform.SetParent( poolConfig.parent, false );
            }

            //把物体加入队列尾部，等待使用
            objectPool.Enqueue( obj );
        }

        //把刚创建的预制体队列添加到字典中
        poolDictionary.Add(poolConfig.kitchenObjectSO, objectPool);
    }

    /// <summary>
    /// 实例化物品对象
    /// </summary>
    /// <param name="kitchenObjectSO">物品对象</param>
    /// <param name="position">位置</param>
    /// <param name="revolve">旋转</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private KitchenObject InstantiateObject(KitchenObjectSO kitchenObjectSO, Vector3 position, Quaternion rotation)
    {
        //创建物品
        GameObject prefabObject = Instantiate(kitchenObjectSO.kitchenPrefab,position, rotation);

        //获取创建的物品的KitchenObject实例，返回该组件
        KitchenObject kitchenObject = prefabObject.GetComponent<KitchenObject>();
        
        //安全性判断，避免空物体占用
        if (kitchenObject == null)
        {
            Debug.Log("物体没有KitchenObject组件");
            Destroy( prefabObject );
            //实例化失败
            return null;
        }

        //返回创建成功的对象实例
        return kitchenObject;
    }

    /// <summary>
    /// 从对象池中获取到对象
    /// </summary>
    /// <param name="kitchenObjectSO">物品</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">旋转</param>
    /// <returns></returns>
    public KitchenObject GetObjectFromPool(KitchenObjectSO kitchenObjectSO,Vector3 position,Quaternion rotation)
    {
        //安全性检查，防止传入的物品为空
        if (kitchenObjectSO == null)
        {
            //获取物品失败
            return null;
        }

        //检查对象池是否有该物品的记录，没有需要动态添加
        if (!poolDictionary.ContainsKey(kitchenObjectSO))
        {
            //不存在需要动态添加物品入对象池中,不用协程
            CreateDynamicPool(kitchenObjectSO);
        }

        //临时存储从对象池中拿到的对象
        KitchenObject kitchenObject;
        //拿到对象池
        Queue<KitchenObject> kitchenObjectPool = poolDictionary[kitchenObjectSO];

        //数量足够直接拿出，池中数量不够直接实例化新的对象
        if (kitchenObjectPool.Count > 0)
        {
            kitchenObject = kitchenObjectPool.Dequeue();
        }
        else
        {
            kitchenObject = InstantiateObject(kitchenObjectSO, position, rotation);
        }

        //激活游戏对象
        kitchenObject.gameObject.SetActive(true);
        kitchenObject.transform.position = position;
        kitchenObject.transform.rotation = rotation;

        return kitchenObject;
    }
    /// <summary>
    /// 动态增加对象池
    /// </summary>
    /// <param name="kitchenObjectSO"></param>
    private void CreateDynamicPool(KitchenObjectSO kitchenObjectSO)
    {
        PoolConfig dynamicPool = new PoolConfig
        {
            kitchenObjectSO = kitchenObjectSO,
            poolInitialSize = 10,
            parent = transform
        };

        CreatePool(dynamicPool);
    }

    /// <summary>
    /// 禁用物品，将对象收回对象池中
    /// </summary>
    public void ReturnPool(KitchenObjectSO kitchenObjectSO,KitchenObject kitchenObject)
    {
        //安全检查，已经消失的不需要回收
        if (kitchenObjectSO == null || kitchenObject == null) return;

        //检查对象池列表中是否存在该对象的池
        if (!poolDictionary.ContainsKey(kitchenObjectSO))
        {
            //直接删除，不需要回收
            Destroy(kitchenObject.gameObject);
            return;
        }

        //场景中隐藏该物体
        kitchenObject.gameObject.SetActive( false );
        //获取队首物体
        KitchenObject firstInPool = poolDictionary[kitchenObjectSO].Peek();
        //设置父物体 不存在则不设置父级
        kitchenObject.transform.SetParent(firstInPool != null ? firstInPool.transform.parent : null);

        //把对象放回队尾，等待使用
        poolDictionary[kitchenObjectSO].Enqueue(kitchenObject);
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
                KitchenObject obj = pool.Dequeue();
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
