using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    private Animator anim;

    public float dodgeDistance = 2f;   // 左右に動く距離
    public float dodgeSpeed = 10f;     // 回避速度
    public float returnSpeed = 8f;     // 中央に戻る速度
    public float dodgeTime = 0.2f;     // 回避時間

    private bool isDodging = false;
    private bool isReturning = false;

    private Vector3 centerPos;         // デフォルト位置
    private Vector3 dodgeTarget;       // 回避先

    void Awake()
    {
        anim = GetComponent<Animator>();
        centerPos = transform.position; // 最初の位置を中央として保存
    }

    public void DodgeLeft()
    {
        if (isDodging || isReturning) return;

        anim.SetTrigger("DodgeLeft");
        StartDodge(-transform.right);
    }

    public void DodgeRight()
    {
        if (isDodging || isReturning) return;

        anim.SetTrigger("DodgeRight");
        StartDodge(transform.right);
    }

    void StartDodge(Vector3 direction)
    {
        isDodging = true;
        dodgeTarget = centerPos + direction * dodgeDistance;
        Invoke(nameof(StartReturn), dodgeTime); // 一定時間後に戻る
    }

    void StartReturn()
    {
        isDodging = false;
        isReturning = true;
    }

    void Update()
    {
        if (isDodging)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                dodgeTarget,
                dodgeSpeed * Time.deltaTime
            );
        }
        else if (isReturning)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                centerPos,
                returnSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, centerPos) < 0.05f)
            {
                isReturning = false;
            }
        }
    }
}
