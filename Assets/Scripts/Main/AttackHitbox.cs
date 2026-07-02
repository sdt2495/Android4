using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private Collider hitbox;
    [SerializeField] private int damage = 10;

    private bool hitSomething = false;

    private void Awake()
    {
        // Inspectorで設定されていなければ自動取得
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

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Enemy に " + damage + " ダメージ");
            }
            else
            {
                Debug.LogWarning("Enemy スクリプトが見つかりません");
            }
        }

        if (other.CompareTag("Player"))
        {
            PlayerHP hp = other.GetComponent<PlayerHP>();
            if (hp != null)
            {
                hp.TakeDamage(10);
            }
        }
    }

}