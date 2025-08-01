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
        float elapsedTime = 0f;
        while (elapsedTime <= time_of_message)
            elapsedTime += Time.deltaTime;
            TextMessage_key.SetActive(true);
        // Text message sprite active when interacted
            Debug.Log("Key obtained!");
        key = true;
        Debug.Log("Key status: " + key);
        // You can trigger animation, dialogue, etc.
    }
}
