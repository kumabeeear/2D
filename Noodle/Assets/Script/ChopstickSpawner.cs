using System.Collections;
using UnityEngine;

public class ChopstickSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject warningPrefab;
    public ChopstickAttack chopstickPrefab;

    [Header("Spawn Points")]
    public Transform topPoint;
    public Transform bottomPoint;

    [Header("X Range")]
    public float minX = -6.5f;
    public float maxX = 6.5f;

    [Header("Timing")]
    public float warningTime = 1.5f;
    public int flashCount = 3;
    public float firstInterval = 2f;
    public float intervalDecrease = 0.1f;
    public float minInterval = 0.6f;

    private bool nextFromTop = true;
    private float currentInterval;
    private Coroutine attackCoroutine;

    public void StartSpawning()
    {
        StopSpawning();

        nextFromTop = true;
        currentInterval = firstInterval;
        attackCoroutine = StartCoroutine(AttackLoop());
    }

    public void StopSpawning()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentInterval);

            if (nextFromTop)
                yield return StartCoroutine(DoAttack(topPoint, true));
            else
                yield return StartCoroutine(DoAttack(bottomPoint, false));

            nextFromTop = !nextFromTop;
            currentInterval = Mathf.Max(minInterval, currentInterval - intervalDecrease);
        }
    }

    IEnumerator DoAttack(Transform point, bool fromTop)
    {
        float targetX = Mathf.Clamp(player.position.x, minX, maxX);

        Vector3 warningPos = new Vector3(targetX, point.position.y, 0f);
        GameObject warning = Instantiate(warningPrefab, warningPos, Quaternion.identity);

        SpriteRenderer sr = warning.GetComponent<SpriteRenderer>();

        for (int i = 0; i < flashCount; i++)
        {
            sr.enabled = true;
            yield return new WaitForSeconds(warningTime / (flashCount * 2));

            sr.enabled = false;
            yield return new WaitForSeconds(warningTime / (flashCount * 2));
        }

        Destroy(warning);

        ChopstickAttack chopstick = Instantiate(
            chopstickPrefab,
            new Vector3(targetX, point.position.y, 0f),
            Quaternion.identity
        );

        chopstick.Init(fromTop);
    }
}