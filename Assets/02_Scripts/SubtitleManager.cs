using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{

    [SerializeField] TMP_Text subtitleText; // 텍스트 UI
    [SerializeField] private AudioSource audioSource; // 효과음 재생할 오디오소스
    [SerializeField] private AudioClip audioClip; // 효과음
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


    // #region 임시 함수
    // public IEnumerator WaitForSecondsCoroutine(float seconds)
    // {
    //     yield return new WaitForSeconds(seconds);
    // }
    // private IEnumerator PlaySubtitlesSequentially()
    // {
    //     for (int i = 1; i <= 8; i++)
    //     {
    //         ShowSubtitle(i);
    //         yield return currentCoroutine;
    //         yield return new WaitForSeconds(1f);
    //     }
    // }

    // #endregion

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
        subtitleText.text = "안녕! 난 널 지원하러 온 요원 Joy야.\n이 드론으로 탈출을 도와줄게."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(6f); // 몇 초 대기

        subtitleText.text = "우리를 방해하는 적들을 제거하고 산 아래 지원 헬기까지 도착해야 해."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        yield return new WaitForSeconds(5f); // 몇 초 대기

        subtitleText.text = "연구실 폭파까지 시간이 얼마 없으니까 빠르게 탈출 과정을 시뮬레이션해보자."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        yield return new WaitForSeconds(6f); // 몇 초 대기

        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle2()
    {
        subtitleText.text = "산을 내려갈 최첨단 스키랑 핸드건 두 자루를 준비해 뒀어."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(5f); // 몇 초 대기

        subtitleText.text = "스키는 자동으로 날 따라 오니까 이동은 나한테 맡기고 주변 적과 장애물에만 집중해!"; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        yield return new WaitForSeconds(6f); // 몇 초 대기

        ClearSubtitle(); // 자막 초기화
    }
    private IEnumerator DisplaySubtitle3()
    {
        subtitleText.text = "먼저 사격과 재장전을 해보자.\n재장전은 원하는 만큼 할 수 있지만 재장전 시간이 걸리니까 주의해야 해."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(8f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
        //yield return new WaitForSeconds(5f); // 추가 대기
    }
    private IEnumerator DisplaySubtitle4()
    {
        subtitleText.text = "이제 매복해 있는 적군을 제거해 보자.\n적이 우리에게 공격하기 전에 먼저 제거해야 해."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(7f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
                         // yield return new WaitForSeconds(3f);
    }
    private IEnumerator DisplaySubtitle5()
    {
        subtitleText.text = "길을 가다 보면 얼음으로 된 장애물이 있을 거야.\n부딪히기 전에 미리 파괴하자."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(7f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
                         // yield return new WaitForSeconds(2f);
    }
    private IEnumerator DisplaySubtitle6()
    {
        subtitleText.text = "조심해! 전방에 나무가 쓰러져 있어.\n미리 경고를 해 줄 테니까 고개를 숙여서 피해."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(7f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
                         // yield return new WaitForSeconds(9f);
    }
    private IEnumerator DisplaySubtitle7()
    {
        subtitleText.text = "실전에선 스키의 속도가 시간에 따라 증가할거야."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(4f); // 몇 초 대기

        subtitleText.text = "하지만 적에게 공격당하거나 적군과 장애물을 제거하지 못하고 놓치게 되면 스키의 속도가 줄어들어."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        yield return new WaitForSeconds(8f); // 몇 초 대기

        subtitleText.text = "일정 속도 이하로 떨어지면 눈사태에 휩쓸리게 되니까 조심해."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        yield return new WaitForSeconds(5f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }

    private IEnumerator DisplaySubtitle8()
    {
        subtitleText.text = "곧 연구실이 폭발해.\n가상 시뮬레이션에서 빠져나갈게. 이제 실전이야."; // 자막 내용 설정
        PlaySound(audioClip); // 효과음 재생
        subtitleText.gameObject.SetActive(true); // 자막 활성화
        yield return new WaitForSeconds(6f); // 몇 초 대기
        ClearSubtitle(); // 자막 초기화
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // 효과음 재생
        }
    }


}
