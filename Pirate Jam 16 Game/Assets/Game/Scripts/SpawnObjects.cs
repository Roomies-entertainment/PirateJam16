using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{

    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private bool loop = true;

    [Header("")]
    [SerializeField] private float minDelay = 0f;
    [SerializeField] private float maxDelay = 0f;
    private float spawnInterval;

    [Header("")]
    [SerializeField] private int amount = 1;
    [SerializeField][Tooltip("Used when applying random offset to multiple objects spawned at same point")] private float spawnPointRadius = 1f;

    [Header("")]
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField][Tooltip("Just uses this game object if none are assigned")] private GameObject[] spawnPoints;
    private Transform spawnParent;

    private void OnValidate()
    {
        if (amount < 1)
        {
            amount = 1;
        }
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
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        if (!enabled || !gameObject.activeInHierarchy || objectsToSpawn.Length == 0)
        {
            return;
        }

        spawnInterval = Mathf.Max(RandomM.Range(minDelay, maxDelay));

        if (loop && spawnInterval == 0f)
        {
            spawnInterval = 0.1f;
        }

        //Debug.Log($"spawn time: {spawnInterval}");

        Invoke(nameof(SpawnInvoked), spawnInterval);
    }

    private void SpawnInvoked()
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

        if (loop)
        {
            Spawn();
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
