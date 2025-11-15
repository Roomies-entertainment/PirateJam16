using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator Animator;

    private void Start() { } // Ensures component toggle in inspector

    public void FaceDirection(float movementInput)
    {
        var scale = transform.localScale;

        if (movementInput != 0)
            scale.x = movementInput > 0 ? 1f : -1f;

        transform.localScale = scale;
    }

    public void PlayAttackAnimation()
    {
        Animator.SetBool("Attack", true);
    }

    public void StopAttackAnimation()
    {
        Animator.SetBool("Attack", false);
    }
}
