using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] platformPrefabs; 
    public Transform[] spawnPoints;      

    public int maxPlatforms = 6;         

    private int currentPlatformCount;

    void Start()
    {
        for (int i = 0; i < maxPlatforms; i++)
        {
            SpawnRandomPlatform();
        }
    }

    void Update()
    {
       
        currentPlatformCount = FindObjectsByType<CrumblingPlatform>(FindObjectsSortMode.None).Length;

        if (currentPlatformCount < maxPlatforms)
        {
            SpawnRandomPlatform();
        }
    }

    void SpawnRandomPlatform()
    {
        if (spawnPoints.Length == 0 || platformPrefabs.Length == 0) return;

        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject randomPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
        Instantiate(randomPrefab, randomPoint.position, Quaternion.identity);
    }
}