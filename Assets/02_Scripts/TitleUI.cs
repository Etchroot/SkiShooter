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
    [SerializeField] Button optionButton;
    [SerializeField] Button backToTitleUIButton;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject bottomElement;
    [SerializeField] GameObject optionPanel;
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
        optionButton.onClick.AddListener(OnOptionButtonClicked);
        backToTitleUIButton.onClick.AddListener(OnBackToTitleButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        string codename = inputCodeName.text;

        if (codename != null)
        {
            codenameText.text = codename;
        }
        else
        {
            codenameText.text = "JamesBond";
        }

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
        toMainScene = "03_Main";
        StartCoroutine(LoadSceneFadeOut(toMainScene));
    }
    private void OnOptionButtonClicked()
    {
        Debug.Log("옵션버튼 클릭됨");
        foreach (var uiElement in uiToAcivate)
        {
            uiElement.SetActive(false);
        }
        bottomElement.SetActive(false);
        optionPanel.SetActive(true);
    }
    private void OnBackToTitleButtonClicked()
    {
        foreach (var uiElement in uiToAcivate)
        {
            uiElement.SetActive(true);
        }
        bottomElement.SetActive(true);
        optionPanel.SetActive(false);
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
