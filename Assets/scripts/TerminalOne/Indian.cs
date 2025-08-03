using UnityEngine;
using System.Collections;

public class Indian : MonoBehaviour
{
    public GameObject indianObject;

    [Header("UI Elements")]
    public GameObject noDeodorantMessage; // Assign the message image in inspector
    
    [Header("Settings")]
    public float messageDisplayTime = 2f; // How long the message will be displayed
    
    void Start()
    {
        Debug.Log("Indian character Start() called");
        
        // Ensure the message is hidden at start
        if (noDeodorantMessage != null)
        {
            noDeodorantMessage.SetActive(false);
        }
        
        // Hide character if player has deodorant
        if (GlobalInventoryManager.Instance != null && GlobalInventoryManager.Instance.HasDeodorant())
        {
            indianObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalInventoryManager.Instance.HasDeodorant())
        {
            indianObject.SetActive(false);
        }
    }
    
    // This method is called by the interaction zone when player presses space
    public void OnInteract()
    {
        Debug.Log("Indian interaction triggered!");
        
        // Check if player has deodorant in inventory
        if (GlobalInventoryManager.Instance != null && GlobalInventoryManager.Instance.HasDeodorant())
        {
            // Player has deodorant - make the character disappear
            Debug.Log("Player has deodorant - Indian character disappearing!");
            gameObject.SetActive(false);
        }
        else
        {
            // Player doesn't have deodorant - show message
            Debug.Log("Player doesn't have deodorant - showing message");
            StartCoroutine(ShowNoDeodorantMessage());
        }
    }
    
    private IEnumerator ShowNoDeodorantMessage()
    {
        if (noDeodorantMessage != null)
        {
            // Show the message
            noDeodorantMessage.SetActive(true);
            Debug.Log("No deodorant message displayed");
            
            // Wait for the specified time
            yield return new WaitForSeconds(messageDisplayTime);
            
            // Hide the message
            noDeodorantMessage.SetActive(false);
            Debug.Log("No deodorant message hidden");
        }
        else
        {
            Debug.LogWarning("No deodorant message canvas is not assigned in the inspector!");
        }
    }
}
