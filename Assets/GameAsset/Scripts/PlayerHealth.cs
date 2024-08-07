using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static int currentHealth; // Set an initial health value
    public TextMeshProUGUI healthText; // Reference to the player's health UI text
	public bool hascollided = false;
	public float collisionCooldown = 1.0f; // Cooldown time in seconds
	

    void Start()
    {
        UpdateHealthUI(); // Initialize the health UI
		currentHealth = 0;
		arr.balls = 0;
		Enemy.enemiesKilled=0;
    }
	void Update()
	{
		UpdateHealthUI();
	}
	

    void OnTriggerEnter(Collider other)
    {
        if (!hascollided && other.CompareTag("obstacle"))
        {
			hascollided=true;
            arr.balls--; // Adjust the damage value as needed
            Destroy(other.gameObject); // Destroy the obstacles
			StartCoroutine(ResetCollisionFlag());
        }
        else if (other.CompareTag("Level End"))
        {
            LoadNextLevel(); // Call the method to load the next level
            Debug.Log("Level change");
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = arr.balls.ToString(); // Update the UI text with the current health
        }
    }
	IEnumerator ResetCollisionFlag()
    {
        yield return new WaitForSeconds(collisionCooldown);
        hascollided = false;
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1); // Load the next scene in the build index
    }
}
