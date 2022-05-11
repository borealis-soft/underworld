using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerResourses : NetworkBehaviour
{
    public static PlayerResourses Singleton;
    public NetworkVariable<Side> side = new NetworkVariable<Side>(Side.Dire);
    public int Gold { get => gold.Value; }
    public int PlayerHP { get => playerHP.Value; }
    public NetworkVariable<int> gold = new NetworkVariable<int>(1000);
    public NetworkVariable<int> playerHP = new NetworkVariable<int>(20);
    private int defaulGold, defaultHP;

    void Start()
    {
        if (IsLocalPlayer && Singleton == null)
            Singleton = this;
        defaulGold = gold.Value;
        defaultHP = playerHP.Value;
        //MainMenuControls.Instance.ResetAll += Reset;
    }

    public void CreateTower(Towers buildingTower, Vector3 pos, Side side_)
    {
        GameObject tower = NetworkTowerController.towersUpgradeMap[buildingTower] as GameObject;
        var towerObj = Instantiate(tower, pos, tower.transform.rotation, NetworkTowerController.Instance.transform);
        towerObj.GetComponent<Tower>().side = side_;
    }

    public void GetGoldForBuild(Towers buildingTower)
    {
        int towerCost = ((GameObject)NetworkTowerController.towersUpgradeMap[buildingTower]).GetComponent<BuildProc>().TowerCost;
        gold.Value -= towerCost;
    }

    [ServerRpc]
    public void BuildTowerServerRpc(Towers buildingTower, Vector3 pos)
    {
        GetGoldForBuild(buildingTower);
        CreateTowerClientRpc(buildingTower, pos, side.Value);
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

    public void EnemyKill(int enemyCost)///////////////////////
    {
        gold.Value += enemyCost;
    }

    public void ReduceHP(int damage)//////////////////////////
    {
        playerHP.Value -= Mathf.Min(damage, playerHP.Value);
        if (playerHP.Value <= 0)
        {
            Time.timeScale = 0;
            //LoseMenu.SetActive(true);
        }
    }

    //private void Reset()
    //{
    //    gold = defaulGold;
    //    playerHP = defaultHP;
    //}

}
