using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BulletText;
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
        BulletText.text = $"{gunShooting.CurruntBullet}";
    }
}
