using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip bombHitSound;
    public AudioClip bladeHitSound;
    public AudioClip coinCollectSound;
    public AudioClip gemCollectSound;

    [Header("Settings")]
    public bool musicEnabled = true;
    public bool soundEnabled = true;

    void Awake()
    {
        // ✅ FIXED Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("✅ AudioManager created and will persist across scenes");
        }
        else if (instance != this)
        {
            Debug.Log("⚠️ Duplicate AudioManager found, destroying it");
            Destroy(gameObject);
            return; // ✅ IMPORTANT: Exit after destroying
        }

        // Load saved settings
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;

        // Apply settings
        if (musicSource != null)
            musicSource.mute = !musicEnabled;
        if (sfxSource != null)
            sfxSource.mute = !soundEnabled;
    }

    void Start()
    {
        // Only start music if this is the persistent instance
        if (instance != this) return;

        // Play background music on loop
        if (backgroundMusic != null && musicEnabled && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
            Debug.Log("🎵 Music started playing in scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    // Play sound effects
    public void PlayBombHit()
    {
        if (bombHitSound == null)
        {
            Debug.LogWarning("❌ Bomb Hit Sound is NULL!");
            return;
        }

        if (soundEnabled && sfxSource != null)
        {
            sfxSource.PlayOneShot(bombHitSound);
            Debug.Log("🔊 Playing bomb hit sound");
        }
    }

    public void PlayBladeHit()
    {
        if (bladeHitSound == null)
        {
            Debug.LogWarning("❌ Blade Hit Sound is NULL!");
            return;
        }

        if (soundEnabled && sfxSource != null)
        {
            sfxSource.PlayOneShot(bladeHitSound);
            Debug.Log("🔊 Playing blade hit sound");
        }
    }

    public void PlayCoinCollect()
    {
        if (coinCollectSound == null)
        {
            Debug.LogWarning("❌ Coin Collect Sound is NULL!");
            return;
        }

        if (soundEnabled && sfxSource != null)
        {
            sfxSource.PlayOneShot(coinCollectSound);
            Debug.Log("🔊 Playing coin collect sound");
        }
    }

    public void PlayGemCollect()
    {
        if (gemCollectSound == null)
        {
            Debug.LogWarning("❌ Gem Collect Sound is NULL!");
            return;
        }

        if (soundEnabled && sfxSource != null)
        {
            sfxSource.PlayOneShot(gemCollectSound);
            Debug.Log("🔊 Playing gem collect sound");
        }
    }

    // Toggle functions for settings
    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        if (musicSource != null)
            musicSource.mute = !musicEnabled;
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Music: " + (musicEnabled ? "ON" : "OFF"));
    }

    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        if (sfxSource != null)
            sfxSource.mute = !soundEnabled;
        PlayerPrefs.SetInt("SoundEnabled", soundEnabled ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Sound: " + (soundEnabled ? "ON" : "OFF"));
    }

    // Set volume (0 to 1)
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
            sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}