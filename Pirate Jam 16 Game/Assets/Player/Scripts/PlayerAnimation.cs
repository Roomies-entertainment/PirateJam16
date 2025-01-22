using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator Animator;

    public bool attacking { get; private set; }

    public void DoUpdate(float movementInput)
    {
        var scale = transform.localScale;
        scale.x = movementInput > 0 ? 1f : -1f;

        transform.localScale = scale;
    }

    public void OnAttack()
    {
        attacking = true;
        Animator.SetBool("Attack", true);
    }

    public void OnStopAttack()
    {
        attacking = false;
        Animator.SetBool("Attack", false);
    }
}
