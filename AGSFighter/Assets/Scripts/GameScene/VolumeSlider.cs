using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;

    private void Start()
    {
        // SoundManager���猻�݂̉��ʂ��擾���ăX���C�_�[�̏����l��ݒ�
        float bgmVolume = SoundManager.Instance.bgmSource.volume;
        bgmSlider.value = bgmVolume;
        SoundManager.Instance.SetBGMVolume(bgmVolume);

        float sfxVolume = SoundManager.Instance.sfxSource.volume;
        sfxSlider.value = sfxVolume;
        SoundManager.Instance.SetSFXVolume(sfxVolume);

        float uiVolume = SoundManager.Instance.uiSource.volume;
        uiSlider.value = uiVolume;
        SoundManager.Instance.SetUIVolume(uiVolume);

        // �X���C�_�[�̒l���ύX���ꂽ�Ƃ��ɌĂяo�����C�x���g��ݒ�
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        uiSlider.onValueChanged.AddListener(OnUIVolumeChanged);
    }

    private void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
        PlayerPrefs.SetFloat("BGMVolume", value); // ���ʂ�ۑ�
    }

    private void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value); // ���ʂ�ۑ�
    }

    private void OnUIVolumeChanged(float value)
    {
        SoundManager.Instance.SetUIVolume(value);
        PlayerPrefs.SetFloat("UIVolume", value); // ���ʂ�ۑ�
    }
}
