using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [Header("Trash Can Settings")]
    public bool containsGarbagePiece = false;
    public bool hasBeenSearched = false;
    
    private Puzzle2Manager puzzleManager;
    
    void Start()
    {
        // Find the puzzle manager
        puzzleManager = FindFirstObjectByType<Puzzle2Manager>();
        
        // Debug output to show which trash cans have garbage pieces
        if (containsGarbagePiece)
        {
            Debug.Log("TRASH CAN SETUP: " + gameObject.name + " CONTAINS a garbage piece!");
        }
        else
        {
            Debug.Log("TRASH CAN SETUP: " + gameObject.name + " is EMPTY (no garbage piece)");
        }
    }
    
    public void OnInteract()
    {
        Debug.Log("=== TRASH CAN INTERACTION ===");
        Debug.Log("Player interacted with: " + gameObject.name);
        
        if (hasBeenSearched)
        {
            Debug.Log("This trash can has already been searched!");
            return;
        }
        
        // Mark as searched
        hasBeenSearched = true;
        Debug.Log("Trash can marked as searched: " + gameObject.name);
        
        if (containsGarbagePiece)
        {
            // Found a piece!
            Debug.Log("SUCCESS! Found a garbage piece in: " + gameObject.name);
            
            // Notify puzzle manager
            if (puzzleManager != null)
            {
                puzzleManager.PieceFound();
            }
        }
        else
        {
            // Empty trash can
            Debug.Log("This trash can is empty: " + gameObject.name);
        }
        
        // Notify puzzle manager about the search
        if (puzzleManager != null)
        {
            puzzleManager.TrashCanSearched(containsGarbagePiece);
        }
    }
    
    // Method to set up the trash can with or without a piece
    public void SetupTrashCan(bool hasPiece)
    {
        containsGarbagePiece = hasPiece;
        hasBeenSearched = false;
        
        // Debug output when trash can is set up
        if (hasPiece)
        {
            Debug.Log("TRASH CAN SETUP: " + gameObject.name + " assigned a GARBAGE PIECE");
        }
        else
        {
            Debug.Log("TRASH CAN SETUP: " + gameObject.name + " assigned as EMPTY");
        }
    }
    
    // Method to reset the trash can for a new puzzle
    public void ResetTrashCan()
    {
        hasBeenSearched = false;
        Debug.Log("Trash can reset: " + gameObject.name);
    }
} 