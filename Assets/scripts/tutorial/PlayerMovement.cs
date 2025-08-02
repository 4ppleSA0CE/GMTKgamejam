using TMPro;
using UnityEngine;

enum Direction
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

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

    private Direction[] directionBuffer = new Direction[4];
    private int bufferHead = 0;
    private int bufferCount = 0;

    int attempts = 0;

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
        Vector3 worldPos = body.position;
        bool wrapped = false;

        if (viewPos.x < 0f)
        {
            // Exited left, wrap to right
            Vector3 newViewPos = new Vector3(1f, viewPos.y, viewPos.z);
            worldPos = Camera.main.ViewportToWorldPoint(newViewPos);
            AddDirection(Direction.LEFT);
            wrapped = true;
        }
        else if (viewPos.x > 1f)
        {
            // Exited right, wrap to left
            Vector3 newViewPos = new Vector3(0f, viewPos.y, viewPos.z);
            worldPos = Camera.main.ViewportToWorldPoint(newViewPos);
            AddDirection(Direction.RIGHT);
            wrapped = true;
        }

        if (viewPos.y < 0f)
        {
            // Exited bottom, wrap to top
            Vector3 newViewPos = new Vector3(viewPos.x, 1f, viewPos.z);
            worldPos = Camera.main.ViewportToWorldPoint(newViewPos);
            AddDirection(Direction.DOWN);
            wrapped = true;
        }
        else if (viewPos.y > 1f)
        {
            // Exited top, wrap to bottom
            Vector3 newViewPos = new Vector3(viewPos.x, 0f, viewPos.z);
            worldPos = Camera.main.ViewportToWorldPoint(newViewPos);
            AddDirection(Direction.UP);
            wrapped = true;
        }

        if (wrapped)
        {
            body.position = new Vector2(worldPos.x, worldPos.y);
            LogLast4Exits();
        }
    }

    private void AddDirection(Direction dir)
    {
        directionBuffer[bufferHead] = dir;
        bufferHead = (bufferHead + 1) % directionBuffer.Length;
        if (bufferCount < directionBuffer.Length)
        {
            bufferCount++;
        }
    }

    void LogLast4Exits()
    {
        Debug.Log("Last 4 exits:");
        for (int i = 0; i < bufferCount; i++)
        {
            int index = (bufferHead - bufferCount + i + directionBuffer.Length) % directionBuffer.Length;
            Debug.Log(directionBuffer[index]);
        }

        // Check for RIGHT, DOWN, LEFT, LEFT
        if (bufferCount >= 4)
        {
            int i0 = (bufferHead - 4 + directionBuffer.Length) % directionBuffer.Length;
            int i1 = (bufferHead - 3 + directionBuffer.Length) % directionBuffer.Length;
            int i2 = (bufferHead - 2 + directionBuffer.Length) % directionBuffer.Length;
            int i3 = (bufferHead - 1 + directionBuffer.Length) % directionBuffer.Length;

            if (directionBuffer[i0] == Direction.RIGHT &&
                directionBuffer[i1] == Direction.DOWN &&
                directionBuffer[i2] == Direction.LEFT &&
                directionBuffer[i3] == Direction.LEFT)
            {
                Debug.Log("Sequence matched: RIGHT → DOWN → LEFT → LEFT");
            }
            else
            {
                attempts += 1;
                Debug.Log("Sequence WRONG: " + attempts + "/4");

                if (attempts >= 4)
                {
                    Debug.Log("4 ATTEMPTS WRONG, DEAD");
                }
            }
        }
    }
}
