using UnityEngine;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour
{
    // Reference to the singleseat script to access the crowbar variable
    public singleseat playerInventory;
    
    // Scene transition settings
    public string nextSceneName = "Landing zone"; // Change this to your next scene name
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
            
            // Check if SceneTransitionManager exists, create if it doesn't
            if (SceneTransitionManager.Instance == null)
            {
                GameObject managerObj = new GameObject("SceneTransitionManager");
                SceneTransitionManager manager = managerObj.AddComponent<SceneTransitionManager>();
                // Force the setup to happen immediately
                manager.SetupFadeCanvas();
            }
            
            SceneTransitionManager.Instance.LoadSceneWithFade(nextSceneName);
        }
        else
        {
            Debug.Log("Exit is blocked. You need a crowbar to proceed.");
        }
    }
}
