using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{

    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private bool loop = true;

    [Header("")]
    [SerializeField] private float minDelay = 1.0f;
    [SerializeField] private float maxDelay = 1.0f;
    private float spawnInterval;

    [Header("")]
    [SerializeField] private int amount = 1;
    [SerializeField][Tooltip("Used when applying random offset to multiple objects spawned at same point")] private float spawnPointRadius = 0.0f;

    [Header("")]
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField][Tooltip("Just uses this game object if none are assigned")] private GameObject[] spawnPoints;
    private Transform spawnParent;

    private void OnValidate()
    {
        amount = Mathf.Max(1, amount);

        minDelay = Mathf.Max(0.0f, minDelay);
        maxDelay = Mathf.Max(0.0f, maxDelay);
    }

    private void Awake()
    {
        if (spawnPoints.Length == 0)
        {
            spawnPoints = new GameObject[] { gameObject };
        }

        spawnParent = new GameObject($"{gameObject.name} - Spawned Objects").transform;
    }

    private void Start()
    {
        if (spawnOnStart)
            Spawn();

        InvokeSpawn();
    }

    private void InvokeSpawn()
    {
        spawnInterval = maxDelay > 0.0 ? Mathf.Max(RandomM.Range(minDelay, maxDelay)) : 0.0f;
        //Debug.Log($"spawn inverval: {spawnInterval}");

        Invoke(nameof(Spawn), spawnInterval);

        if (loop)
            Invoke(nameof(InvokeSpawn), Mathf.Max(0.1f, spawnInterval));
    }

    public void Spawn()
    {
        int[] spawnPointCounts = new int[spawnPoints.Length];

        for (int i = 0; i < amount; i++)
        {
            int spawnPointI = RandomM.Range(0, spawnPoints.Length);

            GameObject instance = Instantiate(
                objectsToSpawn[RandomM.Range(0, objectsToSpawn.Length)],
                spawnPoints[spawnPointI].transform.position,
                Quaternion.identity,
                spawnParent);

            spawnPointCounts[spawnPointI]++;

            if (spawnPointCounts[spawnPointI] > 1)
            {
                instance.transform.Translate(RandomM.RandomDirection() * (RandomM.Float0To1() * spawnPointRadius));
            }
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
