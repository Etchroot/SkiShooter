using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.CloudSave.Internal;
using UnityEngine;
using UnityEngine.UI;

public class WarringSignTree : MonoBehaviour
{
    [SerializeField] private Image targetImage; // 깜빡이게 할 이미지
    [SerializeField] private Image leftSide; // 왼쪽 적
    [SerializeField] private Image rightSide; // 오른쪽 적
    [SerializeField] private float blinkInterval = 0.5f; // 깜빡이는 간격
    [SerializeField] private int blinkCount = 5; // 깜빡일 횟수
    [SerializeField] private float detectionRange = 100f; // 감지 범위
    [SerializeField] private LayerMask detectionLayer; // 감지 대상 레이어

    private bool isBlinkng = false; // 깜빡임 여부 플래그
    private bool leftBlinkng = false;
    private bool rightBlinkng = false;
    private bool hasWarned = false; // 경고 여부 플래그

    private Coroutine blinkCoroutine; // 깜빡임 코루틴을 저장할 변수
    private Dictionary<Collider, float> previousDistances = new Dictionary<Collider, float>();

    public static Action LeftSign;
    public static Action RightSign;

    private void Awake()
    {
        LeftSign = () =>
        {
            DetectLeftSide();
        };

        RightSign = () =>
        {
            DetectRightSide();
        };
    }

    void Start()
    {
        targetImage.enabled = false;
        leftSide.enabled = false;
        rightSide.enabled = false;
    }

    void Update()
    {
        DetectTargetInRange();
    }

    public void DetectLeftSide()
    {
        if (!leftBlinkng)
        {
            StartCoroutine(BlinkRoutine(leftSide));
        }
    }
    public void DetectRightSide()
    {
        if (!rightBlinkng)
        {
            StartCoroutine(BlinkRoutine(rightSide));
        }
    }

    #region 깜빡임 로직
    public void StartBlinking()
    {
        if (!isBlinkng)
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine); // 기존 코루틴이 실행중이라면 종료
            }
            blinkCoroutine = StartCoroutine(BlinkRoutine(targetImage));
        }
    }

    private IEnumerator BlinkRoutine(Image sign)
    {
        if (sign == targetImage)
        {
            isBlinkng = true;
        }
        if (sign == leftSide)
        {
            leftBlinkng = true;
        }
        if (sign == rightSide)
        {
            rightBlinkng = true;
        }

        // 이미지 활성화
        sign.enabled = true;

        for (int i = 0; i < blinkCount; i++)
        {
            if (!hasWarned) // 감지 대상이 없어지면 즉시 중단
            {
                break;
            }
            // Alpha값 조정으로 투명하게 만들기
            SetImageAlpha(sign, 0f);
            yield return new WaitForSeconds(blinkInterval);
            SetImageAlpha(sign, 1f);
            yield return new WaitForSeconds(blinkInterval);
        }

        // 깜빡임이 끝난 후 이미지 비활성화
        sign.enabled = false;

        if (sign == targetImage)
        {
            isBlinkng = false;
        }
        if (sign == leftSide)
        {
            leftBlinkng = false;
        }
        if (sign == rightSide)
        {
            rightBlinkng = false;
        }

    }

    private void SetImageAlpha(Image _sign, float alpha)
    {
        Color color = _sign.color;
        color.a = alpha;
        _sign.color = color;
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
                float currentDistance = Vector3.Distance(transform.position, col.transform.position);

                // 이전 프레임 거리 저장 값이 있는지 확인
                if (!previousDistances.ContainsKey(col))
                {
                    previousDistances[col] = currentDistance;
                }

                float previousDistance = previousDistances[col];
                previousDistances[col] = currentDistance; // 현재 거리 업데이트

                // 물체가 가까워지고 아직 경고하지 않은 경우 경고 시작
                if (currentDistance < previousDistance)
                {
                    if (!hasWarned)
                    {
                        hasWarned = true; // 경고 상태로 설정
                        StartBlinking();
                    }
                }
                else if (currentDistance > previousDistance) // TRUNK가 멀어지기 시작하면 즉시 경고 해제
                {
                    hasWarned = false;

                    if (blinkCoroutine != null)
                    {
                        StopCoroutine(blinkCoroutine);
                        targetImage.enabled = false;
                        isBlinkng = false;
                    }
                }
            }
        }

        // 감지된 TRUNK가 하나도 없을 때도 경고 해제
        if (!trunkInRange && hasWarned)
        {
            hasWarned = false;

            if (blinkCoroutine != null) // 깜빡임 중이라면 종료
            {
                StopCoroutine(blinkCoroutine);
                targetImage.enabled = false;
                isBlinkng = false;
            }

            // foreach (var col in previousDistances.Keys)
            // {
            //     if (!trunkInRange || Vector3.Distance(transform.position, col.transform.position) > previousDistances[col])
            //     {
            //         hasWarned = false; // 경고 초기화
            //     }
            // }
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
