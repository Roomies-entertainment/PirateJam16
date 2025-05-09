using UnityEngine;
using UnityEngine.Events;

public class ChanceEvent : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float chance = 0.5f;
    [SerializeField] private UnityEvent Event;

    public void TryCallEvent()
    {
        if (RandomM.Float0To1() <= chance)
            Event?.Invoke();
    }
}
