using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator Animator;

    public void DoUpdate(float movementInput)
    {
        var scale = transform.localScale;

        if (movementInput != 0)
            scale.x = movementInput > 0 ? 1f : -1f;

        transform.localScale = scale;
    }

    public void OnAttack()
    {
        Animator.SetBool("Attack", true);
    }

    public void OnStopAttack()
    {
        Animator.SetBool("Attack", false);
    }
}
