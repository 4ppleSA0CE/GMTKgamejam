using UnityEngine;

public class emptyseats : MonoBehaviour
{
    // Sprite for message when key found
    public GameObject TextMessage_NoKey;

    public float time_of_message = 1f;
    // How long the message will be displayed
    void Start()
    {
        Debug.Log("emptyseats script started");
        // Script initialization
    }

    public void OnInteract()
    {
        Debug.Log("wrong seat!");
        // You can trigger animation, dialogue, etc.
        StartCoroutine(ShowNoKeyMessage()); // Start the coroutine
    }
    private System.Collections.IEnumerator ShowNoKeyMessage()
    {
        if (TextMessage_NoKey != null) // Use comparison, not assignment
        {
            TextMessage_NoKey.SetActive(true);
            yield return new WaitForSeconds(time_of_message);
            TextMessage_NoKey.SetActive(false);
        }
    }
}