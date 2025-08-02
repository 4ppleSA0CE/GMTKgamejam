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
    private float lastHorizontalInput = 0f;

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
        ExitDetect();
    }

    private void MovementControl()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(xInput, yInput).normalized;
        body.linearVelocity = direction * speed;

        // Check if player is moving
        isMoving = direction.magnitude > 0.1f;

        // Store horizontal input for sprite flipping
        if (Mathf.Abs(xInput) > 0.1f)
        {
            lastHorizontalInput = xInput;
        }
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
        // Only flip based on actual input direction, not position changes
        if (lastHorizontalInput > 0.1f)
        {
            spriteRenderer.flipX = false; // Face right
        }
        else if (lastHorizontalInput < -0.1f)
        {
            spriteRenderer.flipX = true; // Face left
        }
        // If no horizontal input, keep the last direction
    }

    private void ExitDetect()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(body.position);

        // Log the viewport position
        Debug.Log($"Viewport Pos: {viewPos}");

        // Detect if out of bounds (screen space is 0 to 1 in x and y)
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            Debug.Log("Object is out of the camera view!");
            // Do something, like switch scene or reposition
        }
    }
}
