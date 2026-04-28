using System.Collections; // 🌟 ต้องเพิ่มบรรทัดนี้ เพื่อให้ใช้ระบบ Coroutine (หน่วงเวลา) ได้
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

        // 1. หาจุดเกิดแท่นที่ "ว่างอยู่" 
        List<Transform> availablePoints = new List<Transform>();
        foreach (Transform pt in spawnPoints)
        {
            if (pt.childCount == 0)
            {
                availablePoints.Add(pt);
            }
        }

        if (availablePoints.Count == 0) return;

        // 2. สุ่มจุดเกิดแท่น และ เสกแท่นออกมา
        Transform randomPoint = availablePoints[Random.Range(0, availablePoints.Count)];
        GameObject randomPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

        GameObject newPlatform = Instantiate(randomPrefab, randomPoint.position, Quaternion.identity);
        newPlatform.transform.SetParent(randomPoint);

        // ==========================================
        // 🌟 3. สั่งให้รอ 1.5 วินาที แล้วค่อยเสกศัตรู
        // ==========================================
        StartCoroutine(SpawnEnemyWithDelay(1.5f));
    }

    // ==========================================
    // 🌟 ฟังก์ชันจัดการเรื่องหน่วงเวลา และ เสกศัตรู (ยุบรวมให้เหลืออันเดียวแล้ว)
    // ==========================================
    IEnumerator SpawnEnemyWithDelay(float delayTime)
    {
        // หยุดรอก่อน... (ให้แท่นกางให้เสร็จก่อน)
        yield return new WaitForSeconds(delayTime);

        // เช็กก่อนว่ามี Prefab ศัตรู และได้ใส่จุดเกิดศัตรูมาแล้ว
        if (enemyPrefabs.Length > 0 && enemySpawnPoints.Length > 0)
        {
            int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            int targetEnemyCount = Random.Range(minEnemies, maxEnemies + 1);

            if (currentEnemies < targetEnemyCount)
            {
                // ดึงข้อมูล "แท่นทั้งหมด" ที่รอดชีวิตอยู่ "หลังจากผ่านไป 1.5 วิแล้ว"
                CrumblingPlatform[] activePlatforms = FindObjectsByType<CrumblingPlatform>(FindObjectsSortMode.None);

                List<Transform> availableEnemyPoints = new List<Transform>();

                // เช็กจุดเกิดศัตรูทีละจุด
                foreach (Transform ept in enemySpawnPoints)
                {
                    if (ept.childCount == 0) // จุดนี้ต้องว่างอยู่
                    {
                        bool hasPlatform = false;

                        // เรดาร์ทำงาน: วัดระยะกับแท่นทุกอันในฉาก
                        foreach (CrumblingPlatform plat in activePlatforms)
                        {
                            if (Vector2.Distance(ept.position, plat.transform.position) < 3f)
                            {
                                hasPlatform = true;
                                break;
                            }
                        }

                        // ถ้ามีแท่นรองรับ ถึงจะเก็บไว้เป็นจุดที่พร้อมเสก
                        if (hasPlatform)
                        {
                            availableEnemyPoints.Add(ept);
                        }
                    }
                }

                // สุ่มเสกศัตรูลงบนจุดที่ผ่านเงื่อนไข
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