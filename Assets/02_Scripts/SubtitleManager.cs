using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{

    [SerializeField] TMP_Text subtitleText; // 텍스트 UI
    [SerializeField] private AudioSource audioSource; // 효과음 재생할 오디오소스
    [SerializeField] private AudioClip[] narration; // 나레이션
    private int chkNum = 1;
    private int lastNum = 1;
    private Coroutine currentCoroutine;

    void Start()
    {
        ClearSubtitle(); // 처음에는 자막이 비어 있도록 초기화

        ShowSubtitle(chkNum);
    }
    private void Update()
    {
        chkNum = CheckPointManager.Instance.playerCheckpointIndex + 1;
        if (lastNum != chkNum)
        {
            lastNum = chkNum;
            ShowSubtitle(chkNum);
        }
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
            case 9:
                currentCoroutine = StartCoroutine(DisplaySubtitle9());
                break;
            case 10:
                currentCoroutine = StartCoroutine(DisplaySubtitle10());
                break;

            default:
                Debug.LogWarning("해당하는 자막이 존재하지 않음 : " + num);
                break;
        }
    }

    private IEnumerator DisplaySubtitle1()
    {
        subtitleText.text = "반갑습니다."; // 자막 내용 설정
        PlaySound(0); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(2.0f); // 몇 초 대기

        subtitleText.text = "이 드론으로 전투를 도와드릴\nAI ‘제임스’입니다."; // 자막 내용 설정
        PlaySound(1); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(4.0f); // 몇 초 대기

        subtitleText.text = "탈출 지점에 지원 헬기가 도착 예정입니다."; // 자막 내용 설정
        PlaySound(2); // 효과음 재생
        yield return new WaitForSeconds(4.0f); // 몇 초 대기

        subtitleText.text = "최대한 빠르게 탈출 지점에 도착해야 합니다."; // 자막 내용 설정
        PlaySound(3); // 효과음 재생
        yield return new WaitForSeconds(4.0f); // 몇 초 대기

        subtitleText.text = "탈출에 앞서 분석한 정보를 토대로\n시뮬레이션을 진행하겠습니다."; // 자막 내용 설정
        PlaySound(4); // 효과음 재생
        yield return new WaitForSeconds(5.0f); // 몇 초 대기

        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle2()
    {
        subtitleText.text = "최첨단 스키와 핸드건 두 자루를 준비했습니다."; // 자막 내용 설정
        PlaySound(5); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(4.0f); // 몇 초 대기

        subtitleText.text = "스키는 자동으로 저를 따라오게 설정해두었으므로\n주변 적과 장애물에만 집중하시길 바랍니다."; // 자막 내용 설정
        PlaySound(6); // 효과음 재생
        yield return new WaitForSeconds(7.0f); // 몇 초 대기

        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle3()
    {
        subtitleText.text = "먼저 사격과 재장전을 해보겠습니다."; // 자막 내용 설정
        PlaySound(7); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(3.0f); // 몇 초 대기

        subtitleText.text = "재장전은 횟수에 제한이 없지만\n재장전 시 1초가 소요됩니다."; // 자막 내용 설정
        PlaySound(8); // 효과음 재생
        yield return new WaitForSeconds(6.0f); // 몇 초 대기

        subtitleText.text = "검지로 사격하고 중지로 재장전합니다.";
        PlaySound(23);
        yield return new WaitForSeconds(4.0f);
        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle4()
    {
        subtitleText.text = "이제 매복해 있는 적군을 제거해 보겠습니다."; // 자막 내용 설정
        PlaySound(9); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(4.0f); // 몇 초 대기

        subtitleText.text = "적이 우리에게 공격하기 전에\n먼저 제거해야 합니다."; // 자막 내용 설정
        PlaySound(10); // 효과음 재생
        yield return new WaitForSeconds(4.0f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }

    private IEnumerator DisplaySubtitle5()
    {
        subtitleText.text = "양 옆에 적군의 드론이 등장할 겁니다."; // 자막 내용 설정
        PlaySound(11); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(3.0f); // 몇 초 대기

        subtitleText.text = "등장하기 전에 등장 위치를\n미리 알려 드리겠습니다."; // 자막 내용 설정
        PlaySound(12); // 효과음 재생
        yield return new WaitForSeconds(4.0f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화

    }

    private IEnumerator DisplaySubtitle6()
    {
        subtitleText.text = "드럼통을 폭발시켜\n주변의 적들을 한 번에 제거할 수 있습니다."; // 자막 내용 설정
        PlaySound(13); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(5.0f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }


    private IEnumerator DisplaySubtitle7()
    {
        subtitleText.text = "진행 경로에 얼음으로 된 장애물이 있습니다."; // 자막 내용 설정
        PlaySound(14); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(4.0f); // 몇 초 대기

        subtitleText.text = "부딪히기 전에 미리 파괴해야 합니다."; // 자막 내용 설정
        PlaySound(15); // 효과음 재생
        yield return new WaitForSeconds(3.0f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle8()
    {
        subtitleText.text = "전방에 나무가 쓰러져 있습니다."; // 자막 내용 설정
        PlaySound(16); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(3.0f); // 몇 초 대기

        subtitleText.text = "고개를 숙여서 피해야 하며\n등장하기 전 미리 알려 드리겠습니다."; // 자막 내용 설정
        PlaySound(17); // 효과음 재생
        yield return new WaitForSeconds(5.0f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle9()
    {
        subtitleText.text = "실전에서 스키의 속도는\n시간에 따라 증가합니다."; // 자막 내용 설정
        PlaySound(18); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(4f); // 몇 초 대기

        subtitleText.text = "하지만 적에게 공격당하거나\n장애물을 제거하지 못하고 부딪히게 되면\n스키의 속도가 줄어듭니다. "; // 자막 내용 설정
        PlaySound(19); // 효과음 재생
        yield return new WaitForSeconds(7f); // 몇 초 대기

        subtitleText.text = "속도가 일정 수준 이하로 떨어지면\n눈사태에 휩쓸리게 됩니다."; // 자막 내용 설정
        PlaySound(20); // 효과음 재생
        yield return new WaitForSeconds(5f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }

    private IEnumerator DisplaySubtitle10()
    {
        subtitleText.text = "이제 곧 연구실이 폭발합니다."; // 자막 내용 설정
        PlaySound(21); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(3.0f); // 몇 초 대기

        subtitleText.text = "무사히 탈출하시길 바랍니다."; // 자막 내용 설정
        PlaySound(22); // 효과음 재생
        yield return new WaitForSeconds(3.0f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }

    private void PlaySound(int index)
    {
        if (audioSource != null && narration != null && index >= 0 && index < narration.Length)
        {
            AudioClip clip = narration[index];
            audioSource.PlayOneShot(clip); // 효과음 재생
        }
        else
        {
            Debug.LogWarning("유효하지 않은 인덱스이거나 오디오소스가 없습니다.");
        }
    }


}
