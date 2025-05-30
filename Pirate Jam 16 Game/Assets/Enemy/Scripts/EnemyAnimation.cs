using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator Animator;

    private void Start() { } // Ensures component toggle in inspector
    
    public void OnPerformAttack()
    {
        Animator.SetBool("Attack", true);
    }

    public void OnStartBlocking()
    {
        Animator.SetBool("Block", true);
    }

    public void OnStopBlocking()
    {
        Animator.SetBool("Block", false);
    }

    public void OnStopAttack()
    {
        Animator.SetBool("Attack", false);
    }
}
