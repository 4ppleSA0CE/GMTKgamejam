using UnityEngine;

public class emptyseats : MonoBehaviour
{
    void Start(){
        Debug.Log("emptyseats script started");
        // Script initialization
    }
    
    public void OnInteract()
    {
        Debug.Log("wrong seat!");
        // You can trigger animation, dialogue, etc.
    }
}
