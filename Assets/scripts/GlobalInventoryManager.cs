using UnityEngine;

public class GlobalInventoryManager : MonoBehaviour
{
    public static GlobalInventoryManager Instance;

    [Header("Inventory Items")]
    public bool hasDeodorant = false;

    void Awake()
    {
        Debug.Log("GlobalInventoryManager Awake called for: " + gameObject.name);

        // Singleton pattern - only one instance should exist
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GlobalInventoryManager instance set and DontDestroyOnLoad applied");
        }
        else
        {
            // If another instance exists, destroy this one
            Debug.Log("GlobalInventoryManager duplicate found, destroying: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        Debug.Log("GlobalInventoryManager being destroyed: " + gameObject.name);
    }

    public void AddDeodorant()
    {
        hasDeodorant = true;
        Debug.Log("Deodorant added to inventory. Current state: " + hasDeodorant);
    }

    public bool HasDeodorant()
    {
        Debug.Log("Checking deodorant inventory: " + hasDeodorant);
        return hasDeodorant;
    }

    public void RemoveDeodorant()
    {
        hasDeodorant = false;
        Debug.Log("Deodorant removed from inventory. Current state: " + hasDeodorant);
    }
} 