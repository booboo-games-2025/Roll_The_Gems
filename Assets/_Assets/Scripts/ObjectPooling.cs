using UnityEngine;
using UnityEngine.Pool;
using System;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance;

    [System.Serializable]
    public class PoolItem
    {
        public string key;
        public GameObject prefab;
        public int defaultCapacity = 10;
        public int maxSize = 50;
        public Transform parent;
        [HideInInspector] public ObjectPool<GameObject> pool;
        [HideInInspector] public List<GameObject> activeObjects = new List<GameObject>();
    }

    public PoolItem[] pools;

    Dictionary<string, PoolItem> lookup = new Dictionary<string, PoolItem>();

    private void Awake()
    {
        Instance = this;

        foreach (var item in pools)
        {
            item.pool = new ObjectPool<GameObject>(
                () => CreateObject(item.prefab, item.parent),
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPooledObject,
                collectionCheck: false,
                defaultCapacity: item.defaultCapacity,
                maxSize: item.maxSize
            );

            lookup.Add(item.key, item);
        }
    }

    private GameObject CreateObject(GameObject prefab, Transform parent)
    {
        var instance = Instantiate(prefab, parent);
        instance.gameObject.SetActive(false);
        return instance;
    }

    private void OnGetFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReleaseToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyPooledObject(GameObject obj)
    {
        Destroy(obj);
    }

    public GameObject Get(string key, Vector3 position)
    {
        var item = lookup[key];
        var obj = item.pool.Get();
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        item.activeObjects.Add(obj);
        return obj;
    }

    public void Release(string key, GameObject obj)
    {
        var item = lookup[key];
        item.activeObjects.Remove(obj);
        item.pool.Release(obj);
    }
    
    public void ReleaseAll()
    {
        foreach (var item in pools)
        {
            for (int i = item.activeObjects.Count - 1; i >= 0; i--)
            {
                item.pool.Release(item.activeObjects[i]);
            }
            item.activeObjects.Clear();
        }
    }
}
