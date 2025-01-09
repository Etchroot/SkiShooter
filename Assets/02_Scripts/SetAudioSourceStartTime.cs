using UnityEngine;

public class SetAudioSourceStartTime : MonoBehaviour
{
    public AudioSource bgmAudioSource;
    public float startTime = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bgmAudioSource.time = startTime;

        bgmAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
