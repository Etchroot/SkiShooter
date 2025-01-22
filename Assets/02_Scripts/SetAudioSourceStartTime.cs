using UnityEngine;

public class SetAudioSourceStartTime : MonoBehaviour
{
    [SerializeField] private AudioSource skiAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    public float startTime = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //특정 AudioSource 선택
        skiAudioSource.time = startTime;
        bgmAudioSource.time = startTime;

        skiAudioSource.Play();
        bgmAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
