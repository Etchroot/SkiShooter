using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundManager : MonoBehaviour
{
    [SerializeField] private Button[] buttons; // 버튼 배열
    [SerializeField] private AudioClip clickSound; // 버튼 클릭 효과음
    [SerializeField] private AudioSource audioSource; // 효과음을 재생할 AudioSource

    void Start()
    {
        // 모든 버튼에 클릭 이벤트 추가
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PlaySound());
        }
    }

    void PlaySound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.volume = 1.0f; // 기본 볼륨을 최대로 설정
            audioSource.PlayOneShot(clickSound, 2.0f); // 효과음을 2배 크게 재생
        }
    }
}
