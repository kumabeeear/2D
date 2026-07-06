using System.Collections;
using UnityEngine;

public class ChopstickAttack : MonoBehaviour
{
    public float extendDistance = 3.5f;
    public float extendTime = 0.25f;
    public float holdTime = 0.25f;
    public float retractTime = 0.25f;

    private Vector3 startPos;
    private Vector3 endPos;

    public void Init(bool fromTop)
    {
        startPos = transform.position;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayChopstickSound();

        if (fromTop)
        {
            endPos = startPos + Vector3.down * extendDistance;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            endPos = startPos + Vector3.up * extendDistance;
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

        StartCoroutine(AttackMove());
    }

    IEnumerator AttackMove()
    {
        float t = 0f;

        while (t < extendTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t / extendTime);
            yield return null;
        }

        yield return new WaitForSeconds(holdTime);

        t = 0f;

        while (t < retractTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(endPos, startPos, t / retractTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance.GameOver();
    }
}