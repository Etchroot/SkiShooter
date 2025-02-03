using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("ObjectPoolManager");
                instance = obj.AddComponent<ObjectPoolManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    private Dictionary<string, IObjectPool<GameObject>> pools = new Dictionary<string, IObjectPool<GameObject>>();
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 특정 키로 오브젝트 풀 생성
    /// </summary>
    public void CreatePool(string key, GameObject prefab, int initialSize = 10, int maxSize = 50)
    {
        if (pools.ContainsKey(key))
        {
            Debug.LogWarning($"Pool with key {key} already exists.");
            return;
        }

        prefabs[key] = prefab;

        pools[key] = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false, // 중복 반환 방지 (true일 경우 성능 저하 가능)
            defaultCapacity: initialSize,
            maxSize: maxSize
        );
    }

    /// <summary>
    /// 풀에서 오브젝트 가져오기
    /// </summary>
    public GameObject GetFromPool(string key, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogError($"Pool with key {key} not found!");
            return null;
        }

        GameObject obj = pools[key].Get();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    /// <summary>
    /// 오브젝트를 풀로 반환
    /// </summary>
    public void ReturnToPool(string key, GameObject obj)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogError($"Pool with key {key} not found. Destroying object.");
            Destroy(obj);
            return;
        }

        pools[key].Release(obj);
    }

    public IObjectPool<GameObject> GetPool(string key)
    {
        if (pools.ContainsKey(key))
        {
            return pools[key];
        }

        Debug.LogError($"Pool with key {key} not found!");
        return null;
    }

}
