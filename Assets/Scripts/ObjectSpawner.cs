using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject groundPrefab;
    public GameObject coinPrefab;
    public GameObject enemyPrefab;
    public GameObject spikePrefab;
    public GameObject movingPlatformPrefab;

    public float groundY = -1f;
    public float tileWidth = 1f;
    public int initialTiles = 20;

    public float spawnAheadDistance = 25f;
    public int minPlatformLength = 5;
    public int maxPlatformLength = 10;
    public int minGapSize = 2;
    public int maxGapSize = 4;

    [Range(0f, 1f)]
    public float platformSpawnChance = 0.7f;

    private Transform player;
    private float lastGroundX;
    private bool inGap = false;
    private int gapTilesLeft = 0;
    private int platformTilesLeft = 0;
    private int objectSpawnCounter = 0;
    private float gapStartX = 0f;
    private bool ready = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        player = playerObj.transform;
        lastGroundX = player.position.x - 5f;

        for (int i = 0; i < initialTiles; i++)
        {
            SpawnGroundTile(lastGroundX);
            lastGroundX += tileWidth;
        }

        platformTilesLeft = Random.Range(minPlatformLength, maxPlatformLength);
        ready = true;
    }

    void Update()
    {
        if (!ready || player == null) return;

        while (lastGroundX < player.position.x + spawnAheadDistance)
        {
            if (inGap)
            {
                lastGroundX += tileWidth;
                gapTilesLeft--;

                if (gapTilesLeft <= 0)
                {
                    inGap = false;
                    platformTilesLeft = Random.Range(minPlatformLength, maxPlatformLength);

                    if (movingPlatformPrefab != null && Random.value < platformSpawnChance)
                    {
                        float gapCenterX = (gapStartX + lastGroundX) / 2f;
                        float gapWidth = lastGroundX - gapStartX;

                        GameObject platform = Instantiate(
                            movingPlatformPrefab,
                            new Vector3(gapCenterX, groundY + 4f, 0),
                            Quaternion.identity
                        );

                        MovingPlatform mp = platform.GetComponent<MovingPlatform>();
                        if (mp != null)
                        {
                            mp.moveDistance = gapWidth / 2f;
                            mp.speed = 2f;
                        }
                    }
                }
            }
            else
            {
                SpawnGroundTile(lastGroundX);
                platformTilesLeft--;
                objectSpawnCounter++;

                if (objectSpawnCounter % 2 == 0)
                    SpawnObject(lastGroundX);

                lastGroundX += tileWidth;

                if (platformTilesLeft <= 0)
                {
                    inGap = true;
                    gapStartX = lastGroundX;
                    gapTilesLeft = Random.Range(minGapSize, maxGapSize);
                }
            }
        }
    }

    void SpawnGroundTile(float x)
    {
        if (groundPrefab == null) return;
        Instantiate(groundPrefab, new Vector3(x, groundY, 0), Quaternion.identity);
    }

    void SpawnObject(float x)
    {
        int roll = Random.Range(0, 3);

        if (roll == 0 && coinPrefab != null)
        {
            for (int i = 0; i < 3; i++)
                Instantiate(coinPrefab, new Vector3(x + (i * 0.8f), groundY + 1.5f, 0), Quaternion.identity);
        }
        else if (roll == 1 && enemyPrefab != null)
        {
            Instantiate(enemyPrefab, new Vector3(x, groundY + 1f, 0), Quaternion.identity);
        }
        else if (roll == 2 && spikePrefab != null)
        {
            Instantiate(spikePrefab, new Vector3(x, groundY + 1f, 0), Quaternion.identity);
        }
    }
}
