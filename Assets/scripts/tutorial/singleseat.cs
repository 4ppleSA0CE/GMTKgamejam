using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class singleseat : MonoBehaviour
{
    // Boolean variables for inventory items
    public bool crowbar = false;
    public bool key = false;

    // Sprite for message when key found
    public GameObject TextMessage_key;

    public float time_of_message = 5f;
    void Start()
    {
        Debug.Log("singleseat script started");
        // Script initialization
    }

    public void OnInteract()
    {
        // Text message sprite active when interacted
        Debug.Log("Key obtained!");
        key = true;
        Debug.Log("Key status: " + key);
        StartCoroutine(ShowKeyMessage()); // Start the coroutine
        // You can trigger animation, dialogue, etc.
    }
    private System.Collections.IEnumerator ShowKeyMessage()
    {
        if (TextMessage_key != null) // Use comparison, not assignment
        {
            TextMessage_key.SetActive(true);
            yield return new WaitForSeconds(time_of_message);
            TextMessage_key.SetActive(false);
        }
    }
}