using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;

public class CinematicSceneManager : MonoBehaviour
{
    public GameObject targetPanel; // 비활성화할 Panel GameObject
    public GameObject videoPanel; // 활성화 할 비디오 판넬
    public GameObject leftControllerObject;  // 왼손 컨트롤러 (Inspector에서 할당)
    public GameObject rightControllerObject; // 오른손 컨트롤러 (Inspector에서 할당)
    public VideoPlayer videoPlayer; // 비디오 플레이어 (Inspector에서 할당)
    public CanvasGroup fadeCanvasGroup; // 페이드 인/아웃을 위한 캔버스 그룹 (Inspector에서 할당)
    private float fadeDuration = 2.0f;

    private InputDevice leftController;
    private InputDevice rightController;

    void Start()
    {
        GetControllers();

        // 동영상이 끝나면 OnVideoEnd 함수 호출
        videoPlayer.loopPointReached += OnVideoEnd;
    }



    void Update()
    {
        // 컨트롤러가 유효한지 확인하고 입력 감지
        if (!leftController.isValid || !rightController.isValid)
        {
            GetControllers();
        }

        if (IsAnyButtonPressed(leftController) || IsAnyButtonPressed(rightController))
        {
            DisablePanel();
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        StartFadeOut();
    }

    void GetControllers()
    {
        // 왼손 컨트롤러 찾기
        var leftDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, leftDevices);
        if (leftDevices.Count > 0)
        {
            leftController = leftDevices[0];
        }

        // 오른손 컨트롤러 찾기
        var rightDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, rightDevices);
        if (rightDevices.Count > 0)
        {
            rightController = rightDevices[0];
        }
    }

    bool IsAnyButtonPressed(InputDevice device)
    {
        if (!device.isValid) return false;

        bool pressed = false;
        return device.TryGetFeatureValue(CommonUsages.primaryButton, out pressed) && pressed ||
               device.TryGetFeatureValue(CommonUsages.secondaryButton, out pressed) && pressed ||
               device.TryGetFeatureValue(CommonUsages.triggerButton, out pressed) && pressed ||
               device.TryGetFeatureValue(CommonUsages.gripButton, out pressed) && pressed;
    }

    void DisablePanel()
    {
        if (targetPanel != null && targetPanel.activeSelf && videoPanel != null)
        {
            targetPanel.SetActive(false); // Panel 비활성화
            videoPanel.SetActive(true); // 비디오 Panel 활성화
        }
    }

    void StartFadeOut()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 씬 전환
        SceneManager.LoadScene("01_Title");
    }
}
