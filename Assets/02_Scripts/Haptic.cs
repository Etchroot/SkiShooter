using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class Haptic : MonoBehaviour
{
    public void TriggerHapticFeedback(bool isLeft, float amplitude, float duration)
    {
        var devices = new List<InputDevice>();

        // 컨트롤러 구분
        if (isLeft)
        {
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, devices);
        }
        else
        {
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, devices);
        }

        // 진동 실행
        foreach (var device in devices)
        {
            if (device.isValid)
            {
                HapticCapabilities capabilities;
                if (device.TryGetHapticCapabilities(out capabilities) && capabilities.supportsImpulse)
                {
                    device.SendHapticImpulse(0, amplitude, duration);
                }
            }
        }
    }
}
