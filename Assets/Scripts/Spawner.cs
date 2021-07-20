using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    private class Wave
    {
        public float TimeToSpawn;
        public Enemy EnemyPrefab;
        public int EnemyCount;
        public float Delay;
    }

    [SerializeField]
    private ResourceManager resourceManager;
    [SerializeField]
    private GameObject WinMenu;
    [SerializeField]
    private Transform[] points;
    [SerializeField]
    private Wave[] waves;

    private int spawnCount = 0;
    private int currentWave = 0;
    private int enemyCount = 0;
    private float timeUntilWave = 0;
    private float spawnDelay = 0;

    public bool SpawnFinished
    {
        get => enemyCount == 0 && currentWave == waves.Length;
    }

    public void Reset()
    {
        Prepare();
        spawnCount = 0;
        currentWave = 0;
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }

    void Start()
    {
        Prepare();
    }

    void Update()
    {
        if (currentWave < waves.Length)
        {
            if (timeUntilWave > 0)
                timeUntilWave -= Time.deltaTime;
            else if (spawnDelay > 0)
                spawnDelay -= Time.deltaTime;
            else if (enemyCount > 0)
            {
                spawnCount++;
                Enemy enemy = Instantiate(waves[currentWave].EnemyPrefab, transform.position, Quaternion.identity, transform);
                enemy.Points = points;
                enemy.OnDeath += resourceManager.EnemyKill;
                enemy.OnDeath += CheckWin;
                enemy.OnPass += resourceManager.ReduceHP;
                enemy.OnPass += CheckWin;
                if (--enemyCount <= 0)
                {
                    if (++currentWave < waves.Length)
                    {
                        timeUntilWave = waves[currentWave].Delay;
                        enemyCount = waves[currentWave].EnemyCount;
                    }
                }
                else spawnDelay = waves[currentWave].TimeToSpawn;
            }
        }
    }

    private IEnumerator WaitAndWin()
    {
        yield return new WaitForSeconds(3);
        Time.timeScale = 0;
        WinMenu.SetActive(true);
    }

    private void CheckWin(int _)
    {
        if (--spawnCount <= 0 && !WinMenu.activeSelf && SpawnFinished)
            StartCoroutine(WaitAndWin());
    }

    private void Prepare()
    {
        enemyCount = waves[0].EnemyCount;
        timeUntilWave = waves[0].Delay;
    }
}