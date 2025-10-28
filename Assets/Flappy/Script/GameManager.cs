using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button jumpButton;
    [SerializeField] private GameObject gameOverText;

    [Header("Game Objects")]
    [SerializeField] private Transform bird;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject[] pipePairs;

    [Header("Game Settings")]
    public bool isGameActive = true;
    public float jumpForce = 5f;
    public float gravity = -9.8f;
    public float birdForwardSpeed = 2f;     // ✅ new — bird moves forward
    public float pipeResetOffset = 30f;     // distance ahead of camera to respawn pipes
    public float pipeGapRangeY = 2f;        // random Y range

    private float verticalVelocity = 0f;

    void Start()
    {
        if (jumpButton != null)
            jumpButton.onClick.AddListener(OnJump);
        if (gameOverText != null)
            gameOverText.SetActive(false);

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (bird == null)
            Debug.LogError("❌ Bird not assigned!");
    }

    void Update()
    {
        if (!isGameActive || bird == null) return;

        // === Bird movement ===
        verticalVelocity += gravity * Time.deltaTime;
        bird.position += new Vector3(birdForwardSpeed * Time.deltaTime, verticalVelocity * Time.deltaTime, 0);

        // === Camera follows bird ===
        if (mainCamera != null)
        {
            Vector3 camPos = mainCamera.transform.position;
            camPos.x = bird.position.x + 4f; // keep camera slightly ahead
            mainCamera.transform.position = camPos;
        }

        // === Move / recycle pipes ===
        foreach (GameObject pipePair in pipePairs)
        {
            if (pipePair == null) continue;

            // If pipe goes far behind the camera, recycle it
            if (pipePair.transform.position.x < mainCamera.transform.position.x - 10f)
            {
                float newX = mainCamera.transform.position.x + pipeResetOffset;
                float newY = Random.Range(-pipeGapRangeY, pipeGapRangeY);
                pipePair.transform.position = new Vector3(newX, newY, 0);
            }

            // Simple collision detection
            float distance = Vector2.Distance(bird.position, pipePair.transform.position);
            if (distance < 0.7f)
            {
                GameOver();
            }
        }

        // Stop if bird falls below view
        if (bird.position.y < -5f)
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
}
