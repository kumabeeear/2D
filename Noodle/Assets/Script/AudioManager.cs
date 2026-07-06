using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM")]
    public AudioSource bgmSource;

    [Header("SFX")]
    public AudioSource sfxSource;

    [Header("SFX Clips")]
    public AudioClip chopstickSound;
    public AudioClip birdFlySound;
    public AudioClip poopDropSound;
    public AudioClip coinCollectSound;

    void Awake()
    {
        Instance = this;
    }

    public void PlayChopstickSound()
    {
        if (sfxSource != null && chopstickSound != null)
            sfxSource.PlayOneShot(chopstickSound);
    }

    public void PlayBirdFlySound()
    {
        if (sfxSource != null && birdFlySound != null)
            sfxSource.PlayOneShot(birdFlySound);
    }

    public void PlayPoopDropSound()
    {
        if (sfxSource != null && poopDropSound != null)
            sfxSource.PlayOneShot(poopDropSound);
    }

    public void PlayCoinCollectSound()
    {
        if (sfxSource != null && coinCollectSound != null)
            sfxSource.PlayOneShot(coinCollectSound);
    }
}