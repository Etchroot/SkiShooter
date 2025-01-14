using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject xrOriginMain; // 메인 xr오리진
    [SerializeField] private GameObject xrOriginLeaderBoard; // 리더보드 xr오리진
    [SerializeField] private GameObject Helicopter; // 헬기 프리펩

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 상태 확인
        if (xrOriginMain == null || xrOriginLeaderBoard == null)
        {
            Debug.LogError("XR오리진이 모두 설정되지 않았습니다.");
            return;
        }
        if (!xrOriginMain.activeSelf)
        {
            Debug.LogWarning("메인 xr오리진이 비활성화 되어있습니다.");
        }
    }

    public void SwitchToXR()
    {
        xrOriginMain.SetActive(false);
        xrOriginLeaderBoard.SetActive(true);
        Destroy(Helicopter);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
