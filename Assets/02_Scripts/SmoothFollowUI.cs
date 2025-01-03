using UnityEngine;

public class SmoothFollowUI : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (XR Origin의 MainCamera)
    public Transform playercamera; // 바라 볼 카메라
    public float rotationSpeed; // 카메라 회전 따라가는 속도
    public float coreSpeed; // 따라가는 속도

    void LateUpdate()
    {
        // 현재 UI의 위치와 타겟 위치를 선형 보간으로 업데이트
        Vector3 targetPosition = target.position + target.forward * 2f; // 타겟 정면 약간 앞
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * coreSpeed);

        // UI가 타겟을 항상 바라보게 만듦 + 카메라 회전 반영
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - playercamera.position);
        Quaternion finalRotation = Quaternion.Euler(playercamera.localRotation.eulerAngles.x, targetRotation.eulerAngles.y, playercamera.localRotation.eulerAngles.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.smoothDeltaTime * rotationSpeed);
    }
}