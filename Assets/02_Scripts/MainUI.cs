using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LeftBulletText;
    [SerializeField] private TextMeshProUGUI RightBulletText;
    [SerializeField] private GameObject LeftReloadText;
    [SerializeField] private GameObject RightReloadText;
    private GunShooting gunShooting;
    private bool isLeftReloadAcive = false; // 현재 리로드 텍스트 상태
    private bool isRightReloadAcive = false; // 현재 리로드 텍스트 상태
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

        LeftReload();
        RightReload();
    }


    void LeftReload()
    {
        int leftRemainBullet = gunShooting.leftCurrentBullet;


        if (leftRemainBullet == 0 && !isLeftReloadAcive)
        {
            Debug.Log("왼손 재장전 중");
            LeftReloadText.SetActive(true);
            isLeftReloadAcive = true;
        }
        else if (leftRemainBullet != 0 && isLeftReloadAcive)
        {
            LeftReloadText.SetActive(false);
            isLeftReloadAcive = false;
        }
    }
    void RightReload()
    {
        int rightRemainBullet = gunShooting.rightCurrentBullet;
        if (rightRemainBullet == 0 && !isRightReloadAcive)
        {
            Debug.Log("오른손 재장전 중");
            RightReloadText.SetActive(true);
            isRightReloadAcive = true;
        }
        else if (rightRemainBullet != 0 && isRightReloadAcive)
        {
            RightReloadText.SetActive(false);
            isRightReloadAcive = false;
        }


    }
}
