using System.Collections;
using UnityEngine;

public class Drone : MonoBehaviour
{
    /*
     * 일정시간마다 아이템을 가져옴
     * 적군 위치 브리핑(좌,우,앞)
     * 플레이어 앞에서 길안내역
     */

    public GameObject target; // 플레이어
    public float followDistance = 2f; // 플레이어 앞 유지 거리
    public float followHeight = 1.5f; // 드론의 높이
    public GameObject itemPrefab; // 아이템 프리팹
    public Transform itemDropPoint; // 아이템 드랍 위치
    public float itemPickupInterval = 5f; // 아이템 가져오는 간격

    private Transform playerTransform;

    void Start()
    {
        if (target != null)
        {
            playerTransform = target.transform;
        }

        // 일정 시간마다 아이템을 가져오는 코루틴 시작
        //StartCoroutine(PickupItemRoutine());
    }

    void Update()
    {
        if (playerTransform != null)
        {
            GuidePlayer();
        }
    }

    // 플레이어 앞에서 길 안내
    void GuidePlayer()
    {
        Vector3 targetPosition = playerTransform.position + playerTransform.forward * followDistance;
        targetPosition.y = playerTransform.position.y + followHeight;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
        transform.LookAt(playerTransform.position); // 플레이어를 바라봄
    }

    // 일정 시간마다 아이템 가져오기
    //IEnumerator PickupItemRoutine()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(itemPickupInterval);

    //        if (itemPrefab != null && itemDropPoint != null)
    //        {
    //            Instantiate(itemPrefab, itemDropPoint.position, Quaternion.identity);
    //            Debug.Log("아이템을 가져왔습니다!");
    //        }
    //    }
    //}

    // 적군 위치 브리핑
    public void BriefEnemyPosition(Transform enemyTransform)
    {
        Vector3 directionToEnemy = enemyTransform.position - playerTransform.position;
        float angle = Vector3.SignedAngle(playerTransform.forward, directionToEnemy, Vector3.up);

        if (angle > -45 && angle <= 45)
        {
            Debug.Log("적군 위치: 앞");
        }
        else if (angle > 45 && angle <= 135)
        {
            Debug.Log("적군 위치: 오른쪽");
        }
        else if (angle > -135 && angle <= -45)
        {
            Debug.Log("적군 위치: 왼쪽");
        }
        else
        {
            Debug.Log("적군 위치: 뒤");
        }
    }
}
