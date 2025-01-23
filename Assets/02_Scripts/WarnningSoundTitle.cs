using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WarnningSoundTitle : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float intervalTime = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.clip = audioClip;
    }
    public void WarnningAlram()
    {
        InvokeRepeating(nameof(PlayAudio), 1.0f, intervalTime); // 5초마다 호출
    }

    void PlayAudio()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
