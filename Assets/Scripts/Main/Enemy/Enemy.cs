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

    [Header("Result")]
    public GameObject resultPanel;

    [Header("見た目")]
    public Renderer model;

    private Animator anim;
    private Color originalColor;
    private bool isDead = false;

    void Awake()
    {
        anim = GetComponent<Animator>();

        currentHP = maxHP;

        if (hpBar != null)
        {
            hpBar.maxValue = maxHP;
            hpBar.value = currentHP;
        }

        if (model != null)
            originalColor = model.material.color;

        // リザルトパネルを非表示
        if (resultPanel != null)
            resultPanel.SetActive(false);
    }

    public void TakeDamage(int dmg)
    {
        // 死亡後は何もしない
        if (isDead)
            return;

        currentHP -= dmg;

        // カメラを揺らす
        CameraShake.Instance?.Shake(0.12f, 0.4f);

        // HPバー更新
        if (hpBar != null)
            hpBar.value = Mathf.Max(currentHP, 0);

        // 赤色点滅
        if (model != null)
            StartCoroutine(HitEffectRed());

        // ダメージアニメ
        if (anim != null)
        {
            anim.SetTrigger("Hit");
            anim.SetTrigger("Attack1");
        }

        if (currentHP <= 0)
        {
            isDead = true;
            StartCoroutine(DieRoutine());
        }
    }

    IEnumerator HitEffectRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = originalColor;
    }

    IEnumerator DieRoutine()
    {
        // 死亡アニメ
        if (anim != null)
            anim.SetTrigger("Die");

        // 死亡アニメ待ち
        yield return new WaitForSeconds(2f);

        // リザルトパネル表示
        if (resultPanel != null)
            resultPanel.SetActive(true);

        // ゲーム停止
        Time.timeScale = 0f;
    }
}