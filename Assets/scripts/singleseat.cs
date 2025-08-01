using UnityEngine;

public class singleseat : MonoBehaviour
{
    void Start(){
        Debug.Log("singleseat script started");
        // Script initialization
    }
    
    public void OnInteract()
    {
        Debug.Log("Door opened!");
        // You can trigger animation, dialogue, etc.
    }
}
