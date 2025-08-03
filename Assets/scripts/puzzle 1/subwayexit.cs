using UnityEngine;
using UnityEngine.SceneManagement;

public class subwayexit : MonoBehaviour
{
    void Start()
    {
        // No initialization needed
    }

    // This method is called by the interaction zone when player presses space
    public void OnInteract()
    {
        Debug.Log("Subway exit interaction triggered! Transitioning to terminalone landing scene.");
        
        // Use SceneTransitionManager to transition to terminalone landing scene
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadSceneWithFade("terminalone landing");
        }
        else
        {
            Debug.LogError("SceneTransitionManager.Instance is null! Falling back to direct scene load.");
            SceneManager.LoadScene("terminalone landing");
        }
    }
}
