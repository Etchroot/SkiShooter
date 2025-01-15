using System.Collections;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{

    [SerializeField] TMP_Text subtitleText; // 텍스트 UI
    private Coroutine currentCoroutine;

    void Start()
    {
        ClearSubtitle(); // 처음에는 자막이 비어 있도록 초기화
    }

    public void ClearSubtitle()
    {
        subtitleText.text = ""; // 텍스트 비우기
        subtitleText.gameObject.SetActive(false); // 비활성화
    }

    public void ShowSubtitle(int num)
    {
        // 이미 실행 중인 코루틴이 있다면 종료
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // num 값에 따라 다른 코루틴 실행
        switch (num)
        {
            case 1:
                currentCoroutine = StartCoroutine(DisplaySubtitle1());
                break;
            case 2:
                currentCoroutine = StartCoroutine(DisplaySubtitle2());
                break;
            case 3:
                currentCoroutine = StartCoroutine(DisplaySubtitle3());
                break;
            case 4:
                currentCoroutine = StartCoroutine(DisplaySubtitle4());
                break;
            case 5:
                currentCoroutine = StartCoroutine(DisplaySubtitle5());
                break;
            case 6:
                currentCoroutine = StartCoroutine(DisplaySubtitle6());
                break;
            case 7:
                currentCoroutine = StartCoroutine(DisplaySubtitle7());
                break;
            case 8:
                currentCoroutine = StartCoroutine(DisplaySubtitle8());
                break;
            default:
                Debug.LogWarning("해당하는 자막이 존재하지 않음 : " + num);
                break;
        }
    }

    private IEnumerator DisplaySubtitle1()
    {
        subtitleText.text = "안녕! 난 요원 Joy야.\n이 드론으로 탈출을 지원할게."; // 자막 내용 설정
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(5f); // 몇 초 대기

        subtitleText.text = "방해하는 적들을 제거하고 산 아래 지원 헬기까지 도착해야 해."; // 자막 내용 설정
        yield return new WaitForSeconds(5f); // 몇 초 대기

        subtitleText.text = "연구실 폭파까지 시간이 얼마 없으니까 빠르게 시뮬레이션해보자."; // 자막 내용 설정
        yield return new WaitForSeconds(5f); // 몇 초 대기

        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle2()
    {
        subtitleText.text = "최첨단 스키랑 핸드건 두 자루를 준비해 뒀어."; // 자막 내용 설정
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(4f); // 몇 초 대기

        subtitleText.text = "스키는 나를 따라오게 설정해 두었으니까 주변 적과 장애물에만 집중해!"; // 자막 내용 설정
        yield return new WaitForSeconds(6f); // 몇 초 대기

        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle3()
    {
        subtitleText.text = "먼저 사격과 재장전을 해보자.\n재장전은 원하는 만큼 할 수 있지만 재장전 시간이 걸리니까 주의해야 해."; // 자막 내용 설정
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(8f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle4()
    {
        subtitleText.text = "이제 매복해 있는 적군을 제거해 보자.\n적이 우리에게 공격하기 전에 먼저 제거해야 해."; // 자막 내용 설정
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(7f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle5()
    {
        subtitleText.text = "길을 가다 보면 얼음으로 된 장애물이 있을 거야.\n부딪히기 전에 미리 파괴하자."; // 자막 내용 설정
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(7f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle6()
    {
        subtitleText.text = "조심해! 전방에 나무가 쓰러져 있어.\n미리 경고를 해 줄 테니까 고개를 숙여서 피해."; // 자막 내용 설정
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(7f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle7()
    {
        subtitleText.text = "스키의 속도는 시간에 따라 증가해."; // 자막 내용 설정
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(3f); // 몇 초 대기

        subtitleText.text = "하지만 적에게 공격당하거나 적군과 장애물을 제거하지 못하고 놓치게 되면 스키의 속도가 줄어들어."; // 자막 내용 설정
        yield return new WaitForSeconds(8f); // 몇 초 대기

        subtitleText.text = "일정 속도 이하로 떨어지면 눈사태에 휩쓸리게 되니까 조심해."; // 자막 내용 설정
        yield return new WaitForSeconds(5f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }

    private IEnumerator DisplaySubtitle8()
    {
        subtitleText.text = "이제 곧 연구실이 폭파해. 바로 출발하자."; // 자막 내용 설정
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(3f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }


}
