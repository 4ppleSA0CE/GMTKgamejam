using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Flag for when indian lady is reached
    // Change maze correct sequence to clockwise circle when reached

    public bool indianReached = false;

    private Direction[] directionBuffer = new Direction[4];
    private int bufferHead = 0;
    private int bufferCount = 0;
    private static Direction lastExitDirection = Direction.RIGHT; // Store exit direction from landing zone (static to persist across scenes)
    private bool hasPositionedInLandingZone = false; // Track if we've positioned the player in landing zone

    public string nextSceneName = "Landing zone"; // Change this to your next scene name
    public float fadeDuration = 2f;

    int attempts = 0;
    int deaths = 0;

    // Singleton so only one object is created
    private static PlayerMovement instance;
    
    // Property to check deodorant state from GlobalInventoryManager
    private bool hasDeodorant
    {
        get
        {
            if (GlobalInventoryManager.Instance != null)
            {
                return GlobalInventoryManager.Instance.HasDeodorant();
            }
            return false;
        }
    }

    void Awake()
    {
        Debug.Log("PlayerMovement Awake called for: " + gameObject.name + " in scene: " + gameObject.scene.name);
        
        // Check if this is a scene-specific player (like in Deoderant scene)
        string currentScene = SceneManager.GetActiveScene().name;
        
        if (instance != null && instance != this)
        {
            // If we already have a persistent instance, handle this carefully
            if (currentScene == "Deoderant")
            {
                // In Deoderant scene, if there's already a persistent player, 
                // destroy this scene-specific one and let the persistent one handle it
                Debug.Log("Destroying scene-specific player in Deoderant scene: " + gameObject.name);
                Destroy(gameObject);
                return;
            }
            else
            {
                // For other scenes, destroy this duplicate
                Debug.Log("Destroying duplicate PlayerMovement: " + gameObject.name);
                Destroy(gameObject);
                return;
            }
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Prevent state variables from being reset when changing scenes
        Debug.Log("PlayerMovement instance set and DontDestroyOnLoad applied to: " + gameObject.name);
    }

    // Start is called before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set initial sprite to idle
        if (idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }
        
        // Reset positioning flag for new scene
        hasPositionedInLandingZone = false;
        
        // Handle spawn positioning for different scenes
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "maze")
        {
            PositionPlayerBasedOnExitDirection();
        }
        else if (currentScene == "Landing zone")
        {
            PositionPlayerInLandingZone();
        }
    }

    void LateUpdate()
    {
        // Ensure landing zone positioning is applied after all other scripts
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Landing zone" && !hasPositionedInLandingZone)
        {
            PositionPlayerInLandingZone();
            hasPositionedInLandingZone = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovementControl();
        UpdateSprite();
        FlipCharacterX();

        // Debug.Log("Indian reached: " + indianReached);

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Tutorial")
        {
            TutorialExitDetect();
        }
        else if (currentScene == "maze")
        {
            ExitDetect();
        }
        else if (currentScene == "Landing zone")
        {
            LandingZoneExitDetect();
        }
        else if (currentScene == "TerminalOne")
        {
            indianReached = true;
            TerminalOneExitDetect();
        }
        else if (currentScene == "terminalone landing")
        {
            TerminalOneLandingExitDetect();
        }
        else if (currentScene == "puzzle 2")
        {
            Puzzle2ExitDetect();
        }
        else if (currentScene == "Deoderant")
        {
            DeoderantExitDetect();
        }
        else
        {
            // Debug.Log("Unknown scene: " + currentScene);
        }
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

    private void DeoderantExitDetect()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(body.position);
        bool shouldTransition = false;
        Direction exitDirection = Direction.RIGHT;

        if (viewPos.x < 0f)
        {
            // Exited left
            exitDirection = Direction.LEFT;
            shouldTransition = true;
        }
        else if (viewPos.x > 1f)
        {
            // Exited right
            exitDirection = Direction.RIGHT;
            shouldTransition = true;
        }
        else if (viewPos.y < 0f)
        {
            // Exited bottom
            exitDirection = Direction.DOWN;
            shouldTransition = true;
        }
        else if (viewPos.y > 1f)
        {
            // Exited top
            exitDirection = Direction.UP;
            shouldTransition = true;
        }

        if (shouldTransition)
        {
            // Store the exit direction for spawn positioning
            lastExitDirection = exitDirection;
            Debug.Log($"Deoderant exit detected: {exitDirection}, transitioning to landing zone");

            // Instantly transition to landing zone scene
            SceneManager.LoadScene("Landing zone");
        }
    }

    private void TutorialExitDetect()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(body.position);
        bool shouldTransition = false;
        Direction exitDirection = Direction.RIGHT;

        if (viewPos.x < 0f)
        {
            // Exited left
            exitDirection = Direction.LEFT;
            shouldTransition = true;
        }
        else if (viewPos.x > 1f)
        {
            // Exited right
            exitDirection = Direction.RIGHT;
            shouldTransition = true;
        }
        else if (viewPos.y < 0f)
        {
            // Exited bottom
            exitDirection = Direction.DOWN;
            shouldTransition = true;
        }
        else if (viewPos.y > 1f)
        {
            // Exited top
            exitDirection = Direction.UP;
            shouldTransition = true;
        }

        if (shouldTransition)
        {
            // Store the exit direction for spawn positioning
            lastExitDirection = exitDirection;
            Debug.Log($"Tutorial exit detected: {exitDirection}, transitioning to landing zone");

            // Instantly transition to landing zone scene
            SceneManager.LoadScene("Landing zone");
        }
    }

    private void Puzzle2ExitDetect()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(body.position);
        bool shouldTransition = false;
        Direction exitDirection = Direction.RIGHT;

        if (viewPos.x < 0f)
        {
            // Exited left
            exitDirection = Direction.LEFT;
            shouldTransition = true;
        }
        else if (viewPos.x > 1f)
        {
            // Exited right
            exitDirection = Direction.RIGHT;
            shouldTransition = true;
        }
        else if (viewPos.y < 0f)
        {
            // Exited bottom
            exitDirection = Direction.DOWN;
            shouldTransition = true;
        }
        else if (viewPos.y > 1f)
        {
            // Exited top
            exitDirection = Direction.UP;
            shouldTransition = true;
        }

        if (shouldTransition)
        {
            // Store the exit direction for spawn positioning
            lastExitDirection = exitDirection;
            Debug.Log($"Puzzle 2 exit detected: {exitDirection}, transitioning to landing zone");

            // Instantly transition to maze scene
            SceneManager.LoadScene("Terminalone landing");
        }
    }

    private void LandingZoneExitDetect()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(body.position);
        bool shouldTransition = false;
        Direction exitDirection = Direction.RIGHT;

        if (viewPos.x < 0f)
        {
            // Exited left
            exitDirection = Direction.LEFT;
            shouldTransition = true;
        }
        else if (viewPos.x > 1f)
        {
            // Exited right
            exitDirection = Direction.RIGHT;
            shouldTransition = true;
        }
        else if (viewPos.y < 0f)
        {
            // Exited bottom
            exitDirection = Direction.DOWN;
            shouldTransition = true;
        }
        else if (viewPos.y > 1f)
        {
            // Exited top
            exitDirection = Direction.UP;
            shouldTransition = true;
        }

        if (shouldTransition)
        {
            // Store the exit direction for spawn positioning
            lastExitDirection = exitDirection;
            Debug.Log($"Landing zone exit detected: {exitDirection}, transitioning to maze");

            // Instantly transition to maze scene
            SceneManager.LoadScene("maze");
        }
    }

    private void TerminalOneLandingExitDetect()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(body.position);
        bool shouldTransition = false;
        Direction exitDirection = Direction.RIGHT;

        if (viewPos.x < 0f)
        {
            // Exited left
            exitDirection = Direction.LEFT;
            shouldTransition = true;
        }
        else if (viewPos.x > 1f)
        {
            // Exited right
            exitDirection = Direction.RIGHT;
            shouldTransition = true;
        }
        else if (viewPos.y < 0f)
        {
            // Exited bottom
            exitDirection = Direction.DOWN;
            shouldTransition = true;
        }
        else if (viewPos.y > 1f)
        {
            // Exited top
            exitDirection = Direction.UP;
            shouldTransition = true;
        }

        if (shouldTransition)
        {
            // Store the exit direction for spawn positioning
            lastExitDirection = exitDirection;
            Debug.Log($"TerminalOneLanding exit detected: {exitDirection}, transitioning to maze");

            // Instantly transition to maze scene
            SceneManager.LoadScene("maze");
        }
    }

    private void TerminalOneExitDetect()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(body.position);
        bool shouldTransition = false;
        Direction exitDirection = Direction.RIGHT;

        if (viewPos.x < 0f)
        {
            // Exited left
            exitDirection = Direction.LEFT;
            shouldTransition = true;
        }
        else if (viewPos.x > 1f)
        {
            // Exited right
            exitDirection = Direction.RIGHT;
            shouldTransition = true;
        }
        else if (viewPos.y < 0f)
        {
            // Exited bottom
            exitDirection = Direction.DOWN;
            shouldTransition = true;
        }
        else if (viewPos.y > 1f)
        {
            // Exited top
            exitDirection = Direction.UP;
            shouldTransition = true;
        }

        if (shouldTransition)
        {
            // Store the exit direction for spawn positioning
            lastExitDirection = exitDirection;
            Debug.Log($"TerminalOne exit detected: {exitDirection}, transitioning to maze");

            // Instantly transition to maze scene
            SceneManager.LoadScene("maze");
        }
    }

    private void PositionPlayerBasedOnExitDirection()
    {
        // Get camera bounds
        Camera cam = Camera.main;
        if (cam == null) return;
        
        Vector3 spawnPosition = Vector3.zero;
        
        switch (lastExitDirection)
        {
            case Direction.LEFT:
                // Spawn on right side
                spawnPosition = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0f));
                break;
            case Direction.RIGHT:
                // Spawn on left side
                spawnPosition = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f));
                break;
            case Direction.UP:
                // Spawn on bottom
                spawnPosition = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f));
                break;
            case Direction.DOWN:
                // Spawn on top
                spawnPosition = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f));
                break;
        }
        
        // Set player position
        transform.position = new Vector3(spawnPosition.x, spawnPosition.y, transform.position.z);
        Debug.Log($"Player spawned at position: {spawnPosition} based on exit direction: {lastExitDirection}");
    }
    
    private void PositionPlayerInLandingZone()
    {
        // Always spawn the character at the specified coordinates when entering the landing zone
        Vector3 spawnPosition = new Vector3(1.329138f, 3.818122f, transform.position.z);
        
        // Force the position to be set
        transform.position = spawnPosition;
        body.position = new Vector2(spawnPosition.x, spawnPosition.y);
        
        Debug.Log($"Player positioned in Landing Zone at fixed coordinates: {spawnPosition}");
        Debug.Log($"Transform position: {transform.position}, Body position: {body.position}");
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

        if (CheckCorrectMazeSequence())
        {
            Debug.Log("Sequence matched");

            if (indianReached)
            {
                if (hasDeodorant)
                {
                    SceneManager.LoadScene("TerminalOne");
                }
                else{
                    SceneManager.LoadScene("Deoderant");
                }
                
            }
            else
            {
                SceneManager.LoadScene("TerminalOne");
            }
        }
        else
        {
            attempts++;
            Debug.Log("Sequence WRONG: " + attempts + "/4");

            if (attempts >= 4)
            {
                attempts = 0;
                deaths++;

                if (deaths >= 4)
                {
                    Debug.Log("4 ATTEMPTS WRONG, DEAD");

                    // Go back to landing zone if dead
                    SceneManager.LoadScene("Landing zone");

                    // Reset
                    attempts = 0;
                    deaths = 0;
                }
            }
        }
    }

    private bool CheckCorrectMazeSequence()
    {
        if (bufferCount >= 4)
        {
            int i0 = (bufferHead - 4 + directionBuffer.Length) % directionBuffer.Length;
            int i1 = (bufferHead - 3 + directionBuffer.Length) % directionBuffer.Length;
            int i2 = (bufferHead - 2 + directionBuffer.Length) % directionBuffer.Length;
            int i3 = (bufferHead - 1 + directionBuffer.Length) % directionBuffer.Length;

            if (indianReached)
            {
                if (hasDeodorant)
                {
                    return directionBuffer[i0] == Direction.DOWN &&
                       directionBuffer[i1] == Direction.DOWN &&
                       directionBuffer[i2] == Direction.DOWN &&
                       directionBuffer[i3] == Direction.DOWN;
                }
                return directionBuffer[i0] == Direction.UP &&
                       directionBuffer[i1] == Direction.LEFT &&
                       directionBuffer[i2] == Direction.DOWN &&
                       directionBuffer[i3] == Direction.RIGHT;
            }
            else
            {
                return directionBuffer[i0] == Direction.DOWN &&
                       directionBuffer[i1] == Direction.DOWN &&
                       directionBuffer[i2] == Direction.DOWN &&
                       directionBuffer[i3] == Direction.DOWN;
            }
        }

        return false;
    }
}
