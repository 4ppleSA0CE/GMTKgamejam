using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class subwayexit : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject riddleCanvas; // Assign the riddle UI canvas in inspector
    public InputField answerInputField; // Assign the input field in inspector
    public Text feedbackText; // Assign the feedback text in inspector
    
    [Header("Settings")]
    public string correctAnswer = "circle";
    public string successMessage = "Correct! You may now exit the subway.";
    public string wrongAnswerMessage = "That's not quite right. Try again.";
    
    private bool isRiddleActive = false;
    
    void Start()
    {
        // Ensure the riddle canvas is hidden at start
        if (riddleCanvas != null)
        {
            riddleCanvas.SetActive(false);
        }
        
        // Set up input field listener for Enter key
        if (answerInputField != null)
        {
            answerInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        }
    }

    // This method is called by the interaction zone when player presses space
    public void OnInteract()
    {
        Debug.Log("Subway exit interaction triggered!");
        ToggleRiddle();
    }
    
    private void ToggleRiddle()
    {
        if (riddleCanvas != null)
        {
            isRiddleActive = !isRiddleActive;
            riddleCanvas.SetActive(isRiddleActive);
            
            if (isRiddleActive)
            {
                Debug.Log("Riddle displayed - enter your answer");
                // Clear previous input and feedback
                if (answerInputField != null)
                {
                    answerInputField.text = "";
                    answerInputField.Select();
                    answerInputField.ActivateInputField();
                }
                if (feedbackText != null)
                {
                    feedbackText.text = "";
                }
            }
            else
            {
                Debug.Log("Riddle hidden");
            }
        }
        else
        {
            Debug.LogWarning("Riddle canvas is not assigned in the inspector!");
        }
    }
    
    private void OnInputFieldEndEdit(string value)
    {
        // Check answer when Enter is pressed
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckAnswer();
        }
    }
    
    private void CheckAnswer()
    {
        if (answerInputField != null)
        {
            string playerAnswer = answerInputField.text.Trim().ToLower();
            
            if (playerAnswer == correctAnswer.ToLower())
            {
                // Correct answer check please
                if (feedbackText != null)
                {
                    feedbackText.text = successMessage;
                    feedbackText.color = Color.green;
                }
                Debug.Log("Correct answer! Transitioning to terminalone landing scene.");
                
                // Use SceneTransitionManager to transition to terminalone landing scene
                StartCoroutine(TransitionToNextScene(2f));
            }
            else
            {
                // Wrong answer
                if (feedbackText != null)
                {
                    feedbackText.text = wrongAnswerMessage;
                    feedbackText.color = Color.red;
                }
                Debug.Log("Wrong answer: " + playerAnswer);
                
                // Clear the input field for another attempt
                answerInputField.text = "";
                answerInputField.Select();
                answerInputField.ActivateInputField();
            }
        }
    }
    
    private IEnumerator TransitionToNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Use SceneTransitionManager to load the terminalone landing scene
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
