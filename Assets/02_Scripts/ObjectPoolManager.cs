using UnityEngine;
using System.Collections.Generic;
using System;

// 오브젝트 구분을 위한 타입
public enum EPoolObjectType
{
    Bullet,
    HitEffect,
    HitBloodEffect,
    EnemyDrone,
    EnemyDrone_Die,
    EnemyDrone_Attack,
    ICE_BRAKE
}

// 오브젝트 풀
[Serializable]
public class PoolInfo
{
    public EPoolObjectType type;
    public int initCount;
    public GameObject prefab;
    //public GameObject container;

    public Queue<GameObject> poolQueue = new Queue<GameObject>();
}
public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    // 오브젝트 풀 리스트
    [SerializeField] private List<PoolInfo> poolInfoList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 각 풀마다 정해진 개수의 오브젝트를 생성해주는 초기화 함수 
    private void Initialize()
    {
        foreach (PoolInfo poolInfo in poolInfoList)
        {
            for (int i = 0; i < poolInfo.initCount; i++)
            {
                poolInfo.poolQueue.Enqueue(CreatNewObject(poolInfo));
            }
        }
    }

    // 초기화 및 풀에 오브젝트가 부족할 때 오브젝트를 생성하는 함수
    private GameObject CreatNewObject(PoolInfo poolInfo)
    {
        GameObject newObject = Instantiate(poolInfo.prefab);
        newObject.gameObject.SetActive(false);
        return newObject;
    }

    // ObjectType(Enum)으로 해당하는 PoolInfo를 반환해주는 함수
    private PoolInfo GetPoolByType(EPoolObjectType type)
    {
        foreach (PoolInfo poolInfo in poolInfoList)
        {
            if(type == poolInfo.type)
            {
                return poolInfo;
            }
        }
        return null;
    }

    // 오브젝트가 필요할 때 호출하는 함수
    public static GameObject GetObject(EPoolObjectType type)
    {
        PoolInfo poolInfo = Instance.GetPoolByType(type);
        GameObject objInstance = null;
        if (poolInfo.poolQueue.Count > 0)
        {
            objInstance = poolInfo.poolQueue.Dequeue();
        }
        else
        {
            objInstance = Instance.CreatNewObject(poolInfo);
        }
        objInstance.SetActive(true);
        return objInstance;
    }

    // 오브젝트 사용 후 다시 풀에 반환하는 함수
    public static void ReturnObject(GameObject obj, EPoolObjectType type)
    {
        PoolInfo poolInfo = Instance.GetPoolByType(type);
        poolInfo.poolQueue.Enqueue(obj);
        obj.SetActive(false);
    }


    /*
    private Dictionary<string, IObjectPool<GameObject>> pools = new Dictionary<string, IObjectPool<GameObject>>();
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

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
                obj.SetActive(false);
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
        // obj.transform.position = position;
        // obj.transform.rotation = rotation;
        //obj.SetActive(true);
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
    */
}
