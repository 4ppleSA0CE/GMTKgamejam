using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance;
    
    [Header("Background Music")]
    [SerializeField] AudioSource background;
    public AudioClip backgroundMusic;
    [SerializeField] [Range(0f, 1f)] private float backgroundVolume = 0.3f; // Lower default volume
    
    void Awake()
    {
        // Singleton pattern - only one instance should exist
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance exists, destroy this one
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if (background != null && backgroundMusic != null)
        {
            background.clip = backgroundMusic;
            background.loop = true; // Make sure it loops
            background.volume = backgroundVolume; // Set the lower volume
            background.Play();
            Debug.Log("Background music started: " + backgroundMusic.name + " at volume: " + backgroundVolume);
        }
        else
        {
            Debug.LogWarning("Background AudioSource or AudioClip not assigned!");
        }
    }
    
    public void StopBackgroundMusic()
    {
        if (background != null)
        {
            background.Stop();
            Debug.Log("Background music stopped");
        }
    }
    
    public void PauseBackgroundMusic()
    {
        if (background != null)
        {
            background.Pause();
            Debug.Log("Background music paused");
        }
    }
    
    public void ResumeBackgroundMusic()
    {
        if (background != null)
        {
            background.UnPause();
            Debug.Log("Background music resumed");
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        if (background != null)
        {
            background.volume = Mathf.Clamp01(volume);
        }
    }
    
    public bool IsMusicPlaying()
    {
        return background != null && background.isPlaying;
    }
    
    public float GetCurrentVolume()
    {
        return background != null ? background.volume : 0f;
    }
} 