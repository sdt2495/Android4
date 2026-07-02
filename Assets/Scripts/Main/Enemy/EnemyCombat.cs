using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private Animator anim;
    public AttackHitbox attackHitbox;

    [Header("攻撃間隔（秒）")]
    public float minAttackTime = 1.5f;
    public float maxAttackTime = 3.0f;

    private float timer;

    void Awake()
    {
        anim = GetComponent<Animator>();
        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Attack();
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = Random.Range(minAttackTime, maxAttackTime);
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        attackHitbox.EnableHitbox();
    }

    // アニメーションイベントで呼ぶ
    public void EndAttack()
    {
        attackHitbox.DisableHitbox();
    }
}
