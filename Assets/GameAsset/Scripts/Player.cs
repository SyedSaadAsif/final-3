using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Add this namespace for scene management

public class Player : MonoBehaviour
{
    [Header("Setting")]
    private float moveSpeed = 10.0f;
    [Header("Control")]
    [SerializeField] private float slideSpeed = 15.0f;
    private float laneSwitchDuration = 0.5f; // Duration for switching lanes
    public ParticleSystem dirtParticle;
    private Vector3 targetPosition;
    private int currentLane = 0; // 0 for left, 1 for right
    private Vector3[] lanePositions;
    private bool isSwitchingLanes = false;
    private float laneSwitchStartTime;

    [SerializeField] private TextMeshPro gateLabel;
    [SerializeField] private TextMeshPro counterLabel;

    private int gateText;
    private int countertext = 1;

    private Vector3 swipeStart;
    private bool isSwiping = false;
    private float swipeThreshold = 50f; // Minimum distance for a swipe to be registered

    private bool isRunnerScene = false; // Add a flag to check if it's the runner scene

    void Start()
    {
        // Check if the current scene is a runner scene
        if (SceneManager.GetActiveScene().name.Contains("Runner"))
        {
            isRunnerScene = true;
        }

        // Define the positions of the lanes
        lanePositions = new Vector3[2];
        lanePositions[0] = new Vector3(-1.5f, 0f, 0f); // Left lane
        lanePositions[1] = new Vector3(1.5f, 0f, 0f);  // Right lane

        // Start in the left lane
        targetPosition = lanePositions[0];
        transform.position = targetPosition;
    }

    void Update()
    {
        if (isRunnerScene)
        {
            MoveForward();
            ManageControl();
            HandleDustParticle();
            if (isSwitchingLanes)
            {
                SmoothLaneSwitch();
            }
        }
    }

    void MoveForward()
    {
        transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
    }

    private void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            Vector3 swipeEnd = Input.mousePosition;
            Vector3 swipeDelta = swipeEnd - swipeStart;

            if (Mathf.Abs(swipeDelta.x) > swipeThreshold)
            {
                if (swipeDelta.x > 0)
                {
                    MoveRight();
                }
                else
                {
                    MoveLeft();
                }
            }

            isSwiping = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
    }

    private void MoveLeft()
    {
        if (currentLane != 0)
        {
            currentLane = 0;
            targetPosition = lanePositions[0];
            StartLaneSwitch();
        }
    }

    private void MoveRight()
    {
        if (currentLane != 1)
        {
            currentLane = 1;
            targetPosition = lanePositions[1];
            StartLaneSwitch();
        }
    }

    private void StartLaneSwitch()
    {
        // If already switching, continue or override with new target
        isSwitchingLanes = true;
        laneSwitchStartTime = Time.time;
    }

    private void SmoothLaneSwitch()
    {
        float elapsedTime = (Time.time - laneSwitchStartTime) / laneSwitchDuration;
        float t = Mathf.Clamp01(elapsedTime);

        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 endPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);

        transform.position = newPosition;

        if (t >= 1.0f)
        {
            isSwitchingLanes = false;
            // Ensure the final position is exactly at the target
            transform.position = endPosition;
        }
    }

    // Here we are doing code for multipliers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Multiplier"))
        {
            counterLabel.text = countertext.ToString();
            countertext += 1;
        }
        else if (other.gameObject.CompareTag("Level End"))
        {
            LoadNextLevel(); // Call the method to load the next level
        }
    }

    private void HandleDustParticle()
    {
        if (moveSpeed > 0 && !dirtParticle.isPlaying)
        {
            dirtParticle.Play();
        }
        else if (moveSpeed <= 0 && dirtParticle.isPlaying)
        {
            dirtParticle.Stop();
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1); // Load the next scene in the build index
    }
}
