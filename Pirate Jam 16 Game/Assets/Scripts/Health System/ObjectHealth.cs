using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : Health
{
    private void LateUpdate()
    {
        EntityControllerL.ProcessHealthEvents(this);

        if (dieFlag) gameObject.SetActive(false);  

        EntityControllerL.ClearHealthUpdate(this);
    }
    
    private void OnDisable()
    {
        EntityControllerL.ClearHealthUpdate(this);
    }
}
