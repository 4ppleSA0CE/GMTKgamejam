using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D body;

    [SerializeField] private SpriteRenderer spriteRenderer;
    
    // Add sprite references for idle and running
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite runningSprite;

    private float xPosLastFrame;
    private bool isMoving = false;
    
    // Start is called before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set initial sprite to idle
        if (idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovementControl();
        UpdateSprite();
        FlipCharacterX();
    }

    private void MovementControl()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(xInput, yInput).normalized;
        body.linearVelocity = direction * speed;
        
        // Check if player is moving
        isMoving = direction.magnitude > 0.1f;
    }
    
    private void UpdateSprite()
    {
        if (isMoving && runningSprite != null)
        {
            spriteRenderer.sprite = runningSprite;
        }
        else if (!isMoving && idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    private void FlipCharacterX()
    {
        if (transform.position.x > xPosLastFrame)
        {
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x < xPosLastFrame)
        {
            spriteRenderer.flipX = true;
        }
        xPosLastFrame = transform.position.x; 
    }
}
