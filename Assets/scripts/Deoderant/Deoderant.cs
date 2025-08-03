using UnityEngine;

public class Deoderant : MonoBehaviour
{
    public GameObject deoderantObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInteract()
    {
        // Add deodorant to global inventory
        if (GlobalInventoryManager.Instance != null)
        {
            GlobalInventoryManager.Instance.AddDeodorant();
            deoderantObject.SetActive(true);
        }
        
        // Hide this interactable object
        gameObject.SetActive(false);
        
        Debug.Log("Deodorant collected and added to inventory!");
    }
}
