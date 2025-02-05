using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    
    private Dictionary<string, IObjectPool<GameObject>> pools = new Dictionary<string, IObjectPool<GameObject>>();
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance => instance;
    private void Awake()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "01_Title")
        {
            Destroy(gameObject);
            return; 
        }

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
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);

                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "01_Title")
                {
                    DontDestroyOnLoad(obj);  // 씬이 변경되어도 삭제되지 않도록 설정
                }

                return obj;
            },
            //actionOnGet: obj =>
            //{
            //    if (obj != null)
            //        obj.SetActive(true);
            //},
            actionOnRelease: obj =>
            {
                if (obj != null)
                    obj.SetActive(false);
            },
            actionOnDestroy: obj =>
            {
                if (obj != null)
                    Destroy(obj);
            },
            collectionCheck: false,
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
        obj.SetActive(true);
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
