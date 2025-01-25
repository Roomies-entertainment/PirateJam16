using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawning : MonoBehaviour
{
    public GameObject[] objectsToSpawn;

    public GameObject[] spawnPoints;

    [SerializeField] private float initialDelay = 0f;
    [SerializeField] private float minDelay, maxDelay;


    private float spawnInterval;

    void Start(){
        Invoke("Spawn", initialDelay);
    }

    void Spawn(){
        spawnInterval = Random.Range(minDelay, maxDelay);
        Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].transform);
        //Debug.Log($"spawn time: {spawnInterval}");

        Invoke("Spawn", spawnInterval);
    }
}
