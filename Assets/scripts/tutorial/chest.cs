using UnityEngine;

public class chest : MonoBehaviour
{
    // Sprite for message when chest unlocked
    public GameObject TextMessage_Chest_Unlocked;

    // Sprite for message when chest locked
    public GameObject TextMessage_Chest_Locked;

    public float time_of_message = 1f;
    // How long the message will be displayed

    public GameObject crowbarSprite;

    // Reference to the singleseat script to access the key variable
    public singleseat playerInventory;

    void Start()
    {
        Debug.Log("Chest script started");
        // Find the singleseat script in the scene
        if (playerInventory == null)
        {
            playerInventory = FindFirstObjectByType<singleseat>();
        }
    }

    public void OnInteract()
    {
        if (playerInventory != null && playerInventory.key)
        {
            Debug.Log("Chest opened with key!");
            playerInventory.crowbar = true;
            keySprite.SetActive(false);
            crowbarSprite.SetActive(true);
            Debug.Log("Crowbar obtained! Crowbar status: " + playerInventory.crowbar);
            // You can trigger chest opening animation, sound, etc.
            StartCoroutine(ShowChestUnlockedMessage()); // Start the coroutine
        }
        else
        {
            Debug.Log("Chest is locked. You need a key to open it.");
            StartCoroutine(ShowChestLockedMessage()); // Start the coroutine
        }
    }
    private System.Collections.IEnumerator ShowChestUnlockedMessage()
    {
        if (TextMessage_Chest_Unlocked != null) // Use comparison, not assignment
        {
            TextMessage_Chest_Unlocked.SetActive(true);
            yield return new WaitForSeconds(time_of_message);
            TextMessage_Chest_Unlocked.SetActive(false);
        }
    }
    private System.Collections.IEnumerator ShowChestLockedMessage()
    {
        if (TextMessage_Chest_Locked != null) // Use comparison, not assignment
        {
            TextMessage_Chest_Locked.SetActive(true);
            yield return new WaitForSeconds(time_of_message);
            TextMessage_Chest_Locked.SetActive(false);
        }
    }
}
