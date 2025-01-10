using UnityEngine;
using System.Collections;

public class AudioLoopControl : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource 연결
    public float playDuration = 0.5f; // 사운드 재생 시간
    public float pauseDuration = 0.5f; // 쉬는 시간
    public float totalDuration = 3.0f; // 전체 반복 시간
    public float initialWaitTime = 2.0f; // 시작 대기 시간

    private float elapsedTime = 0f;

    void Start()
    {
        StartCoroutine(PlayAudioWithPause());
    }

    private IEnumerator PlayAudioWithPause()
    {
        yield return new WaitForSeconds(initialWaitTime);

        while (elapsedTime < totalDuration)
        {
            audioSource.Play();
            yield return new WaitForSeconds(playDuration);

            audioSource.Stop();
            yield return new WaitForSeconds(pauseDuration);

            elapsedTime += playDuration + pauseDuration;
        }
    }
}
