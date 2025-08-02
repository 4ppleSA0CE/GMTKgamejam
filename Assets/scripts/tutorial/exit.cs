using UnityEngine;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour
{
    // Sprite for message when exit is opened
    public GameObject TextMessage_Exit_Opened;

    // Sprite for message when exit is blocked
    public GameObject TextMessage_Exit_Blocked;

    // How long the message will be displayed
    public float time_of_message = 1f;

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
                StartCoroutine(ShowExitOpenedMessage()); // Start the coroutine
            }

            SceneTransitionManager.Instance.LoadSceneWithFade(nextSceneName);
        }
        else
        {
            Debug.Log("Exit is blocked. You need a crowbar to proceed.");
            StartCoroutine(ShowExitBlockedMessage()); // Start the coroutine
        }
    }
    private System.Collections.IEnumerator ShowExitOpenedMessage()
    {
        if (TextMessage_Exit_Opened != null) // Use comparison, not assignment
        {
            TextMessage_Exit_Opened.SetActive(true);
            yield return new WaitForSeconds(time_of_message);
            TextMessage_Exit_Opened.SetActive(false);
        }
    }
    private System.Collections.IEnumerator ShowExitBlockedMessage()
    {
        if (TextMessage_Exit_Blocked != null) // Use comparison, not assignment
        {
            TextMessage_Exit_Blocked.SetActive(true);
            yield return new WaitForSeconds(time_of_message);
            TextMessage_Exit_Blocked.SetActive(false);
        }
    }
}
