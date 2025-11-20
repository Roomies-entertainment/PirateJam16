using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : Health
{
    private void LateUpdate()
    {
        ProcessHealthEvents();

        if (deathFlag) Destroy(gameObject);

        ClearUpdate();
    }
}
