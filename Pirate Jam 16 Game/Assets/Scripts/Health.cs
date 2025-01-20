using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private int healthMax = 3;

    public int health { get; private set; }

    private void Awake()
    {
        health = Mathf.Max(startingHealth, healthMax);
    }

    public void IncrementHealth(int increment)
    {
        health = (int) Mathf.Clamp(health + increment, 0f, healthMax);
    }


}
