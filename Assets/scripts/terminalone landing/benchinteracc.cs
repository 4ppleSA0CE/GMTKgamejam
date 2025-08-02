using UnityEngine;
using System.Collections;

public class benchinteracc : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject benchMessageCanvas; // Assign the canvas image in inspector
    
    [Header("Settings")]
    public float messageDisplayTime = 2f; // How long the message will be displayed
    
    void Start()
    {
        // Ensure the message canvas is hidden at start
        if (benchMessageCanvas != null)
        {
            benchMessageCanvas.SetActive(false);
        }
    }

    // This method is called by the interaction zone when player presses space
    public void OnInteract()
    {
        Debug.Log("Bench interaction triggered!");
        StartCoroutine(ShowBenchMessage());
    }
    
    private IEnumerator ShowBenchMessage()
    {
        if (benchMessageCanvas != null)
        {
            // Show the message
            benchMessageCanvas.SetActive(true);
            Debug.Log("Bench message displayed");
            
            // Wait for the specified time
            yield return new WaitForSeconds(messageDisplayTime);
            
            // Hide the message
            benchMessageCanvas.SetActive(false);
            Debug.Log("Bench message hidden");
        }
        else
        {
            Debug.LogWarning("Bench message canvas is not assigned in the inspector!");
        }
    }
}
