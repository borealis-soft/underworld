using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkTowerController : Singleton<NetworkTowerController>
{
    [Serializable]
    private class TowerInfo
    {
        public Towers id;
        public GameObject towerPrefab;
    }

    [HideInInspector]
    public NetworkCell cell;
    public GameObject towerInventoryMenu;
    public GameObject towerUpgradeMenu;
    public static Hashtable towersUpgradeMap = new Hashtable();

    [SerializeField]
    private List<TowerInfo> towersUpgrades;
    private void Awake()
    {
        for (int i = 0; i < towersUpgrades.Count; i++)
        {
            towersUpgradeMap.Add(towersUpgrades[i].id, towersUpgrades[i].towerPrefab);
        }
        towersUpgrades.Clear();
    }

    public void BuildInCell(int buildingTower)
    {
        int towerCost = ((GameObject)towersUpgradeMap[(Towers)buildingTower]).GetComponent<BuildProc>().TowerCost;
        if (PlayerResourses.Singleton.Gold >= towerCost)
        {
            //делается на сервере
            PlayerResourses.Singleton.BuildTowerServerRpc((Towers)buildingTower, cell.transform.position);
            cell = null;
            towerInventoryMenu.SetActive(false);
        }
        else GetComponent<AudioSource>().Play();
    }

    public void TownUpgrade(GameObject LastTower, int NetTowerUpgrade)
    {
        int towerCost = ((GameObject)towersUpgradeMap[(Towers)NetTowerUpgrade]).GetComponent<BuildProc>().TowerCost;
        if (PlayerResourses.Singleton.Gold >= towerCost)
        {
            //делается на сервере
            PlayerResourses.Singleton.UpgrateTowerServerRpc((Towers)NetTowerUpgrade, LastTower.transform.position);
            towerUpgradeMenu.SetActive(false);
        }
        else GetComponent<AudioSource>().Play();
    }
}

public enum Towers
{
    TowerLvl1,
    TowerLvl2,
    TowerLvl3,
    ArcheryLvl1,
    ArcheryLvl2,
    ArcheryLvl3,
    MagicLvl1,
    MagicLvl2,
    MagicLvl3,
    EnemySpawnerLvl1,
    EnemySpawnerLvl2,
    EnemySpawnerLvl3,
}
