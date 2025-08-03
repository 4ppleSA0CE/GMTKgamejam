using UnityEngine;
using System.Collections;

public class LandingZoneAudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip firstAudioClip;
    [SerializeField] private AudioClip secondAudioClip;
    
    [Header("Audio Settings")]
    [SerializeField] private bool playOnStart = true;
    [SerializeField] [Range(0f, 2f)] private float volume = 1.5f; // Louder than background music
    
    private AudioSource audioSource;
    
    void Start()
    {
        // Get or create AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configure audio source
        audioSource.volume = volume;
        audioSource.loop = false; // Never loop for this sequence
        
        // Start playing audio sequence if enabled and clips are assigned
        if (playOnStart && (firstAudioClip != null || secondAudioClip != null))
        {
            StartCoroutine(PlayAudioSequence());
        }
        else if (firstAudioClip == null && secondAudioClip == null)
        {
            Debug.LogWarning("No audio clips assigned to Landing Zone Audio Manager!");
        }
    }
    
    private IEnumerator PlayAudioSequence()
    {
        // Play first audio clip if assigned
        if (firstAudioClip != null)
        {
            audioSource.clip = firstAudioClip;
            audioSource.Play();
            Debug.Log("First audio started: " + firstAudioClip.name);
            
            // Wait for first clip to finish
            yield return new WaitForSeconds(firstAudioClip.length);
        }
        
        // Play second audio clip if assigned
        if (secondAudioClip != null)
        {
            audioSource.clip = secondAudioClip;
            audioSource.Play();
            Debug.Log("Second audio started: " + secondAudioClip.name);
            
            // Wait for second clip to finish
            yield return new WaitForSeconds(secondAudioClip.length);
        }
        
        Debug.Log("Audio sequence completed");
    }
    
    // Public methods for manual control
    public void StartAudioSequence()
    {
        StartCoroutine(PlayAudioSequence());
    }
    
    public void StopAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            Debug.Log("Audio sequence stopped");
        }
    }
    
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp(newVolume, 0f, 2f);
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
    
    public bool IsAudioPlaying()
    {
        return audioSource != null && audioSource.isPlaying;
    }
} 