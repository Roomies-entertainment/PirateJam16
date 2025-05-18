using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interact : MonoBehaviour
{
    public void PerformInteraction(Vector2 point, float radius)
    {
         Detection.DetectComponentInParent<Interactable>(point, radius, out var interactables);

        foreach(var i in interactables)
        {
            i.Interact();
        }
    }
}
