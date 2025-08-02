using UnityEngine;

public class bench : MonoBehaviour
{
    // Sprite for message when bench is interacted with
    public GameObject TextMessage_bench;

    public float time_of_message = 1f;
    // How long the message will be displayed

    void Start()
    {
        Debug.Log("Bench script started");
        // Script initialization
    }

    public void OnInteract()
    {
        Debug.Log("Bench interacted with!");
        StartCoroutine(ShowBenchMessage()); // Start the coroutine
        // You can trigger animation, sound, etc.
    }

    private System.Collections.IEnumerator ShowBenchMessage()
    {
        if (TextMessage_bench != null) // Use comparison, not assignment
        {
            TextMessage_bench.SetActive(true);
            yield return new WaitForSeconds(time_of_message);
            TextMessage_bench.SetActive(false);
        }
    }
} 