using UnityEngine;

[RequireComponent(typeof(HorizontalMovement))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(EnemyAnimation))]
[RequireComponent(typeof(EnemyHealth))]

[ExecuteAlways]
public class Enemy : MonoBehaviour
{
    private void Awake()
    {
        DestroyImmediate(this);
    }
}
