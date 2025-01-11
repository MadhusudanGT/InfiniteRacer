using System.Collections.Generic;
using UnityEngine;

public class PoolManagerGen : MonoBehaviour
{
    private void Awake()
    {
        ManagerRegistry.Register<PoolManagerGen>(this);
    }

    [System.Serializable]
    public class PoolConfig<T> where T : MonoBehaviour
    {
        public PoolManagerKeys key;
        public T prefab;
        public int initialSize;
    }

    [SerializeField] private List<PoolConfig<MonoBehaviour>> poolConfigs;

    private readonly Dictionary<PoolManagerKeys, object> pools = new Dictionary<PoolManagerKeys, object>();

    private void Start()
    {
        foreach (var config in poolConfigs)
        {
            var poolType = typeof(ObjectPool<>).MakeGenericType(config.prefab.GetType());
            var pool = System.Activator.CreateInstance(poolType, config.prefab, config.initialSize, transform);
            pools[config.key] = pool;
        }
    }

    public T GetObject<T>(PoolManagerKeys key) where T : MonoBehaviour
    {
        if (pools.TryGetValue(key, out var pool))
        {
            return ((ObjectPool<T>)pool).GetObject();
        }
        Debug.LogError($"No pool found with key {key}");
        return null;
    }

    public void ReturnObject<T>(PoolManagerKeys key, T obj) where T : MonoBehaviour
    {
        if (pools.TryGetValue(key, out var pool))
        {
            ((ObjectPool<T>)pool).ReturnObject(obj);
        }
        else
        {
            Debug.LogError($"No pool found with key {key}");
        }
    }
}

public enum PoolManagerKeys
{
    NONE,
    PLATFORM,
    COINS,
    ABSTRACLES
}