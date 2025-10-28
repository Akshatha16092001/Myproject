using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float jumpForce = 5f;     // Upward force when jumping
    public float forwardSpeed = 2f;  // Horizontal speed

    private Rigidbody2D rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;        // Make it fall naturally
        rb.freezeRotation = true;    // Prevent rotation
         //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Update()
    {
        // Jump with space or mouse click
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        // Constant horizontal movement
        rb.velocity = new Vector2(forwardSpeed, rb.velocity.y);
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);       // Reset vertical speed
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
