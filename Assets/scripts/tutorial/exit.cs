using UnityEngine;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour
{
    // Reference to the singleseat script to access the crowbar variable
    public singleseat playerInventory;
    
    // Scene transition settings
    public string nextSceneName = "NextScene"; // Change this to your next scene name
    public float fadeDuration = 2f;
    
    void Start()
    {
        // Find the singleseat script in the scene
        if (playerInventory == null)
        {
            playerInventory = FindFirstObjectByType<singleseat>();
        }
    }

    public void OnInteract()
    {
        if (playerInventory != null && playerInventory.crowbar)
        {
            Debug.Log("Exit unlocked! Transitioning to next scene...");
            StartCoroutine(FadeToBlackAndLoadScene());
        }
        else
        {
            Debug.Log("Exit is blocked. You need a crowbar to proceed.");
        }
    }
    
    private System.Collections.IEnumerator FadeToBlackAndLoadScene()
    {
        // Create a black overlay
        GameObject fadeObject = new GameObject("FadeOverlay");
        Canvas canvas = fadeObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // Ensure it's on top
        
        // Create the black image
        GameObject imageObject = new GameObject("BlackImage");
        imageObject.transform.SetParent(fadeObject.transform, false);
        UnityEngine.UI.Image blackImage = imageObject.AddComponent<UnityEngine.UI.Image>();
        blackImage.color = new Color(0, 0, 0, 0); // Start transparent
        
        // Set the image to fill the screen
        RectTransform rectTransform = imageObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        // Fade to black
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        
        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
