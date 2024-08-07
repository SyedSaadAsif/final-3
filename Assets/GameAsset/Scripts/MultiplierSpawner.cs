using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierSpawner : MonoBehaviour
{
    [SerializeField] private int MuliplierCount = 40;
    [SerializeField] private GameObject Multiplier;
    [SerializeField] private Vector2 minMaxPos;

    private float zvalue = 0f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < MuliplierCount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(minMaxPos.x, minMaxPos.y), 0, zvalue + 20f);
            Instantiate(Multiplier, spawnPos, Quaternion.identity);
            zvalue += 20f;
        }
    }
}
