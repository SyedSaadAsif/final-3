using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateManager : MonoBehaviour
{
    public TextMeshPro gateNo;
    public int randomNumber;
    public bool multiply;
    public TextMeshProUGUI uGUI; // Assume this is the player's text label
    private int playerNumber; // Current number on the player

    [Header("Manual Number Settings")]
    public bool useManualNumber;
    public int manualNumber;

    // Start is called before the first frame update
    void Start()
    {
        if (useManualNumber)
        {
            randomNumber = manualNumber;
        }
        else
        {
            if (multiply)
            {
                randomNumber = Random.Range(1, 3);
                gateNo.text = "X" + randomNumber;
            }
            else
            {
                randomNumber = Random.Range(10, 100);

                if (randomNumber % 2 != 0)
                {
                    randomNumber += 1;
                }

                gateNo.text = randomNumber.ToString();
            }
        }

        if (!multiply)
        {
            gateNo.text = randomNumber.ToString();
        }
    }

    // This method will handle the collision with the player
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Retrieve the current number on the player
            if (int.TryParse(uGUI.text, out playerNumber))
            {
                    // Add the gate's random number to the player's number
                    arr.balls += randomNumber;

                // Update the player's text label with the new number
                uGUI.text = arr.balls.ToString();
				//arr.balls = playerNumber;
            }
        }
    }
}
