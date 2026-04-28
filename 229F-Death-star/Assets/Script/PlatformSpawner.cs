using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] platformPrefabs;
    public Transform[] spawnPoints;

    public int maxPlatforms = 6;

    private int currentPlatformCount;

    public GameObject[] enemyPrefabs;
    public Transform[] enemySpawnPoints;
    public int minEnemies = 3;
    public int maxEnemies = 5;

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
        List<Transform> availablePoints = new List<Transform>();
        foreach (Transform pt in spawnPoints)
        {
            if (pt.childCount == 0)
            {
                availablePoints.Add(pt);
            }
        }

        if (availablePoints.Count == 0) return;

   
        Transform randomPoint = availablePoints[Random.Range(0, availablePoints.Count)];
        GameObject randomPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

        GameObject newPlatform = Instantiate(randomPrefab, randomPoint.position, Quaternion.identity);
        newPlatform.transform.SetParent(randomPoint);

        StartCoroutine(SpawnEnemyWithDelay(1.5f));
    }

    IEnumerator SpawnEnemyWithDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (enemyPrefabs.Length > 0 && enemySpawnPoints.Length > 0)
        {
            int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            int targetEnemyCount = Random.Range(minEnemies, maxEnemies + 1);

            if (currentEnemies < targetEnemyCount)
            {
                CrumblingPlatform[] activePlatforms = FindObjectsByType<CrumblingPlatform>(FindObjectsSortMode.None);

                List<Transform> availableEnemyPoints = new List<Transform>();

                foreach (Transform ept in enemySpawnPoints)
                {
                    if (ept.childCount == 0) 
                    {
                        bool hasPlatform = false;

                        foreach (CrumblingPlatform plat in activePlatforms)
                        {
                            
                            float distanceX = Mathf.Abs(ept.position.x - plat.transform.position.x);

                            float distanceY = Mathf.Abs(ept.position.y - plat.transform.position.y);

                            if (distanceX <= 0.5f && distanceY <= 2.0f)
                            {
                                hasPlatform = true;
                                break;
                            }
                        }

                        if (hasPlatform)
                        {
                            availableEnemyPoints.Add(ept);
                        }
                    }
                }

                if (availableEnemyPoints.Count > 0)
                {
                    Transform randomEnemyPoint = availableEnemyPoints[Random.Range(0, availableEnemyPoints.Count)];
                    GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                    GameObject newEnemy = Instantiate(randomEnemy, randomEnemyPoint.position, Quaternion.identity);

                    newEnemy.transform.SetParent(randomEnemyPoint);
                }
            }
        }
    }
}