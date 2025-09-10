using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();
    private AudioSource bgmSource;
    private AudioSource sfxSource;
    private AudioSource uiSource;


    private void Awake()
    {
        Instance = this;

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        sfxSource = gameObject.AddComponent<AudioSource>();
        uiSource = gameObject.AddComponent<AudioSource>();


        LoadAllSounds();
    }

    private void LoadAllSounds()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");
        foreach (AudioClip clip in clips)
        {
            soundLibrary[clip.name] = clip;
        }
    }

    public void Play(string soundName, SoundType type = SoundType.SFX, float volume = 1f)
    {
        if (!soundLibrary.ContainsKey(soundName))
        {
            Debug.LogWarning($"Sound '{soundName}' not found.");
            return;
        }

        AudioClip clip = soundLibrary[soundName];
        switch (type)
        {
            case SoundType.BGM:
                PlayBGM(clip, volume);
                break;
            case SoundType.SFX:
                sfxSource.PlayOneShot(clip, volume);
                break;
            case SoundType.UI:
                uiSource.PlayOneShot(clip, volume);
                break;
        }
    }

    public void PlayBGM(AudioClip clip, float volume)
    {
        if (bgmSource.isPlaying)
            StartCoroutine(FadeOutAndPlayNew(clip, volume));
        else
        {
            bgmSource.clip = clip;
            bgmSource.volume = volume;
            bgmSource.Play();
        }
    }

    private IEnumerator FadeOutAndPlayNew(AudioClip newClip, float targetVolume)
    {
        float fadeTime = 1f;
        float startVolume = bgmSource.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, targetVolume, t / fadeTime);
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }
}
