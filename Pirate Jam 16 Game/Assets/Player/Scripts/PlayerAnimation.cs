using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator Animator;

    public void OnAttack()
    {
        Animator.SetBool("Attack", true);
    }
}
