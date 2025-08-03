using UnityEngine;
using System.Collections;

public class paper : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject paperMessageCanvas; // Assign the canvas image in inspector
    
    private bool isImageActive = false;
    
    void Start()
    {
        // Ensure the message canvas is hidden at start
        if (paperMessageCanvas != null)
        {
            paperMessageCanvas.SetActive(false);
        }
    }

    // This method is called by the interaction zone when player presses space
    public void OnInteract()
    {
        Debug.Log("Paper interaction triggered!");
        TogglePaperMessage();
    }
    
    private void TogglePaperMessage()
    {
        if (paperMessageCanvas != null)
        {
            isImageActive = !isImageActive;
            paperMessageCanvas.SetActive(isImageActive);
            
            if (isImageActive)
            {
                Debug.Log("Paper message displayed - press space again to close");
            }
            else
            {
                Debug.Log("Paper message hidden");
            }
        }
        else
        {
            Debug.LogWarning("Paper message canvas is not assigned in the inspector!");
        }
    }
}
