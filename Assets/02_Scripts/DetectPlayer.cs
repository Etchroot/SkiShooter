using System;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] private float detectionRange = 100f; // 감지 범위
    [SerializeField] private LayerMask detectionLayer; // 감지 대상 레이어
    private bool isPlayerDetected = false; // 플레이어 감지 여부 플래그

    [SerializeField] private WarringSignTree warringSignTree; // 스크립트 참조

    // Update is called once per frame
    void Update()
    {
        DetectNearByPlayer();
    }

    private void DetectNearByPlayer()
    {
        // 감지 범위 내에 있는 모든 콜라이더 가져오기
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, detectionLayer);

        bool playerFound = false;

        foreach (var col in colliders)
        {
            if (col.CompareTag("PLAYER"))
            {
                playerFound = true;
                if (!isPlayerDetected) // 처음 감지될 때만 실행
                {
                    isPlayerDetected = true;
                    warringSignTree?.StartBlinking();
                }
                break;
            }
        }
        // 범위 내에 PLAYER가 없을 때 감지 상태 초기화
        if (!playerFound)
        {
            isPlayerDetected = false;
        }
    }


}
