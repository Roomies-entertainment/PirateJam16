using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator Animator;

    public void OnPerformAttack()
    {
        Animator.SetBool("Attack", true);
    }

    public void OnStopAttack()
    {
        Animator.SetBool("Attack", false);
    }
}
