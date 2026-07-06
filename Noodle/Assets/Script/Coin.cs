using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(scoreValue);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayCoinCollectSound();

            Destroy(gameObject);
        }
    }
}