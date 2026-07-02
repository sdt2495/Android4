using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 30;
    public Renderer model;
    public Animator animator;

    private Color originalColor;

    void Start()
    {
        originalColor = model.material.color;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        // Ô‚­Œõ‚é
        StartCoroutine(HitEffect());

        // ”íƒ_ƒƒAƒjƒ
        animator.SetTrigger("Hit");

        // š Player ‚ÉUŒ‚‚³‚ê‚½uŠÔ‚É”½Œ‚
        animator.SetTrigger("Attack1");

        if (hp <= 0)
        {
            Die();
        }
    }

    IEnumerator HitEffect()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = originalColor;
    }

    void Die()
    {
        animator.SetTrigger("Die");
    }
}
