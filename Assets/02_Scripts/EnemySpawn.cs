using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab; // 적군 프리팹
    public Transform spawnPoint;  // 스폰 위치
    public Transform target;      // 목표 오브젝트

    void Start()
    {

    }

    void Update()
    {

    }

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        //MonsterController MonsterController = enemyPrefab.GetComponent<MonsterController>();
        //if (MonsterController != null)
        //{
        //    MonsterController.target = target;
        //}
    }
}
