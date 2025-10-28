using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button jumpButton;
    [SerializeField] private GameObject gameOverText;

    [Header("Bird")]
    [SerializeField] private Rigidbody2D birdRb;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    [Header("Pipes")]
    [SerializeField] private GameObject[] pipePairs;
    [SerializeField] private float pipeResetOffset = 25f;
    [SerializeField] private float pipeGapRangeY = 2f;

    [Header("Game Settings")]
    [SerializeField] private float forwardSpeed = 2.5f;
    [SerializeField] private float jumpForce = 5f;

    private bool started = false;
    private bool isGameOver = false;

    void Start()
    {
        if (birdRb == null) Debug.LogError("Assign Bird Rigidbody2D in Inspector");
        if (mainCamera == null) mainCamera = Camera.main;

        if (jumpButton != null)
        {
            jumpButton.onClick.RemoveAllListeners();
            jumpButton.onClick.AddListener(OnJump);
        }

        if (gameOverText != null)
            gameOverText.SetActive(false);

        birdRb.gravityScale = 0f;
        birdRb.velocity = Vector2.zero;
    }

    void Update()
    {
        if (isGameOver) return;

        if (started)
        {
            // Bird moves forward constantly
            birdRb.velocity = new Vector2(forwardSpeed, birdRb.velocity.y);

            // Camera follows bird
            Vector3 camPos = mainCamera.transform.position;
            camPos.x = birdRb.transform.position.x + 4f;
            mainCamera.transform.position = camPos;

            // Recycle pipes only if they go far behind the camera
            foreach (GameObject pipe in pipePairs)
            {
                if (pipe == null) continue;

                if (pipe.transform.position.x < mainCamera.transform.position.x - 10f)
                {
                    float newX = mainCamera.transform.position.x + pipeResetOffset;
                    float newY = Random.Range(-pipeGapRangeY, pipeGapRangeY);
                    pipe.transform.position = new Vector3(newX, newY, pipe.transform.position.z);
                }
            }
        }

        // Optional: Game over if bird falls too far
        if (birdRb.transform.position.y < -6f)
            GameOver();
    }

    public void OnJump()
    {
        if (isGameOver) return;

        if (!started)
        {
            started = true;
            birdRb.gravityScale = 1f;
        }

        birdRb.velocity = new Vector2(forwardSpeed, jumpForce);
    }

    // === COLLISION HANDLING ===
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver) return;

        // Check parent or self name for PipePair
        Transform t = collision.transform;
        if (t.name.StartsWith("PipePair") || (t.parent != null && t.parent.name.StartsWith("PipePair")))
        {
            Debug.Log("💥 Hit pipe: " + t.name);
            GameOver();
        }
    }

    private void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        birdRb.velocity = Vector2.zero;
        birdRb.gravityScale = 0f;

        if (gameOverText != null)
            gameOverText.SetActive(true);

        Debug.Log("💥 Game Over!");
    }
}
