using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    public AudioMixer audioMixer; // 오디오 믹서 연결
    private string sfxVolumeParameter = "SFXVoulume";
    private string bgmVolumeParameter = "BGMVoulume";



    void Start()
    {
        // PlayerPrefs에서 저장된 볼륨 값 불러오기
        float bgmVolume = PlayerPrefs.GetFloat("BGMBVolulme", 0.75f); // 볼륨을 로그 스케일로 표현할 거라서 0.75
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        // 슬라이더 초기값 설정
        bgmSlider.value = bgmVolume;
        sfxSlider.value = sfxVolume;

        // 오디오믹서 파라미터 설정
        SetVolume(bgmVolume, sfxVolume);

        // 슬러이더 이벤트리스너 추가
        bgmSlider.onValueChanged.AddListener((value) => SetVolume(value, sfxSlider.value));
        sfxSlider.onValueChanged.AddListener((value) => SetVolume(bgmSlider.value, value));
    }

    public void SetVolume(float bgmVolume, float sfxVolume)
    {
        audioMixer.SetFloat(bgmVolumeParameter, Mathf.Log10(bgmVolume) * 20);
        audioMixer.SetFloat(sfxVolumeParameter, Mathf.Log10(sfxVolume) * 20);

        // PlayerPrefs에 저장
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

    }
}