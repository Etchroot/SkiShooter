using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoSignalFadeOut : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvasdGroup;
    [SerializeField] private GameObject textObj;
    [SerializeField] private float deadSpeed = 5f;

    private bool isFading = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        // Player_New 싱글토의 currentSpeed 값 확인
        if (Player_New.Instance != null && Player_New.Instance.currentSpeed <= deadSpeed && !isFading)
        {
            StartCoroutine(FadeOutAndActivateText());
        }
    }

    private IEnumerator FadeOutAndActivateText()
    {
        isFading = true; // 중복 실행 방지
        float elapsedTime = 0f;

        DisableAllAudioSources();

        // 텍스트 활성화
        if (textObj != null)
        {
            textObj.SetActive(true);
        }

        // 서서히 어두워짐
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasdGroup.alpha = Mathf.Clamp01(elapsedTime / 1f);
            yield return null;
        }

        // 완전히 어두워짐
        fadeCanvasdGroup.alpha = 1f;

        // 5초 후 씬 이동
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("01_Title");
    }

    public void DisableAllAudioSources()
    {
        // 씬에 존재하는 활성화 상태의 AudioSource 가져오기
        AudioSource[] audioSources = FindObjectsByType<AudioSource>(
            FindObjectsInactive.Exclude, // 비활성화 된 오브젝트 제외
            FindObjectsSortMode.None); // 정렬하지 않음

        // 각 AudioSource를 비활성화
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.enabled = false;
        }
    }
}