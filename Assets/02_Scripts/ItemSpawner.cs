using UnityEngine;


public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemPrefabs;
    [SerializeField]
    private float spawnInterval = 10f; // 아이템 생성 주기
    [SerializeField]
    private float fristDelay = 10f; // 아이템 처음 나오는 시간
    [SerializeField]
    private Transform[] spawnPoints;

    void Start()
    {
        InvokeRepeating(nameof(SpawnItem), fristDelay, spawnInterval);
    }

    void SpawnItem()
    {
        GameObject randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(randomItem, randomSpawnPoint.position, Quaternion.identity);
    }
}