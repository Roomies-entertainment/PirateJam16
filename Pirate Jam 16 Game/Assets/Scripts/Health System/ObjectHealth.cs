using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : Health
{
    private void LateUpdate()
    {
        ProcessHealthEvents();
        ClearUpdate();

        if (dead) gameObject.SetActive(false);
    }
}
