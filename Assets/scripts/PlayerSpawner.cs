using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Prefab")]
    public GameObject playerPrefab; // Assign a player prefab in inspector
    
    [Header("Spawn Settings")]
    public Vector3 spawnPosition = new Vector3(-6.89f, 0.73f, 0f);
    public bool spawnIfNoPlayer = true;
    
    void Start()
    {
        Debug.Log("=== PlayerSpawner Start ===");
        
        // Check if there's already a player
        PlayerMovement existingPlayer = FindFirstObjectByType<PlayerMovement>();
        
        if (existingPlayer != null)
        {
            Debug.Log("Found existing player: " + existingPlayer.name);
            existingPlayer.transform.position = spawnPosition;
        }
        else if (spawnIfNoPlayer && playerPrefab != null)
        {
            Debug.Log("No player found, spawning new player from prefab");
            GameObject newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Spawned new player: " + newPlayer.name);
        }
        else
        {
            Debug.LogWarning("No player found and no prefab assigned or spawnIfNoPlayer is false");
        }
    }
} 