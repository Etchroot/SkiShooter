using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera mainCam; // 메인 카메라
    [SerializeField] private Camera heliCam; // 공중 카메라
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam.enabled = true;
        heliCam.enabled = false;
    }

    public void SwitchToHeliCam()
    {
        mainCam.enabled = false;
        heliCam.enabled = true;
    }
}
