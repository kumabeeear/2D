using System.Collections;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject warningPrefab;
    public BirdObstacle birdPrefab;

    [Header("Spawn Settings")]
    public float leftX = -9f;
    public float rightX = 9f;
    public float minY = -3.2f;
    public float maxY = 3.2f;

    [Header("Timing")]
    public float warningTime = 1f;
    public int flashCount = 3;
    public float interval = 3f;

    private bool fromLeft = true;
    private Coroutine spawnCoroutine;
    private GameObject currentWarning;

    public void StartSpawning()
    {
        StopSpawning();

        fromLeft = true;
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        if (currentWarning != null)
        {
            Destroy(currentWarning);
            currentWarning = null;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            yield return StartCoroutine(DoBirdAttack());

            fromLeft = !fromLeft;
        }
    }

    IEnumerator DoBirdAttack()
    {
        float targetY = Mathf.Clamp(player.position.y, minY, maxY);

        float warningX = fromLeft ? leftX : rightX;
        Vector3 warningPos = new Vector3(warningX, targetY, 0f);

        currentWarning = Instantiate(warningPrefab, warningPos, Quaternion.identity);
        SpriteRenderer sr = currentWarning.GetComponent<SpriteRenderer>();

        for (int i = 0; i < flashCount; i++)
        {
            if (sr == null) yield break;

            sr.enabled = true;
            yield return new WaitForSeconds(warningTime / (flashCount * 2));

            sr.enabled = false;
            yield return new WaitForSeconds(warningTime / (flashCount * 2));
        }

        if (currentWarning != null)
        {
            Destroy(currentWarning);
            currentWarning = null;
        }

        float spawnX = fromLeft ? leftX : rightX;
        int direction = fromLeft ? 1 : -1;

        BirdObstacle bird = Instantiate(
            birdPrefab,
            new Vector3(spawnX, targetY, 0f),
            Quaternion.identity
        );

        bird.Init(direction);
    }
}