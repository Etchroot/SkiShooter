using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] Button loginButton;
    [SerializeField] Button gameStartButton;
    [SerializeField] Button quickStartButton;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject[] uiToAcivate;
    [SerializeField] TMP_InputField inputCodeName;
    [SerializeField] TMP_Text codenameText;
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    private string toTutorialScene;
    private string toMainScene;

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
        quickStartButton.onClick.AddListener(OnQuickStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        string codename = inputCodeName.text;

        codenameText.text = codename;

        loginPanel.SetActive(false);

        foreach (var uiElement in uiToAcivate)
        {
            uiElement.SetActive(true);
        }
    }

    private void OnGameStartButtonClicked()
    {
        toTutorialScene = "02_Tutorial";
        StartCoroutine(LoadSceneFadeOut(toTutorialScene));
    }
    private void OnQuickStartButtonClicked()
    {
        toMainScene = "SampleScene";
        StartCoroutine(LoadSceneFadeOut(toMainScene));
    }
    private void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private IEnumerator LoadSceneFadeOut(String scenename)
    {
        // Fade out 시작
        yield return StartCoroutine(FadeOut());

        // 씬 비동기 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenename);
        asyncLoad.allowSceneActivation = false;

        // 로딩이 완료될 떄까지 대기
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f) // 로딩이 완료되면
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / 1f); // 서서히 어두워짐
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f; // 완전히 어두워짐
    }
}
