using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class CinematicSceneManager : MonoBehaviour
{
    public GameObject targetPanel; // 비활성화할 Panel GameObject
    public GameObject leftControllerObject;  // 왼손 컨트롤러 (Inspector에서 할당)
    public GameObject rightControllerObject; // 오른손 컨트롤러 (Inspector에서 할당)

    private InputDevice leftController;
    private InputDevice rightController;

    void Start()
    {
        GetControllers();
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
        if (targetPanel != null && targetPanel.activeSelf)
        {
            targetPanel.SetActive(false); // Panel 비활성화
        }
    }
}
