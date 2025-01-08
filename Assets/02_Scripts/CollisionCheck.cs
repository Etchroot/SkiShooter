using UnityEngine;
using UnityEngine.Events;

public class CollisionCheck : MonoBehaviour
{
    [SerializeField] private UnityEvent onObstacleEnemyCollision; // 유니티 이벤트 연결

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY"))
        {
            Debug.Log("장애물 및 적과 충돌!");
            onObstacleEnemyCollision?.Invoke(); //이벤트 호출
        }
    }


}
