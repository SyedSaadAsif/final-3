using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class RopeScript : MonoBehaviour
{
    [Header("Rope Settings")]
    [SerializeField] private Transform Transpoint1;
    [SerializeField] private Transform Transpoint2;

    [Header("Prefab")]
    [SerializeField] private Transform BallPrefab;
	public int balls = 0;
    private Transform _newBall;
    private Camera mainCamLocal;
    private LineRenderer _lineRenderer;
    private Vector3 initialMousePosition;
    public TextMeshProUGUI healthText;
	public TextMeshProUGUI enemyNumber;

    //private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        mainCamLocal = Camera.main;
		//arr.balls += PlayerHealth.currentHealth;
       // playerHealth = FindObjectOfType<PlayerHealth>();
        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
		UpdateHealth();
		if(arr.balls<=-1)
		{
			if (true)
			{
				// Handle player death
				Debug.Log("Player Dead");
				SceneManager.LoadScene("MainMenu"); // Load the main menu scene
			}
		}
        if (Input.GetMouseButtonDown(0) && _newBall == null)
        {
            _newBall = Instantiate(BallPrefab, Transpoint2.position, Quaternion.identity);
            _newBall.GetComponent<Rigidbody>().isKinematic = true;
            initialMousePosition = Input.mousePosition;
			arr.balls--;
            //playerHealth.ReduceHealth(1);
            UpdateHealthUI();
        }

        if (Input.GetMouseButton(0) && _newBall != null)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamLocal.WorldToScreenPoint(Transpoint2.position).z);
            Vector3 worldPos = mainCamLocal.ScreenToWorldPoint(mousePos);

            // Keeping the ball's position in the same plane as Transpoint2
            _newBall.position = new Vector3(worldPos.x, Transpoint2.position.y - 0.2f, worldPos.z);

            _lineRenderer.positionCount = 3;
            _lineRenderer.SetPosition(1, _newBall.position);
            _lineRenderer.SetPosition(2, _newBall.position);
        }
        else if (Input.GetMouseButtonUp(0) && _newBall != null)
        {
            // Calculate the direction from the initial to the final mouse position
            Vector3 finalMousePosition = Input.mousePosition;
            Vector3 mouseDirection = (finalMousePosition - initialMousePosition).normalized;

            // Calculate the force direction based on the x and z components
            Vector3 forceDirection = new Vector3(mouseDirection.x, 0, mouseDirection.y).normalized;

            // Negate the direction to ensure the ball moves towards the drag direction
            forceDirection = -forceDirection;

            // Add a slight upward component
            forceDirection += Vector3.up * 0.1f; // Adjust the upward component as needed

            Rigidbody rb = _newBall.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            // Apply force with an increased multiplier for more sensitivity
            rb.AddForce(forceDirection * 2000); // Adjust the force multiplier as needed

            // Destroy the ball after 5 seconds
            Destroy(_newBall.gameObject, 5f);

            _lineRenderer.SetPosition(1, _newBall.position);
            _lineRenderer.SetPosition(2, _newBall.position);

            _newBall = null;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, Transpoint1.position);
            _lineRenderer.SetPosition(1, Transpoint2.position);
        }

        if (Transpoint1 && Transpoint2)
        {
            _lineRenderer.SetPosition(0, Transpoint1.position);
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, Transpoint2.position);
        }
    }
	void UpdateHealth()
    {
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		if(currentSceneIndex == 3)
		{
		 enemyNumber.text = Enemy.enemiesKilled.ToString()+" / 8"; // Update the UI text with the current health
		}
		if(currentSceneIndex == 6)
		{
		 enemyNumber.text = Enemy.enemiesKilled.ToString()+" / 10"; // Update the UI text with the current health
		}
		if(currentSceneIndex == 9)
		{
		 enemyNumber.text = Enemy.enemiesKilled.ToString()+" / 12"; // Update the UI text with the current health
		}
		if(currentSceneIndex == 12)
		{
		 enemyNumber.text = Enemy.enemiesKilled.ToString()+" / 15"; // Update the UI text with the current health
		}
		if(currentSceneIndex == 15)
		{
		 enemyNumber.text = Enemy.enemiesKilled.ToString()+" / 18"; // Update the UI text with the current health
		}
    }
    void UpdateHealthUI()
    {
        healthText.text = arr.balls.ToString(); // Update the UI text with the current health
    }
}

public static class arr
{
	public static int balls;
}