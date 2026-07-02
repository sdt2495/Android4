using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator anim;
    public AttackHitbox attackHitbox;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnSwipe(Vector2 dir)
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsAttack", true);

        anim.SetTrigger("Attack");
        attackHitbox.EnableHitbox();
    }

    // アニメーションイベントで呼ぶ
    public void EndAttack()
    {
        attackHitbox.DisableHitbox();

        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", true);
    }
}
