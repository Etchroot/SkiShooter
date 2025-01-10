using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    public AudioMixer audioMixer; // 오디오 믹서 연결
    private string sfxVolumeParameter = "SFXVolume";
    private string bgmVolumeParameter = "BGMVolume";



    void Start()
    {
        // PlayerPrefs에서 저장된 볼륨 값 불러오기
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.75f); // 볼륨을 로그 스케일로 표현할 거라서 0.75
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        Debug.Log($"초기 저장 사운드 값 : {bgmVolume},{sfxVolume}");

        if (bgmSlider != null && sfxSlider != null)
        {
            // 슬라이더 초기값 설정
            bgmSlider.value = bgmVolume;
            sfxSlider.value = sfxVolume;

            // 슬라이더 이벤트리스너 추가
            bgmSlider.onValueChanged.AddListener((value) => SetVolume(value, sfxSlider.value));
            sfxSlider.onValueChanged.AddListener((value) => SetVolume(bgmSlider.value, value));
        }

        // 오디오믹서 파라미터 설정
        SetVolume(bgmVolume, sfxVolume);
    }

    public void SetVolume(float bgmVolume, float sfxVolume)
    {
        if (bgmVolume <= 0.0001f)
        { audioMixer.SetFloat(bgmVolumeParameter, -80f); }
        else
        {
            audioMixer.SetFloat(bgmVolumeParameter, Mathf.Log10(bgmVolume) * 20);
        }
        if (sfxVolume <= 0.0001f)
        { audioMixer.SetFloat(sfxVolumeParameter, -80f); }
        else
        {
            audioMixer.SetFloat(sfxVolumeParameter, Mathf.Log10(sfxVolume) * 20);
        }


        Debug.Log($"사운드 값 : {bgmVolume},{sfxVolume}");

        // PlayerPrefs에 저장
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

    }
}