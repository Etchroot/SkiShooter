using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivateButtonAfterParent : MonoBehaviour
{
    private Button button;  // 버튼
    private GameObject grandparentObject;  // 2단계 위 부모 객체

    void Start()
    {
        // 현재 객체의 2단계 위 부모 객체를 grandparentObject로 설정
        grandparentObject = transform.parent.parent.gameObject;

        // 버튼 컴포넌트 가져오기
        button = GetComponent<Button>();

        // 버튼을 초기 상태로 비활성화
        // button.gameObject.SetActive(false);

        // 2단계 위 부모 객체의 활성화를 감지하는 코루틴 시작
        StartCoroutine(CheckGrandparentActiveStatus());
    }

    IEnumerator CheckGrandparentActiveStatus()
    {
        // 2단계 위 부모 객체가 비활성화일 경우, 활성화될 때까지 대기
        while (!grandparentObject.activeSelf)
        {
            yield return null;
        }

        // 2단계 위 부모 객체가 활성화되면 2초 대기 후 버튼 활성화
        yield return new WaitForSeconds(2f);

        // 버튼 활성화
        button.gameObject.SetActive(true);
    }
}