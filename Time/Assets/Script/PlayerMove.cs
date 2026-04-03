using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移动速度")]
    public float moveSpeed = 5f;   // 可以在 Inspector 里直接改

    private Rigidbody2D rb;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 获取左右输入：A/D 或 左右方向键
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        // 只控制 X 方向速度，Y 保持原本速度（比如跳跃/下落时不会被覆盖）
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}