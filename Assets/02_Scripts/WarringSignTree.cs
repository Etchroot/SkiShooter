using System;
using System.Collections;
using Unity.Services.CloudSave.Internal;
using UnityEngine;
using UnityEngine.UI;

public class WarringSignTree : MonoBehaviour
{
    [SerializeField] private Image targetImage; // 깜빡이게 할 이미지
    [SerializeField] private float blinkInterval = 0.5f; // 깜빡이는 간격
    [SerializeField] private int blinkCount = 5; // 깜빡일 횟수
    [SerializeField] private float detectionRange = 100f; // 감지 범위
    [SerializeField] private LayerMask detectionLayer; // 감지 대상 레이어

    private bool isBlinkng = false; // 깜빡임 여부 플래그
    private bool hasWarned = false; // 경고 여부 플래그

    void Start()
    {
        targetImage.enabled = false;
    }

    void Update()
    {
        DetectTargetInRange();
    }

    #region 깜빡임 로직
    public void StartBlinking()
    {
        if (!isBlinkng)
        {
            StartCoroutine(BlinkRoutine());
        }
    }

    private IEnumerator BlinkRoutine()
    {
        isBlinkng = true;

        // 이미지 활성화
        targetImage.enabled = true;

        for (int i = 0; i < blinkCount; i++)
        {
            // Alpha값 조정으로 투명하게 만들기
            SetImageAlpha(0f);
            yield return new WaitForSeconds(blinkInterval);
            SetImageAlpha(1f);
            yield return new WaitForSeconds(blinkInterval);
        }

        // 깜빡임이 끝난 후 이미지 비활성화
        targetImage.enabled = false;
        isBlinkng = false;
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = targetImage.color;
        color.a = alpha;
        targetImage.color = color;
    }
    #endregion 

    #region 범위 감지 로직
    private void DetectTargetInRange()
    {
        // 감지 범위 내 레이어 필터를 사용하여 콜라이더 탐색
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, detectionLayer);

        bool trunkInRange = false;

        foreach (Collider col in colliders)
        {
            // TRUNK 태그 확인
            if (col.CompareTag("TRUNK"))
            {
                trunkInRange = true;

                // 아직 경고하지 않은 경우 경고 시작
                if (!hasWarned)
                {
                    hasWarned = true; // 경고 상태로 설정
                    StartBlinking();
                }
                break;
            }
        }

        // TRUNK가 범위에서 벗어난 경우 다시 호출 기회를 부여
        if (!trunkInRange)
        {
            hasWarned = false; // 경고 초기화
        }
    }
    #endregion

    // private void OnDrawGizmosSelected()
    // {
    //     // 감지 범위 시각화 (디버깅용)
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, detectionRange);
    // }
}
