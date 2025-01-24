using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{

    [SerializeField] private float returnHeight = 500f; // 헬리콥터의 초기 위치 상승 y값
    [SerializeField] private float detectionRadius = 1000f; // 플레이어를 감지할 반경
    [SerializeField] private float returnDuration = 10f; // 돌아오는 데 걸리는 시간 (초)  

    private bool hasReturned = false; // 플레이어가 들어왔는지 확인하는 플래그

    void Start()
    {
        // 씬이 시작되면 y값 수정
        transform.position = new Vector3(transform.position.x, transform.position.y + returnHeight, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        if (!hasReturned)
        {
            DetectPlayerInRange();
        }
    }

    private void DetectPlayerInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("PLAYER"));

        if (colliders.Length > 0)
        {
            hasReturned = true;
            StartCoroutine(ReturnToOriginalPosition());
        }
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x, transform.position.y - returnHeight, transform.position.z);

        float elapsedTime = 0f;

        // y 좌표 returnHeight만큼 감소시키기
        while (elapsedTime < returnDuration)
        {
            float newY = Mathf.Lerp(startPosition.y, endPosition.y, elapsedTime / returnDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막 y좌표로 이동
        transform.position = endPosition;
        // 리턴이 끝난 후 플래그 초기화
        //hasReturned = false;
    }
}
