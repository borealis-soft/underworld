using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkSpawner : NetworkBehaviour
{
    [Serializable]
    public struct Wave : INetworkSerializable, IEquatable<Wave>
    {
        public Wave(int count, Enemys enemy)
        {
            CountEnemy = count;
            EnemyId = enemy;
        }
        public int CountEnemy;
        public Enemys EnemyId;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref CountEnemy);
            serializer.SerializeValue(ref EnemyId);
        }

        public bool Equals(Wave other)
        {
            return CountEnemy == other.CountEnemy && EnemyId == other.EnemyId;
        }
    }

    [Serializable]
    public class EnemyInfo
    {
        public Enemys EnemyId;
        public Enemy EnemyPrefab;
    }

    public Side Side_;
    public NetworkList<Wave> NetworkWaves;
    public Hashtable EnemyMap = new Hashtable();

    [SerializeField]
    private float TimeUntilNewRound = 30f;
    [SerializeField]
    private float TimeUntilSpawnEnemy = 1f;
    [SerializeField]
    private Transform[] points;
    [SerializeField]
    private List<EnemyInfo> Enemys;
    [SerializeField]
    private List<Wave> Waves;

    private float baseTimeUntilRound, baseTimeUntilSpawnEnemy;
    private int currentEnemyId = 0;
    private int currentWaveId = 0;

    private void Awake()
    {
        for (int i = 0; i < Enemys.Count; i++)
        {
            EnemyMap.Add(Enemys[i].EnemyId, Enemys[i].EnemyPrefab);
        }
        Enemys.Clear();
        NetworkWaves = new NetworkList<Wave>(Waves);
        Waves.Clear();
    }

    public void Start()
    {
        baseTimeUntilSpawnEnemy = TimeUntilSpawnEnemy;
        baseTimeUntilRound = TimeUntilNewRound;
    }

    void Update()
    {
        if (TimeUntilNewRound > 0)
            TimeUntilNewRound -= Time.deltaTime;
        else if (TimeUntilSpawnEnemy > 0)
            TimeUntilSpawnEnemy -= Time.deltaTime;
        else 
        {
            TimeUntilSpawnEnemy = baseTimeUntilSpawnEnemy;
            if (currentEnemyId >= NetworkWaves[currentWaveId].CountEnemy)
            {
                currentEnemyId = 0;
                currentWaveId++;
            }
            if (currentWaveId >= NetworkWaves.Count)
            {
                TimeUntilNewRound = baseTimeUntilRound;
                currentWaveId = 0;
            }

            Enemy newEnemy = EnemyMap[NetworkWaves[currentWaveId].EnemyId] as Enemy;
            Enemy enemy = Instantiate(newEnemy, transform.position, Quaternion.identity, transform);
            enemy.Points = points;
            enemy.Side_ = Side_;
            enemy.OnDeath += PlayerResourses.Singleton.EnemyKillServerRpc;
            enemy.OnPass += PlayerResourses.Singleton.ReduceHPServerRpc;
            currentEnemyId++;
        }
    }
}

public enum Enemys
{
    enemy,
    skeleton,
    enemyBoss,
}