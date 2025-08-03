using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExhaustSceneManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public float exhaustDisplayDuration = 3f; // How long to show the exhaust scene
    public string landingZoneScene = "Landing zone";
    
    [Header("UI Elements")]
    public GameObject exhaustImage; // Assign the exhaust image in inspector
    
    private bool isTransitioning = false;
    
    void Awake()
    {
        // Subscribe to scene load events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDestroy()
    {
        // Unsubscribe from scene load events
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void Start()
    {
        Debug.Log("ExhaustSceneManager Start() called");
        Debug.Log("Current scene: " + SceneManager.GetActiveScene().name);
        
        // Show exhaust image if assigned
        if (exhaustImage != null)
        {
            exhaustImage.SetActive(true);
            Debug.Log("Exhaust scene started - showing exhaust image");
        }
        else
        {
            Debug.LogWarning("Exhaust image not assigned in inspector!");
        }
        
        // Start the transition sequence
        StartCoroutine(ExhaustSequence());
    }
    
    private System.Collections.IEnumerator ExhaustSequence()
    {
        Debug.Log("Starting exhaust sequence...");
        
        // Wait for the specified duration
        yield return new WaitForSeconds(exhaustDisplayDuration);
        
        Debug.Log("Exhaust sequence complete, transitioning to landing zone...");
        Debug.Log("Target scene name: " + landingZoneScene);
        
        // Check if SceneTransitionManager is available
        if (SceneTransitionManager.Instance != null)
        {
            Debug.Log("Using SceneTransitionManager for transition");
            SceneTransitionManager.Instance.LoadSceneWithFade(landingZoneScene);
        }
        else
        {
            Debug.Log("SceneTransitionManager not available, using direct SceneManager");
            try
            {
                SceneManager.LoadScene(landingZoneScene);
                Debug.Log("SceneManager.LoadScene called successfully");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load scene: " + e.Message);
                // Fallback to direct scene loading without fade
                SceneManager.LoadScene(landingZoneScene);
            }
        }
    }
    
    // Public method to skip the exhaust sequence (for testing or player input)
    public void SkipExhaustSequence()
    {
        if (!isTransitioning)
        {
            StopAllCoroutines();
            StartCoroutine(TransitionToLandingZone());
        }
    }
    
    private System.Collections.IEnumerator TransitionToLandingZone()
    {
        isTransitioning = true;
        
        Debug.Log("Skipping exhaust sequence, transitioning to landing zone...");
        
        // Transition back to landing zone
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadSceneWithFade(landingZoneScene);
        }
        else
        {
            SceneManager.LoadScene(landingZoneScene);
        }
        
        yield return null;
    }
    
    // Handle player input to skip the sequence
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SkipExhaustSequence();
        }
        
        // Debug: Press T to test transition immediately
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Manual transition test triggered");
            StopAllCoroutines();
            StartCoroutine(TransitionToLandingZone());
        }
    }
    
    // Called when a new scene loads
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        
        // If landing zone scene was loaded, position the player
        if (scene.name == landingZoneScene)
        {
            Debug.Log("Landing zone scene loaded, positioning player...");
            
            // Find the PlayerMovement instance and position the player
            PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.PositionPlayerInLandingZone();
                Debug.Log("Player positioned in landing zone from exhaust scene");
            }
            else
            {
                Debug.LogWarning("PlayerMovement not found in landing zone scene!");
            }
        }
    }
} 