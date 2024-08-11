using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource sfxSource;
    public AudioSource uiSource;
    public AudioSource bgmSource; // BGM�p��AudioSource
    public AudioClip[] sfxClips;
    public AudioClip[] missClips;
    public AudioClip[] uiClips;
    public AudioClip[] bgmClips; // BGM�p��AudioClip�z��

    public float initBGMVolume = 0.5f; // ����BGM����
    public float initSFXVolume = 0.5f; // ����SFX����
    public float initUIVolume = 0.5f; // ����UI����

    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> uiDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> missDict = new Dictionary<string, AudioClip>(); // ��U��p�̎���

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioDictionaries();
            SetInitialVolumes(); // �������ʂ�ݒ�
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioDictionaries()
    {
        foreach (var clip in sfxClips)
        {
            sfxDict[clip.name] = clip;
        }

        foreach (var clip in uiClips)
        {
            uiDict[clip.name] = clip;
        }

        foreach (var clip in bgmClips)
        {
            bgmDict[clip.name] = clip;
        }
        foreach (var clip in missClips)
        {
            missDict[clip.name] = clip;
        }
    }

    public void PlaySFX(string clipName)
    {
        if (sfxDict.TryGetValue(clipName, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX clip not found: " + clipName);
        }
    }

    public void PlayUIClip(string clipName)
    {
        if (uiDict.TryGetValue(clipName, out var clip))
        {
            uiSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("UI clip not found: " + clipName);
        }
    }

    public void PlayBGM(string clipName, bool loop = true)
    {
        if (bgmDict.TryGetValue(clipName, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM clip not found: " + clipName);
        }
    }

    public void PlayMissSound(string clipName)
    {
        if (missDict.TryGetValue(clipName, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Miss sound clip not found: " + clipName);
        }
    }

    private void SetInitialVolumes()
    {
        bgmSource.volume = initBGMVolume;
        sfxSource.volume = initSFXVolume;
        uiSource.volume = initUIVolume;
    }
    public void SetBGMVolume(float value)
    {
        bgmSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
    }

    public void SetUIVolume(float value)
    {
        uiSource.volume = value;
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
