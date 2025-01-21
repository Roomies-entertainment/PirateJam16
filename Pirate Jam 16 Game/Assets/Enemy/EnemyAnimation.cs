using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator Animator;

    public void OnAttack()
    {
        Animator.SetBool("Attack", true);
    }
}
