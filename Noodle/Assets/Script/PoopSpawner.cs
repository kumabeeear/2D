using System.Collections;
using UnityEngine;

public class PoopSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Prefabs")]
    public GameObject warningPrefab;
    public PoopObstacle poopPrefab;

    [Header("Spawn Area")]
    public float minX = -6.5f;
    public float maxX = 6.5f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Timing")]
    public float interval = 3f;
    public float warningTime = 1f;

    private Coroutine spawnCoroutine;

    public void StartSpawning()
    {
        StopSpawning();
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            yield return StartCoroutine(SpawnPoop());
        }
    }

    IEnumerator SpawnPoop()
    {
        float targetX = Mathf.Clamp(player.position.x, minX, maxX);
        float targetY = Mathf.Clamp(player.position.y, minY, maxY);

        Vector3 targetPos = new Vector3(targetX, targetY, 0f);

        GameObject warning = Instantiate(warningPrefab, targetPos, Quaternion.identity);

        yield return new WaitForSeconds(warningTime);

        Destroy(warning);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPoopDropSound();

        Instantiate(poopPrefab, targetPos, Quaternion.identity);
    }
}