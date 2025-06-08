using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float delay;
    private float timer;

    private void Update()
    {

        if (timer > delay)
            Destroy(gameObject);

        timer += Time.deltaTime;
    }
}
