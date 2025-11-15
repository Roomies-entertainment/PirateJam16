using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float timer = 7f;

    private void Update()
    {
        if (timer <= 0f)
            Destroy(gameObject);

        timer -= Time.deltaTime;
    }
}
