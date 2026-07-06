using UnityEngine;

public class WaterTileSpawner : MonoBehaviour
{
    public GameObject waterTilePrefab;

    public float scrollSpeed = 2f;
    public float tileWidth = 16f;

    public int startTileCount = 3;

    public float spawnX = 16f;
    public float destroyX = -16f;

    private float nextSpawnX;

    void Start()
    {
        nextSpawnX = -tileWidth;

        for (int i = 0; i < startTileCount; i++)
        {
            SpawnTile(i * tileWidth);
        }
    }

    void Update()
    {
        MoveTiles();

        GameObject lastTile = GetLastTile();

        if (lastTile != null && lastTile.transform.position.x <= spawnX - tileWidth)
        {
            SpawnTile(lastTile.transform.position.x + tileWidth);
        }
    }

    void SpawnTile(float xPosition)
    {
        GameObject tile = Instantiate(waterTilePrefab, new Vector3(xPosition, 0f, 0f), Quaternion.identity);
        tile.transform.SetParent(transform);
    }

    void MoveTiles()
    {
        foreach (Transform tile in transform)
        {
            tile.position += Vector3.left * scrollSpeed * Time.deltaTime;

            if (tile.position.x < destroyX)
            {
                Destroy(tile.gameObject);
            }
        }
    }

    GameObject GetLastTile()
    {
        GameObject lastTile = null;
        float maxX = float.MinValue;

        foreach (Transform tile in transform)
        {
            if (tile.position.x > maxX)
            {
                maxX = tile.position.x;
                lastTile = tile.gameObject;
            }
        }

        return lastTile;
    }
}