using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawning : MonoBehaviour
{
    public GameObject[] cloudToSpawn;

    public GameObject[] spawnPoints;

    public float minDelay, maxDelay;


    private float spawnInterval;

    void Start(){
        Invoke("Spawn", 1f);
    }

    void Spawn(){
        spawnInterval = Random.Range(minDelay, maxDelay);
        Instantiate(cloudToSpawn[Random.Range(0, cloudToSpawn.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].transform);
        //Debug.Log($"spawn time: {spawnInterval}");

        Invoke("Spawn", spawnInterval);
    }
}
