using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    
    [Header("Fade Settings")]
    public float fadeDuration = 2f;
    public Color fadeColor = Color.black;
    
    private Canvas fadeCanvas;
    private Image fadeImage;
    private bool isTransitioning = false;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupFadeCanvas();
            
            // Subscribe to scene load events
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void OnEnable()
    {
        // Ensure setup happens even if Awake wasn't called
        if (Instance == this && fadeCanvas == null)
        {
            SetupFadeCanvas();
        }
    }
    
    void Start()
    {
        // Only fade in if this is a new scene (not when first created)
        if (fadeImage != null && !isTransitioning)
        {
            // Ensure we start completely black
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
            StartCoroutine(FadeIn());
        }
    }
    
    public void SetupFadeCanvas()
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
        
        // Start transparent for fade out, will be set to black for fade in
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        
        // Ensure the fade image is visible from the start
        fadeImage.raycastTarget = false; // Prevent blocking UI interactions
    }
    
    public void LoadSceneWithFade(string sceneName)
    {
        if (!isTransitioning)
        {
            // Ensure fade canvas is set up
            if (fadeCanvas == null)
            {
                SetupFadeCanvas();
            }
            StartCoroutine(FadeOutAndLoadScene(sceneName));
        }
    }
    
    private System.Collections.IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        isTransitioning = true;
        
        // Fade out
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }
        
        // Ensure screen is completely black before loading scene
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
        
        // Load scene
        SceneManager.LoadScene(sceneName);
        
        // Fade in will happen automatically in Start() of the new scene
        isTransitioning = false;
    }
    
    // This method will be called when a new scene loads
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeImage != null)
        {
            // Ensure we start completely black
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
            StartCoroutine(FadeIn());
        }
    }
    
    private System.Collections.IEnumerator FadeIn()
    {
        // Start from black
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
        
        // Fade in
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }
        
        // Ensure fully transparent
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
    }
} 