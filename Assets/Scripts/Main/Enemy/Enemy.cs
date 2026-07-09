using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 30;
    public int currentHP;

    [Header("UI")]
    public Slider hpBar;

    [Header("見た目")]
    public Renderer model;
    private Animator anim;

    private Color originalColor;

    void Awake()
    {
        anim = GetComponent<Animator>();

        currentHP = maxHP;

        if (hpBar != null)
        {
            hpBar.maxValue = maxHP;
            hpBar.value = currentHP;
        }

        originalColor = model.material.color;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        // カメラを揺らす
        CameraShake.Instance?.Shake(0.12f, 0.4f);

        // HPバー更新
        if (hpBar != null)
            hpBar.value = currentHP;

        // 赤色点滅
        StartCoroutine(HitEffectRed());

        // ダメージアニメ
        anim.SetTrigger("Hit");

        // 反撃アニメ
        anim.SetTrigger("Attack1");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    IEnumerator HitEffectRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = originalColor;
    }

    void Die()
    {
        anim.SetTrigger("Die");
    }
}