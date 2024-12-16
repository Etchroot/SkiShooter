using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LeftBulletText;
    [SerializeField] private TextMeshProUGUI RightBulletText;
    [SerializeField] private TextMeshProUGUI LeftReloadText;
    [SerializeField] private TextMeshProUGUI RightReloadText;
    private GunShooting gunShooting;

    void Start()
    {
        GameObject GunManager = GameObject.FindWithTag("MANAGER");

        if (GunManager != null)
        {
            gunShooting = GunManager.GetComponent<GunShooting>();
        }
        if (gunShooting == null)
        {
            Debug.Log("Manager 태그를 가진 오브젝트에서 gunshooting 스크립트를 찾을 수 없음");
        }

    }


    // Update is called once per frame
    void Update()
    {
        LeftBulletText.text = $"{gunShooting.leftCurrentBullet}";
        RightBulletText.text = $"{gunShooting.rightCurrentBullet}";
    }

    private bool isReloadAcive = false; // 현재 리로드 텍스트 상태
    void ReloadText()
    {
        int leftRemainBullet = gunShooting.LeftCurruntBullet;
        int rightRemainBullet = gunShooting.RightCurruntBullet;

        if (leftRemainBullet == 0 && !isReloadAcive)
        {
            // LeftReloadText.SetAcive(true);
        }


    }
}
