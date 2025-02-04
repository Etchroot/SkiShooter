using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class HoveringHaptic : MonoBehaviour, IPointerEnterHandler
{

    private float hapticIntensity = 0.5f; // 햅틱 강도
    private float hapticDuration = 0.1f; // 햅틱 지속시간

    public void OnPointerEnter(PointerEventData eventData)
    {
        TriggerHapticFeedback(false, hapticIntensity, hapticDuration);
    }

    private void TriggerHapticFeedback(bool isLeft, float amplitude, float duration)
    {
        var devices = new List<InputDevice>();

        if (isLeft)
        {
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, devices);
        }
        else
        {
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, devices);
        }

        // 컨트롤러에 햅틱 신호 보내기
        foreach (var device in devices)
        {
            if (device.isValid)
            {
                if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities) && capabilities.supportsImpulse)
                {

                    device.SendHapticImpulse(0, amplitude, duration);
                }
            }

        }
    }
}
