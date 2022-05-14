using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerResourses : NetworkBehaviour
{
    public static PlayerResourses Singleton;
    public NetworkVariable<Side> side = new NetworkVariable<Side>(Side.None);
    public int Gold { get => gold.Value; }
    public int PlayerHP { get => playerHP.Value; }
    public NetworkVariable<int> gold = new NetworkVariable<int>(1000);
    public NetworkVariable<int> playerHP = new NetworkVariable<int>(20);
    public NetworkVariable<int> goldPerSecond = new NetworkVariable<int>(1);
    public ClientRpcParams callbackRpcParams;
    public ClientRpcParams callbackApponentRpcParams;
    public ClientRpcParams callbackViewersRpcParams;

    [HideInInspector]
    public NetworkSpawner MySpawner;

    private int defaulGold, defaultHP;
    private float time = 1;

    void Start()
    {
        if (IsLocalPlayer && Singleton == null)
            Singleton = this;
        defaulGold = gold.Value;
        defaultHP = playerHP.Value;

        if (IsOwner)
        {
            SetSideServerRpc();
            if (side.Value != Side.Viewer)
                 StartCoroutine(SetSpawner());
        }
        if (IsServer)
        {
            SetCallbackParams();
        }
        //MainMenuControls.Instance.ResetAll += Reset;
    }

    private void Update()
    {
        if (IsServer)
        {
            PassiveGoldGetting();
        }
    }

    private IEnumerator SetSpawner()
    {
        while(MySpawner == null)
        {
            if (side.Value != Side.None)
            {
                NetworkSpawner[] spawners = FindObjectsOfType<NetworkSpawner>();
                if (spawners.Length > 2)
                    Debug.LogError("На сцене слишком много спавнеров!");
                if (spawners != null && spawners.Length == 2)
                {
                    MySpawner = spawners[spawners[0].Side_ == side.Value ? 0 : 1];
                    if (MySpawner.Side_ != side.Value)
                        MySpawner = null;
                }
                break;
            }
            else
                yield return new WaitForSeconds(0.05f);
        }
    }

    private void PassiveGoldGetting()
    {
        if (time >= 0)
            time -= Time.deltaTime;
        else
        {
            time = 1;
            gold.Value += goldPerSecond.Value;
        }
    }

    private void SetCallbackParams()
    {
        callbackRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { OwnerClientId }
            }
        };

        callbackApponentRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { 1 }
            }
        };

        callbackViewersRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams()
        };
    }

    public void CreateTower(Towers buildingTower, Vector3 pos, Side side_)
    {
        GameObject tower = NetworkTowerController.towersUpgradeMap[buildingTower] as GameObject;
        var towerObj = Instantiate(tower, pos, tower.transform.rotation, NetworkTowerController.Instance.transform);
        towerObj.GetComponent<Tower>().side = side_;
    }

    public void GetGoldForBuild(Towers buildingTower)
    {
        GameObject tower = (GameObject)NetworkTowerController.towersUpgradeMap[buildingTower];
        int towerCost = tower.GetComponent<BuildProc>().TowerCost;
        gold.Value -= towerCost;

        SpawnTower spawnTower = tower.GetComponent<SpawnTower>();
        if (spawnTower != null)
        {
            goldPerSecond.Value += spawnTower.goldcount;
        }
    }

    private void OnSpawnTowerBuild(Towers buildingTower, ulong clientId)
    {
        GameObject tower = (GameObject)NetworkTowerController.towersUpgradeMap[buildingTower];
        SpawnTower spawnTower = tower.GetComponent<SpawnTower>();
        if (spawnTower != null)
        {
            NetworkObject client = NetworkManager.ConnectedClients[clientId].PlayerObject;
            PlayerResourses clientResources = client.gameObject.GetComponent<PlayerResourses>();
            clientResources.MySpawner.NetworkWaves.Add(new NetworkSpawner.Wave(spawnTower.enemyCount, spawnTower.spawnEnemyId, spawnTower.timeUntilSpawn));
        }
    }

    [ServerRpc]
    public void BuildTowerServerRpc(Towers buildingTower, Vector3 pos, ulong clientId)
    {
        GetGoldForBuild(buildingTower);
        CreateTowerClientRpc(buildingTower, pos, side.Value);
        OnSpawnTowerBuild(buildingTower, clientId);
    }

    [ClientRpc]
    public void CreateTowerClientRpc(Towers buildingTower, Vector3 pos, Side side_)
    {
        CreateTower(buildingTower, pos, side_);

        Collider[] hitColliders = Physics.OverlapBox(pos, new Vector3(0.9f, 1f, 0.9f) * 0.5f, Quaternion.identity, LayerMask.GetMask("Cell"));
        for (int i = 0; i < hitColliders.Length; i++)
        {
            var cell = hitColliders[i].GetComponent<NetworkCell>();
            if (cell != null)
                cell.CanBuild = false;
        }
    }

    [ServerRpc]
    public void UpgrateTowerServerRpc(Towers buildingTower, Vector3 pos)
    {
        GetGoldForBuild(buildingTower);
        UpgrateTowerClientRpc(buildingTower, pos, side.Value);
    }

    [ClientRpc]
    public void UpgrateTowerClientRpc(Towers buildingTower, Vector3 pos, Side side_)
    {
        Collider[] hitColliders = Physics.OverlapBox(pos, new Vector3(0.9f, 1f, 0.9f) * 0.5f, Quaternion.identity, LayerMask.GetMask("Tower"));
        Destroy(hitColliders[0].gameObject);
        CreateTower(buildingTower, pos, side_);
    }

    [ServerRpc]
    public void SetSideServerRpc()
    {
        var players = NetworkManager.Singleton.ConnectedClients;
        if (players.Count == 1)
        {
            side.Value = Side.Dire;
        }
        else if (players.Count == 2)
        {
            side.Value = Side.Radiant;
        }
        else
            side.Value = Side.Viewer;
    }

    [ServerRpc]
    public void EnemyKillServerRpc(int enemyCost)///////////////////////
    {
        gold.Value += enemyCost;
    }

    [ServerRpc]
    public void ReduceHPServerRpc(int damage)//////////////////////////
    {
        playerHP.Value -= Mathf.Min(damage, playerHP.Value);
        if (playerHP.Value <= 0)
        {
            SetActiveLoseMenuClientRpc(true, callbackRpcParams);
            SetActiveWinMenuClientRpc(true, callbackApponentRpcParams);
        }
    }

    [ClientRpc]
    private void SetActiveLoseMenuClientRpc(bool flag, ClientRpcParams clientRpcParams = default)
    {
        Time.timeScale = flag ? 0f : 1f;
        MainMenuControls.Instance.LoseMenu.SetActive(flag);
    }

    [ClientRpc]
    private void SetActiveWinMenuClientRpc(bool flag, ClientRpcParams clientRpcParams = default)
    {
        Time.timeScale = flag ? 0f : 1f;
        MainMenuControls.Instance.WinMenu.SetActive(flag);
    }

    [ServerRpc]
    public void ImReadyServerRpc()
    {
        PlayersManager.Instance.PlayerReady();
    }

    //private void Reset()
    //{
    //    gold = defaulGold;
    //    playerHP = defaultHP;
    //}

}
