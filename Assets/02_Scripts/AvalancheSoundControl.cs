using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class AvalancheSoundControl : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Player_New player_New;
    [SerializeField] private float minVol = 0.1f;
    [SerializeField] private float maxVol = 1.0f;
    [SerializeField] private float minSpeed = 10f;
    [SerializeField] private float maxSpeed = 40f;

    // void Start()
    // {
    //     if (audioSource == null)
    //     {
    //         audioSource = gameObject.AddComponent<AudioSource>();
    //     }

    //     // 오디오 소스 초기화
    //     audioSource.clip = audioClip;
    //     audioSource.loop = true;
    //     audioSource.Play();
    // }

    // Update is called once per frame
    void Update()
    {
        if (player_New != null)
        {
            // currentSpeed 값에 따라 볼륨 계산
            float speed = player_New.currentSpeed;
            float normalizedSpeed = Mathf.Clamp01((speed - minSpeed) / (maxSpeed - minSpeed));
            audioSource.volume = Mathf.Lerp(maxVol, minVol, normalizedSpeed);
            //Debug.Log($"{audioSource}에서 소리 {audioSource.volume} 으로 재생중");

        }
    }
}
