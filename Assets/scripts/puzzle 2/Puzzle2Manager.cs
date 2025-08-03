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
    public string landingZoneScene = "Terminalone landing";
    public string endSceneName = "EndScene";
    public float sceneTransitionDelay = 2f;
    public float fadeDuration = 2f;

    [Header("Game Over UI")]
    public GameObject gameOverImage; // Assign the game over image in inspector

    [Header("Piece Found UI")]
    public GameObject piece1Image; // Image for first piece found
    public GameObject piece2Image; // Image for second piece found
    public GameObject piece3Image; // Image for third piece found
    public GameObject noPieceFoundImage; // Image for when no piece is found in trash can
    public float pieceImageDisplayTime = 0.5f; // How long to show each piece image

    [Header("Inventory UI")]
    public GameObject inventoryPiece1; // Shown when first piece is found
    public GameObject inventoryPiece2; // Shown when second piece is found
    public GameObject inventoryPiece3; // Shown when third piece is found

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
        SetupGameOverImage();
        SetupInventoryImages();
        EnsureSceneTransitionManager();
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
        Debug.Log("Setting up fade canvas for puzzle 2...");

        // Create fade canvas
        GameObject canvasObj = new GameObject("FadeCanvas");
        canvasObj.transform.SetParent(transform);
        fadeCanvas = canvasObj.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 100; // Lower than game over image

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

        Debug.Log("Fade canvas setup complete for puzzle 2");
    }

    void SetupGameOverImage()
    {
        // Hide the game over image at start
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(false);

            // Ensure the game over image has a higher sorting order than the fade
            Canvas gameOverCanvas = gameOverImage.GetComponentInParent<Canvas>();
            if (gameOverCanvas != null)
            {
                gameOverCanvas.sortingOrder = 200; // Higher than fade canvas (100)
                Debug.Log("Game over canvas sorting order set to: " + gameOverCanvas.sortingOrder);
            }

            Debug.Log("Game over image hidden at start");
        }
        else
        {
            Debug.LogWarning("Game over image not assigned in inspector!");
        }

        // Setup piece found images
        SetupPieceFoundImages();
    }

    void SetupPieceFoundImages()
    {
        // Hide all piece found images at start
        if (piece1Image != null)
        {
            piece1Image.SetActive(false);
            SetCanvasSortingOrder(piece1Image, 300); // Higher than game over image
        }

        if (piece2Image != null)
        {
            piece2Image.SetActive(false);
            SetCanvasSortingOrder(piece2Image, 300);
        }

        if (piece3Image != null)
        {
            piece3Image.SetActive(false);
            SetCanvasSortingOrder(piece3Image, 300);
        }

        if (noPieceFoundImage != null)
        {
            noPieceFoundImage.SetActive(false);
            SetCanvasSortingOrder(noPieceFoundImage, 300);
        }

        Debug.Log("Piece found images hidden at start");
    }

    void SetCanvasSortingOrder(GameObject uiElement, int sortingOrder)
    {
        Canvas canvas = uiElement.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = sortingOrder;
            Debug.Log("Canvas sorting order set to: " + sortingOrder + " for: " + uiElement.name);
        }
    }

    void ShowPieceFoundImage(int pieceNumber)
    {
        GameObject imageToShow = null;

        switch (pieceNumber)
        {
            case 1:
                imageToShow = piece1Image;
                Debug.Log("Showing image for piece 1");
                break;
            case 2:
                imageToShow = piece2Image;
                Debug.Log("Showing image for piece 2");
                break;
            case 3:
                imageToShow = piece3Image;
                Debug.Log("Showing image for piece 3");
                break;
            default:
                Debug.LogWarning("Unknown piece number: " + pieceNumber);
                return;
        }

        if (imageToShow != null)
        {
            StartCoroutine(ShowImageForDuration(imageToShow));
        }
        else
        {
            Debug.LogWarning("Piece " + pieceNumber + " image not assigned in inspector!");
        }
    }

    void ShowNoPieceFoundImage()
    {
        if (noPieceFoundImage != null)
        {
            StartCoroutine(ShowImageForDuration(noPieceFoundImage));
            Debug.Log("Showing 'no piece found' image");
        }
        else
        {
            Debug.LogWarning("No piece found image not assigned in inspector!");
        }
    }

    System.Collections.IEnumerator ShowImageForDuration(GameObject image)
    {
        // Show the image
        image.SetActive(true);
        Debug.Log("Piece found image displayed: " + image.name);

        // Wait for the specified duration
        yield return new WaitForSeconds(pieceImageDisplayTime);

        // Hide the image
        image.SetActive(false);
        Debug.Log("Piece found image hidden: " + image.name);
    }

    void SetupTrashCans()
    {
        // Find all trash cans in the scene
        TrashCan[] foundTrashCans = FindObjectsByType<TrashCan>(FindObjectsSortMode.None);
        trashCans.Clear();

        Debug.Log("=== TRASH CAN SETUP DEBUG ===");
        Debug.Log("Found " + foundTrashCans.Length + " trash cans with TrashCan script:");

        foreach (TrashCan trashCan in foundTrashCans)
        {
            trashCans.Add(trashCan);
            Debug.Log("  - " + trashCan.gameObject.name);
        }

        // Ensure we have the right number of trash cans
        if (trashCans.Count != totalTrashCans)
        {
            Debug.LogWarning("Expected " + totalTrashCans + " trash cans, but found " + trashCans.Count);
            Debug.LogWarning("One trash can is missing the TrashCan.cs script!");
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

        Debug.Log("=== GARBAGE PIECE PLACEMENT ===");

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

                Debug.Log("GARBAGE PIECE " + (i + 1) + " placed in: " + trashCans[trashCanIndex].gameObject.name);
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

        Debug.Log("=== GARBAGE PIECE PLACEMENT COMPLETE ===");
    }

    public void TrashCanSearched(bool pieceFound)
    {
        if (gameWon || gameLost) return;

        currentTries--;

        Debug.Log("Try used! " + currentTries + "/" + maxTries + " tries remaining.");

        // Only show "no piece found" image if no piece was actually found
        if (!pieceFound)
        {
            ShowNoPieceFoundImage();
        }

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

        // Show appropriate piece found image
        ShowPieceFoundImage(piecesFound);

        UpdateInventoryUI(piecesFound);

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

        // Transition to EndScene using the SceneTransitionManager
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadSceneWithFade(endSceneName);
        }
        else
        {
            // Fallback: use local fade system if SceneTransitionManager is not available
            StartCoroutine(FadeOutAndLoadScene(endSceneName));
        }
    }

    void GameLost()
    {
        gameLost = true;
        Debug.Log("You ran out of tries. The stench was too much!");

        // Show game over image
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(true);
            Debug.Log("Game over image displayed");
        }

        // Use local fade system
        StartCoroutine(FadeOutAndLoadScene(landingZoneScene));
    }

        System.Collections.IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        Debug.Log("Starting fade to black for scene transition...");
        
        if (fadeImage == null)
        {
            Debug.LogError("Fade image is null! Cannot fade to black.");
            SceneManager.LoadScene(sceneName);
            yield break;
        }
        
        // Fade to black
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        
        // Ensure screen is completely black
        fadeImage.color = new Color(0f, 0f, 0f, 1f);
        Debug.Log("Fade complete. Loading scene: " + sceneName);
        
        // Wait a moment on black screen
        yield return new WaitForSeconds(sceneTransitionDelay);
        
        // Load the scene
        SceneManager.LoadScene(sceneName);
    }
    
    System.Collections.IEnumerator FadeToBlack()
    {
        Debug.Log("Starting fade to black for game win...");
        
        if (fadeImage == null)
        {
            Debug.LogError("Fade image is null! Cannot fade to black.");
            yield break;
        }
        
        // Fade to black
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        
        // Ensure screen is completely black
        fadeImage.color = new Color(0f, 0f, 0f, 1f);
        Debug.Log("Game won - screen faded to black");
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

        // Hide game over image if it was showing
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(false);
        }
    }
    void SetupInventoryImages()
    {
        if (inventoryPiece1 != null) inventoryPiece1.SetActive(false);
        if (inventoryPiece2 != null) inventoryPiece2.SetActive(false);
        if (inventoryPiece3 != null) inventoryPiece3.SetActive(false);
        Debug.Log("Inventory images hidden at start");
    }
    void UpdateInventoryUI(int pieceCount)
    {
        if (pieceCount == 1 && inventoryPiece1 != null)
        {
            inventoryPiece1.SetActive(true);
            Debug.Log("Inventory updated: Piece 1 shown");
        }
        else if (pieceCount == 2 && inventoryPiece2 != null)
        {
            inventoryPiece2.SetActive(true);
            inventoryPiece1.SetActive(false);
            Debug.Log("Inventory updated: Piece 2 shown, Piece 1 hidden");
        }
        else if (pieceCount == 3 && inventoryPiece3 != null)
        {
            inventoryPiece3.SetActive(true);
            inventoryPiece2.SetActive(false);
            Debug.Log("Inventory updated: Piece 3 shown, Piece 2 hidden");
        }
    }

    void EnsureSceneTransitionManager()
    {
        // Check if SceneTransitionManager already exists
        if (SceneTransitionManager.Instance == null)
        {
            // Create a new GameObject with SceneTransitionManager
            GameObject transitionManagerObj = new GameObject("SceneTransitionManager");
            transitionManagerObj.AddComponent<SceneTransitionManager>();
            Debug.Log("Created SceneTransitionManager for puzzle 2 scene");
        }
        else
        {
            Debug.Log("SceneTransitionManager already exists");
        }
    }
} 