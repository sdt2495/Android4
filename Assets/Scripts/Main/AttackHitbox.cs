using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("Hitbox")]
    [SerializeField] private Collider hitbox;
    [SerializeField] private int damage = 10;

    [Header("Hit Effect")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float effectDestroyTime = 2f;

    [Header("Hit Sound")]
    [SerializeField] private AudioClip hitSE;
    [SerializeField][Range(0f, 1f)] private float hitSEVolume = 1f;

    private bool hitSomething = false;

    private void Awake()
    {
        if (hitbox == null)
        {
            hitbox = GetComponent<Collider>();
        }
    }

    private void Start()
    {
        if (hitbox != null)
        {
            hitbox.enabled = false;
        }
        else
        {
            Debug.LogError("Collider が設定されていません！");
        }
    }

    public void EnableHitbox()
    {
        if (hitbox == null) return;

        hitSomething = false;
        hitbox.enabled = true;

        Debug.Log("Hitbox ON");
    }

    public void DisableHitbox()
    {
        if (hitbox == null) return;

        hitbox.enabled = false;

        if (!hitSomething)
        {
            Debug.Log("当たらなかった");
        }

        Debug.Log("Hitbox OFF");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hitbox.enabled) return;

        Debug.Log("Hitbox に当たった : " + other.name);
        hitSomething = true;

        // ==========================
        // ヒットエフェクト
        // ==========================
        if (hitEffectPrefab != null)
        {
            Vector3 hitPos = other.ClosestPoint(transform.position);

            GameObject effect = Instantiate(
                hitEffectPrefab,
                hitPos,
                Quaternion.identity
            );

            Destroy(effect, effectDestroyTime);
        }

        // ==========================
        // ヒット効果音
        // ==========================
        if (hitSE != null)
        {
            AudioSource.PlayClipAtPoint(
                hitSE,
                transform.position,
                hitSEVolume
            );
        }

        // ==========================
        // Enemy
        // ==========================
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                // カメラを揺らす
                CameraShake.Instance?.Shake(0.08f, 0.08f);

                Debug.Log("Enemy に " + damage + " ダメージ");
            }
            else
            {
                Debug.LogWarning("Enemy スクリプトが見つかりません");
            }
        }

        // ==========================
        // Player
        // ==========================
        if (other.CompareTag("Player"))
        {
            PlayerHP hp = other.GetComponent<PlayerHP>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }
}