using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public Slider hpBar;
    public Renderer model; // Player の見た目
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

        if (hpBar != null)
            hpBar.value = currentHP;

        // 黄色点滅
        StartCoroutine(HitEffectYellow());

        // ダメージアニメ
        anim.SetTrigger("Damage");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    IEnumerator HitEffectYellow()
    {
        model.material.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        model.material.color = originalColor;
    }

    void Die()
    {
        anim.SetBool("Dead", true);
    }
}
