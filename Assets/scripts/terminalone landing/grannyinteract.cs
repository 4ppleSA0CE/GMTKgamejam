using UnityEngine;
using System.Collections;

public class grannyinteract : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject grannyMessageCanvas; // Assign the canvas image in inspector
    
    [Header("Settings")]
    public float messageDisplayTime = 3f; // How long the message will be displayed
    
    void Start()
    {
        // Ensure the message canvas is hidden at start
        if (grannyMessageCanvas != null)
        {
            grannyMessageCanvas.SetActive(false);
        }
    }

    // This method is called by the interaction zone when player presses space
    public void OnInteract()
    {
        Debug.Log("Granny interaction triggered!");
        StartCoroutine(ShowGrannyMessage());
    }
    
    private IEnumerator ShowGrannyMessage()
    {
        if (grannyMessageCanvas != null)
        {
            // Show the message
            grannyMessageCanvas.SetActive(true);
            Debug.Log("Granny message displayed");
            
            // Wait for the specified time
            yield return new WaitForSeconds(messageDisplayTime);
            
            // Hide the message
            grannyMessageCanvas.SetActive(false);
            Debug.Log("Granny message hidden");
        }
        else
        {
            Debug.LogWarning("Granny message canvas is not assigned in the inspector!");
        }
    }
}
