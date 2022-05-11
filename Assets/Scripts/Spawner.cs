using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public int MaxCountEnemy { get; private set; } = 0;
    //public FlyBullet MegaUltaBulletPref;
    //public Transform MegaUltaPoint;

    [SerializeField]
    private MainMenuControls MainMenu;
    [SerializeField]
    private ResourceManager resourceManager;
    [SerializeField]
    private Transform[] points;
    [SerializeField]
    private Wave[] waves;

    private int currentWave = 0;
    private int enemyCount = 0;
    private float timeUntilWave = 0;
    private float spawnDelay = 0;

    public bool SpawnFinished
    {
        get => enemyCount == 0 && currentWave == waves.Length;
    }

    private void Reset()
    {
        Prepare();
        currentWave = 0;
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }

    private void Awake()
    {
        foreach (var wave in waves)
            MaxCountEnemy += wave.EnemyCount;
    }

    void Start()
    {
        Prepare();
        MainMenuControls.Instance.ResetAll += Reset;
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
                Enemy enemy = Instantiate(waves[currentWave].EnemyPrefab, transform.position, Quaternion.identity, transform);
                enemy.Points = points;
                enemy.OnDeath += resourceManager.EnemyKill;
                enemy.OnDeath += MainMenu.DecrementCountUntilWin;
                enemy.OnPass += resourceManager.ReduceHP;
                enemy.OnPass += MainMenu.DecrementCountUntilWin;
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

    private void Prepare()
    {
        enemyCount = waves[0].EnemyCount;
        timeUntilWave = waves[0].Delay;
    }
}

//[CustomEditor(typeof(Spawner))]
//public class SpawnerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        if (GUILayout.Button("Kill all"))
//        {
//            if (Application.isPlaying)
//            {
//                Spawner spawner = target as Spawner;
//                for (int i = 0; i < spawner.transform.childCount; i++)
//                {
//                    var bullet = Instantiate(spawner.MegaUltaBulletPref, spawner.MegaUltaPoint);
//                    bullet.Damage = 9999999;
//                    bullet.Target = spawner.transform.GetChild(i);
//                }
//            }
//            else Debug.Log("It only works during the game!");
//        }
//    }
//}