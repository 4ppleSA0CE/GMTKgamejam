using UnityEngine;

public class interactionzone : MonoBehaviour
{
    // public GameObject ePrompt; // Assign in inspector
    private GameObject currentTarget;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("InteractionZone script started");
        // ePrompt.SetActive(false);
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
        Debug.Log("Trigger entered with: " + other.name + " (Tag: " + other.tag + ")");
        if (other.CompareTag("Interactable"))
        {
            Debug.Log("Setting current target to: " + other.name);
            currentTarget = other.gameObject;
            // ePrompt.SetActive(true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Regular collision detection (if needed)
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Staying in trigger (if needed)
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentTarget)
        {
            currentTarget = null;
            // ePrompt.SetActive(false);
        }
    }
}
