using UnityEngine;

public class interactionzone : MonoBehaviour
{
    public GameObject spaceprompt; // Assign in inspector

    private GameObject currentTarget;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("InteractionZone script started");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed while in range of: " + currentTarget.name);
            currentTarget.SendMessage("OnInteract", SendMessageOptions.DontRequireReceiver);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("=== ANY COLLISION DETECTED ===");
        Debug.Log("Trigger entered with: " + other.name + " (Tag: " + other.tag + ")");
        if (other.CompareTag("Interactable"))
        {
            Debug.Log("Setting current target to: " + other.name);
            currentTarget = other.gameObject;
            if (spaceprompt != null)
            {
                spaceprompt.SetActive(true);
                Debug.Log("Space prompt activated");
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger exited with: " + other.name);
        if (other.gameObject == currentTarget)
        {
            Debug.Log("Clearing current target");
            currentTarget = null;
            if (spaceprompt != null)
            {
                spaceprompt.SetActive(false);
                Debug.Log("Space prompt deactivated");
            }
        }
    }
}
