using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float attackDistance = 1.5f;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Player ‚ª‹ß‚Ã‚¢‚½‚çUŒ‚
        if (dist <= attackDistance)
        {
            anim.SetTrigger("Attack1");
        }
    }
}
