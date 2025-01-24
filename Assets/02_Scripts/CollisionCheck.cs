using System;
using UnityEngine;
using UnityEngine.Events;

public class CollisionCheck : MonoBehaviour
{
    [SerializeField] public UnityEvent onObstacleEnemyCollision; // 유니티 이벤트 연결

    public GameObject player;
    public float distance = 10f; // Wall과 player사이의 거리


    // Update is called once per frame
    void Update()
    {
        UpdateWallPosition();
    }

    private void UpdateWallPosition()
    {
        if (player != null)
        {
            // MainCamera의 방향을 가져와서 반대 방향으로 설정
            Vector3 cameraDirection = -player.transform.forward;
            Vector3 newPosition = player.transform.position + cameraDirection * distance;

            // Wall의 위치 설정
            transform.position = newPosition;
            transform.rotation = Quaternion.LookRotation(cameraDirection); // 반대 방향으로 회전
        }
        else
        {
            Debug.LogWarning("플레이어를 찾지 못함");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OBSTACLE"))
        {
            Debug.Log("장애물과 충돌!");
            onObstacleEnemyCollision?.Invoke(); //이벤트 호출
        }
    }


}
