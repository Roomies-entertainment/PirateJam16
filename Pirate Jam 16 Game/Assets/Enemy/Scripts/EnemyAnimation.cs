using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator Animator;

    private void Start() { } // Ensures component toggle in inspector
    
    public void SetAnimatorAttack(bool setTo)
    {
        Animator.SetBool("Attack", setTo);
    }

    public void SetAnimatorBlock(bool setTo)
    {
        Animator.SetBool("Block", setTo);
    }
}
