using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Add this for scene management

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    public AudioClip hitSound;
    private AudioSource playerAudio;
    public ParticleSystem dirtParticle;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 moveDirection = new Vector3(0, 0, 1); // Change to 1 to move forward

    // Add a static counter for enemies killed
    public static int enemiesKilled = 0;
    private PlayerHealth playerHealth; // Reference to PlayerHealth script

    void Start()
    {
		//enemiesKilled=0;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        if (animator != null)
        {
            animator.SetBool("isRunning", true); // Assumes there's a boolean parameter named "isRunning" in your Animator
        }

        if (rb != null)
        {
            // Rotate enemy by 180 degrees
            transform.Rotate(0, 180, 0);

            // Adjust direction to move backwards
            moveDirection = -moveDirection;

            // Set velocity in the new direction
            rb.velocity = moveDirection.normalized * moveSpeed;
        }

        // Find the PlayerHealth script in the scene
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("SlingshotWeapon"))
        {
            if (gameObject != null)
            {
                Debug.Log("Collided with SlingshotWeapon");
            }
            PlayDirtParticleEffect();
            playerAudio.PlayOneShot(hitSound, 1.0f);
            Destroy(gameObject); // Destroy the enemy

            // Increment the kill count and check if it's time to change the level
            enemiesKilled++;
			PlayDirtParticleEffect();
			//UpdateHealthUI();
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (enemiesKilled >= 8 && currentSceneIndex==3)
            {
				arr.balls=0;
                ChangeLevel(4);
            }
			if (enemiesKilled >= 10 && currentSceneIndex==6)
            {
				arr.balls=0;
                ChangeLevel(7);
            }
			if (enemiesKilled >= 12 && currentSceneIndex==9)
            {
				arr.balls=0;
                ChangeLevel(10);
            }
			if (enemiesKilled >= 15 && currentSceneIndex==12)
            {
				arr.balls=0;
                ChangeLevel(13);
            }
			if (enemiesKilled >= 18 && currentSceneIndex==15)
            {
				arr.balls=0;
                ChangeLevel(16);
            }

            // Reduce the player's health by 10
        }
    }

    private void PlayDirtParticleEffect()
    {
        if (dirtParticle != null)
        {
            ParticleSystem instantiatedParticle = Instantiate(dirtParticle, transform.position, Quaternion.identity);

            // Adjust the emission rate
            var emission = instantiatedParticle.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(9); // Adjust this value as needed

            instantiatedParticle.Play();

            // Destroy the particle system after it has finished playing
            Destroy(instantiatedParticle.gameObject, instantiatedParticle.main.duration + instantiatedParticle.main.startLifetime.constantMax);
        }
    }
	
    private void ChangeLevel(int no)
    {
        // Assuming the next level is specified or determined by build index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(no); // Load the next scene in the build index
    }
}
