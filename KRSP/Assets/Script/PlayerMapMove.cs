using UnityEngine;

public class PlayerMapMove : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 moveDirection;

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }
}