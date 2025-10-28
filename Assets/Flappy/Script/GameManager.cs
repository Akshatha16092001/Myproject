using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button jumpButton;
    [SerializeField] private GameObject gameOverText;

    [Header("Game Objects")]
    [SerializeField] private Transform bird;
    [SerializeField] private GameObject[] pipePairs;

    [Header("Game Settings")]
    public bool isGameActive = true;
    public float jumpForce = 5f;
    public float gravity = -9.8f;
    public float pipeMoveSpeed = 2f;        // pipes move left
    public float pipeResetOffset = 10f;     // how far apart pipes are
    public float pipeGapRangeY = 2f;        // random Y range

    private float verticalVelocity = 0f;
    private Vector3 birdStartPos;

    void Start()
    {
        if (jumpButton != null)
            jumpButton.onClick.AddListener(OnJump);
        if (gameOverText != null)
            gameOverText.SetActive(false);

        if (bird == null)
            Debug.LogError("❌ Bird not assigned!");

        birdStartPos = bird.position;

        // Spacebar also jumps
        jumpButton?.onClick.AddListener(OnJump);
    }

    void Update()
    {
        if (!isGameActive || bird == null) return;

        // === Bird movement (only vertical) ===
        verticalVelocity += gravity * Time.deltaTime;
        bird.position += new Vector3(0, verticalVelocity * Time.deltaTime, 0);

        // === Pipes movement ===
        foreach (GameObject pipePair in pipePairs)
        {
            if (pipePair == null) continue;

            pipePair.transform.position += Vector3.left * pipeMoveSpeed * Time.deltaTime;

            // If pipe goes off screen, recycle it
            if (pipePair.transform.position.x < -10f)
            {
                float newX = GetFurthestPipeX() + pipeResetOffset;
                float newY = Random.Range(-pipeGapRangeY, pipeGapRangeY);
                pipePair.transform.position = new Vector3(newX, newY, 0);
            }

            // Simple collision detection
            float distance = Vector2.Distance(bird.position, pipePair.transform.position);
            if (distance < 0.6f)
            {
                GameOver();
            }
        }

        // === Ground check ===
        if (bird.position.y < -4.5f || bird.position.y > 5f)
            GameOver();
    }

    void OnJump()
    {
        if (!isGameActive) return;
        verticalVelocity = jumpForce;
    }

    void GameOver()
    {
        if (!isGameActive) return;

        isGameActive = false;
        verticalVelocity = 0f;

        if (gameOverText != null)
            gameOverText.SetActive(true);

        Debug.Log("💥 Game Over!");
    }

    float GetFurthestPipeX()
    {
        float maxX = -Mathf.Infinity;
        foreach (GameObject pipePair in pipePairs)
        {
            if (pipePair.transform.position.x > maxX)
                maxX = pipePair.transform.position.x;
        }
        return maxX;
    }
}
