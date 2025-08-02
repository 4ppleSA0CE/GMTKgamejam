using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class Puzzle2Manager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public int totalTrashCans = 15;
    public int piecesToFind = 3;
    public int maxTries = 7;
    
    [Header("Scene Management")]
    public string landingZoneScene = "Landing zone";
    public float sceneTransitionDelay = 2f;
    public float fadeToBlackDuration = 5f;
    
    // Game state
    private int currentTries;
    private int piecesFound;
    private bool gameWon = false;
    private bool gameLost = false;
    
    // References
    private List<TrashCan> trashCans = new List<TrashCan>();
    private Canvas fadeCanvas;
    private Image fadeImage;
    
    void Start()
    {
        InitializePuzzle();
        SetupTrashCans();
        SetupFadeCanvas();
    }
    
    void InitializePuzzle()
    {
        currentTries = maxTries;
        piecesFound = 0;
        gameWon = false;
        gameLost = false;
        
        Debug.Log("Puzzle initialized. Find " + piecesToFind + " pieces in " + totalTrashCans + " trash cans with " + maxTries + " tries.");
    }
    
    void SetupFadeCanvas()
    {
        // Create fade canvas
        GameObject canvasObj = new GameObject("FadeCanvas");
        canvasObj.transform.SetParent(transform);
        fadeCanvas = canvasObj.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 999;
        
        // Create fade image
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);
        fadeImage = imageObj.AddComponent<Image>();
        
        // Set image to fill screen
        RectTransform rectTransform = imageObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        // Start transparent
        fadeImage.color = new Color(0f, 0f, 0f, 0f);
        fadeImage.raycastTarget = false;
    }
    
    void SetupTrashCans()
    {
        // Find all trash cans in the scene
        TrashCan[] foundTrashCans = FindObjectsByType<TrashCan>(FindObjectsSortMode.None);
        trashCans.Clear();
        
        foreach (TrashCan trashCan in foundTrashCans)
        {
            trashCans.Add(trashCan);
        }
        
        // Ensure we have the right number of trash cans
        if (trashCans.Count != totalTrashCans)
        {
            Debug.LogWarning("Expected " + totalTrashCans + " trash cans, but found " + trashCans.Count);
        }
        
        // Randomly place pieces
        PlacePiecesRandomly();
    }
    
    void PlacePiecesRandomly()
    {
        // Create a list of indices for random selection
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < trashCans.Count; i++)
        {
            availableIndices.Add(i);
        }
        
        // Randomly select indices for pieces
        for (int i = 0; i < piecesToFind; i++)
        {
            if (availableIndices.Count > 0)
            {
                int randomIndex = Random.Range(0, availableIndices.Count);
                int trashCanIndex = availableIndices[randomIndex];
                
                // Place piece in this trash can
                trashCans[trashCanIndex].SetupTrashCan(true);
                availableIndices.RemoveAt(randomIndex);
                
                Debug.Log("Placed piece " + (i + 1) + " in trash can " + trashCanIndex);
            }
        }
        
        // Set remaining trash cans as empty
        for (int i = 0; i < trashCans.Count; i++)
        {
            if (!trashCans[i].containsGarbagePiece)
            {
                trashCans[i].SetupTrashCan(false);
            }
        }
    }
    
    public void TrashCanSearched()
    {
        if (gameWon || gameLost) return;
        
        currentTries--;
        
        Debug.Log("Try used! " + currentTries + "/" + maxTries + " tries remaining.");
        
        // Check if out of tries
        if (currentTries <= 0)
        {
            GameLost();
        }
    }
    
    public void PieceFound()
    {
        if (gameWon || gameLost) return;
        
        piecesFound++;
        
        Debug.Log("Piece found! " + piecesFound + "/" + piecesToFind + " pieces collected.");
        
        // Check if all pieces found
        if (piecesFound >= piecesToFind)
        {
            GameWon();
        }
    }
    
    void GameWon()
    {
        gameWon = true;
        Debug.Log("Congratulations! You found all " + piecesToFind + " pieces!");
        
        // Start fade to black
        StartCoroutine(FadeToBlackAndTransition());
    }
    
    void GameLost()
    {
        gameLost = true;
        Debug.Log("Game Over! You ran out of tries. The stench was too much!");
        
        // Transition back to landing zone after delay
        StartCoroutine(TransitionToLandingZone());
    }
    
    System.Collections.IEnumerator FadeToBlackAndTransition()
    {
        Debug.Log("Starting fade to black...");
        
        // Fade to black over 5 seconds
        float elapsedTime = 0f;
        while (elapsedTime < fadeToBlackDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeToBlackDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        
        // Ensure screen is completely black
        fadeImage.color = new Color(0f, 0f, 0f, 1f);
        
        Debug.Log("Fade complete. Puzzle completed!");
    }
    
    System.Collections.IEnumerator TransitionToLandingZone()
    {
        yield return new WaitForSeconds(sceneTransitionDelay);
        
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
    
    // Public method to reset puzzle (for testing or restart)
    public void ResetPuzzle()
    {
        // Reset all trash cans
        foreach (TrashCan trashCan in trashCans)
        {
            trashCan.ResetTrashCan();
        }
        
        // Reinitialize puzzle
        InitializePuzzle();
        SetupTrashCans();
    }
} 