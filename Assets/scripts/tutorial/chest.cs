using UnityEngine;

public class chest : MonoBehaviour
{
    // Reference to the singleseat script to access the key variable
    public singleseat playerInventory;
    
    void Start(){
        Debug.Log("Chest script started");
        // Find the singleseat script in the scene
        if (playerInventory == null)
        {
            playerInventory = FindFirstObjectByType<singleseat>();
        }
    }
    
    public void OnInteract()
    {
        if (playerInventory != null && playerInventory.key)
        {
            Debug.Log("Chest opened with key!");
            playerInventory.crowbar = true;
            Debug.Log("Crowbar obtained! Crowbar status: " + playerInventory.crowbar);
            // You can trigger chest opening animation, sound, etc.
        }
        else
        {
            Debug.Log("Chest is locked. You need a key to open it.");
        }
    }
}
