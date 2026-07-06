using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject coinPrefab;

    [Header("Spawn Area")]
    public float minX = -6.5f;
    public float maxX = 6.5f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Settings")]
    public float interval = 2f;
    public int maxCoins = 5;

    private Coroutine spawnCoroutine;

    public void StartSpawning()
    {
        StopSpawning();

        SpawnCoin();

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
            SpawnCoin();
        }
    }

    void SpawnCoin()
    {
        if (coinPrefab == null)
        {
            Debug.LogWarning("CoinSpawner: coinPrefab is missing.");
            return;
        }

        if (GameObject.FindGameObjectsWithTag("Coin").Length >= maxCoins)
            return;

        Vector3 spawnPos = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0f
        );

        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }
}