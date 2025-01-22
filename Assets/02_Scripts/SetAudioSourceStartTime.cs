using UnityEngine;

public class SetAudioSourceStartTime : MonoBehaviour
{
    [SerializeField] private AudioSource skiAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    public float startTime = 3f; // 오디오 소스가 재생되는 지점
    public float lateStart = 3f; // 늦게 시작하는 시간
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //특정 AudioSource 선택
        skiAudioSource.time = startTime;
        bgmAudioSource.time = startTime;

        Invoke(nameof(PlayAudjioSources), lateStart);
    }

    void PlayAudjioSources()
    {
        skiAudioSource.Play();
        bgmAudioSource.Play();
    }
}
