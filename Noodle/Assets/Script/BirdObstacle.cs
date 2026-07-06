using UnityEngine;

public class BirdObstacle : MonoBehaviour
{
    public float moveSpeed = 8f;

    private int direction = 1;

    public void Init(int moveDirection)
    {
        direction = moveDirection;

        // 左 → 右
        if (direction == 1)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        // 右 → 左
        else
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayBirdFlySound();
    }

    void Update()
    {
        transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

        if (transform.position.x > 12f || transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
    }
}