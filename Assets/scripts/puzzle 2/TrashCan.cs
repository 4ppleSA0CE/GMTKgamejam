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
    }
    
    public void OnInteract()
    {
        if (hasBeenSearched)
        {
            Debug.Log("This trash can has already been searched!");
            return;
        }
        
        // Mark as searched
        hasBeenSearched = true;
        
        if (containsGarbagePiece)
        {
            // Found a piece!
            Debug.Log("Found a garbage piece!");
            
            // Notify puzzle manager
            if (puzzleManager != null)
            {
                puzzleManager.PieceFound();
            }
        }
        else
        {
            // Empty trash can
            Debug.Log("This trash can is empty.");
        }
        
        // Notify puzzle manager about the search
        if (puzzleManager != null)
        {
            puzzleManager.TrashCanSearched();
        }
    }
    
    // Method to set up the trash can with or without a piece
    public void SetupTrashCan(bool hasPiece)
    {
        containsGarbagePiece = hasPiece;
        hasBeenSearched = false;
    }
    
    // Method to reset the trash can for a new puzzle
    public void ResetTrashCan()
    {
        hasBeenSearched = false;
    }
} 