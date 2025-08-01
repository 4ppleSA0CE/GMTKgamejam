using UnityEngine;

public class singleseat : MonoBehaviour
{
    // Boolean variables for inventory items
    public bool crowbar = false;
    public bool key = false;
    
    void Start(){
        Debug.Log("singleseat script started");
        // Script initialization
    }
    
    public void OnInteract()
    {
        Debug.Log("Key obtained!");
        key = true;
        Debug.Log("Key status: " + key);
        // You can trigger animation, dialogue, etc.
    }
}
