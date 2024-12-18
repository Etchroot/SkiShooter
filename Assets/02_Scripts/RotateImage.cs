using UnityEngine;

public class RotateImage : MonoBehaviour
{
    // 회전 속도 (도/초)
    public float rotationSpeed = 50f;

    void Update()
    {
        // 이미지의 현재 회전 값을 가져옴
        Vector3 rotation = transform.rotation.eulerAngles;

        // z 축을 기준으로 회전시킴
        rotation.z += rotationSpeed * Time.deltaTime; // Time.deltaTime을 사용하여 프레임 독립적인 회전 적용

        // 새로운 회전 값으로 설정
        transform.rotation = Quaternion.Euler(rotation);
    }
}
