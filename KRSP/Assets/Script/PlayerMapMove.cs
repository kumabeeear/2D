using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 moveInput;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;

        // 默认朝左
        if (moveX > 0)
        {
            sr.flipX = true;   // 向右时翻转
        }
        else if (moveX < 0)
        {
            sr.flipX = false;  // 向左时恢复默认
        }
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)(moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}