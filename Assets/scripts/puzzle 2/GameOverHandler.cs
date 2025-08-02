using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverHandler : MonoBehaviour
{
    [Header("Scene Management")]
    public string landingZoneScene = "Landing zone";
    public float sceneTransitionDelay = 1f;
    
    private bool isFainting = false;
    private Puzzle2Manager puzzleManager;
    
    void Start()
    {
        // Find puzzle manager
        puzzleManager = FindFirstObjectByType<Puzzle2Manager>();
    }
    
    // Called by Puzzle2Manager when game is lost
    public void StartGameOverSequence()
    {
        if (!isFainting)
        {
            StartCoroutine(GameOverSequence());
        }
    }
    
    IEnumerator GameOverSequence()
    {
        isFainting = true;
        
        Debug.Log("Starting game over sequence...");
        
        yield return new WaitForSeconds(1f);
        
        Debug.Log("The stench is overwhelming!");
        
        yield return new WaitForSeconds(2f);
        
        Debug.Log("Player is fainting from the stench!");
        
        yield return new WaitForSeconds(3f);
        
        Debug.Log("Fading to black...");
        
        yield return new WaitForSeconds(sceneTransitionDelay);
        
        Debug.Log("Returning to landing zone...");
        
        // Use SceneTransitionManager if available
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadSceneWithFade(landingZoneScene);
        }
        else
        {
            SceneManager.LoadScene(landingZoneScene);
        }
    }
    
    // Public method to reset game over state (for testing)
    public void ResetGameOverState()
    {
        isFainting = false;
    }
    
    // Public method to check if currently fainting
    public bool IsFainting()
    {
        return isFainting;
    }
    
    // Method to handle player input during game over (skip sequence)
    void Update()
    {
        if (isFainting && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Player skipped game over sequence");
            StopAllCoroutines();
            StartCoroutine(ReturnToLandingZone());
        }
    }
    
    IEnumerator ReturnToLandingZone()
    {
        Debug.Log("Returning to landing zone...");
        
        // Use SceneTransitionManager if available
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
} 