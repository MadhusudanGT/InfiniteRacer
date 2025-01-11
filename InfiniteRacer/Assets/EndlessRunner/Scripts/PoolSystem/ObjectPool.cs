using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly T prefab;
    private readonly Transform parentTransform;
    private readonly Queue<T> pool = new Queue<T>();

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parentTransform = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNewObject();
            pool.Enqueue(obj);
        }
    }

    private T CreateNewObject()
    {
        T obj = Object.Instantiate(prefab, parentTransform);
        obj.gameObject.SetActive(false);
        return obj;
    }

    public T GetObject()
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : CreateNewObject();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}