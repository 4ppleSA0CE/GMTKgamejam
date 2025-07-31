using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D body;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private float xPosLastFrame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovementControl();

        FlipCharacterX();
    }

    private void MovementControl()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(xInput, yInput).normalized;
        body.linearVelocity = direction * speed;
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
