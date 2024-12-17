using UnityEngine;
using TMPro;
using System.Collections;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LeftBulletText;
    [SerializeField] private TextMeshProUGUI RightBulletText;
    [SerializeField] private TextMeshProUGUI PlayerSpeedText;
    [SerializeField] private GameObject LeftReloadText;
    [SerializeField] private GameObject LeftReloadImage;
    [SerializeField] private GameObject RightReloadText;
    [SerializeField] private GameObject RightReloadImage;
    [SerializeField] private GameObject LeftController;
    [SerializeField] private GameObject RightController;
    private GunShooting leftgunShooting;
    private GunShooting rightgunShooting;
    private Player player;
    private bool isLeftReloadAcive = false; // 현재 리로드 텍스트 상태
    private bool isRightReloadAcive = false; // 현재 리로드 텍스트 상태
    void Start()
    {

        //GameObject LeftController = GameObject.FindWithTag("LCONT");

        if (LeftController != null)
        {
            Debug.Log("LCONT 태그를 가진 오브젝트르 찾았습니다.");
            leftgunShooting = LeftController.GetComponent<GunShooting>();
        }
        if (leftgunShooting == null)
        {
            Debug.Log("LCONT 태그를 가진 오브젝트에서 gunshooting 스크립트를 찾을 수 없음");
        }

        //GameObject rightController = GameObject.FindWithTag("RCONT");

        if (RightController != null)
        {
            rightgunShooting = RightController.GetComponent<GunShooting>();
        }
        if (rightgunShooting == null)
        {
            Debug.Log("RCONT 태그를 가진 오브젝트에서 gunshooting 스크립트를 찾을 수 없음");
        }


    }



    // Update is called once per frame
    void Update()
    {
        LeftBulletText.text = $"{leftgunShooting.leftCurrentBullet}";
        RightBulletText.text = $"{rightgunShooting.rightCurrentBullet}";

        if (Player.Instance != null)
        {
            PlayerSpeedText.text = $"{Player.Instance.moveSpeed} km/h";
        }
        else
        {
            Debug.LogWarning("Player 인스턴스를 찾을 수 없습니다.");
        }


        LeftReloading();
        RightReloading();
        LeftReload();
        RightReload();
    }


    void LeftReload()
    {
        int leftRemainBullet = leftgunShooting.leftCurrentBullet;


        if (leftRemainBullet == 0 && !isLeftReloadAcive)
        {
            Debug.Log("왼손 재장전 필요");
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
        int rightRemainBullet = rightgunShooting.rightCurrentBullet;

        if (rightRemainBullet == 0 && !isRightReloadAcive)
        {
            Debug.Log("오른손 재장전 필요");
            RightReloadText.SetActive(true);
            //RightReloadImage.SetActive(true);
            //Invoke("DeacitveRightReloadImage", 2f);
            isRightReloadAcive = true;
        }
        else if (rightRemainBullet != 0 && isRightReloadAcive)
        {
            RightReloadText.SetActive(false);
            isRightReloadAcive = false;
        }

    }

    void LeftReloading()
    {
        if (leftgunShooting.LeftReloadValue == true)
        {
            LeftReloadImage.SetActive(true);
            Invoke("DeacitveLeftReloadImage", 2f);
        }
    }
    void RightReloading()
    {
        if (rightgunShooting.RightReloadValue == true)
        {
            RightReloadImage.SetActive(true);
            Invoke("DeacitveRightReloadImage", 2f);
        }
    }
    void DeacitveLeftReloadImage()
    {
        LeftReloadImage.SetActive(false);
    }
    void DeacitveRightReloadImage()
    {
        RightReloadImage.SetActive(false);
    }
}
