using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [HideInInspector]
    public Cell cell;
    public static TowerController instance;
    public GameObject towerInventoryMenu;
    public GameObject towerUpgradeMenu;
    public ResourceManager resourceManager;

    private void Awake()
    {
        instance = this;
    }

    public void BuildInCell(GameObject buildingTower)
    {
        int towerCost = buildingTower.GetComponent<BuildProc>().TowerCost;
        if (resourceManager.Gold >= towerCost)
        {
            resourceManager.BuildTower(towerCost);
            Instantiate(buildingTower, cell.transform.position, buildingTower.transform.rotation, transform).GetComponent<BuildProc>();
            cell.CanBuild = false;
            cell = null;
            towerInventoryMenu.SetActive(false);
        }
        else GetComponent<AudioSource>().Play();
    }

    public void TownUpgrade(GameObject lastTower, BuildProc TowerUpgrade)
    {
        int towerCost = TowerUpgrade.TowerCost;
        if (resourceManager.Gold >= towerCost)
        {
            resourceManager.BuildTower(towerCost);
            Instantiate(TowerUpgrade, lastTower.transform.position, lastTower.transform.rotation, lastTower.transform.parent);
            Destroy(lastTower);
            towerUpgradeMenu.SetActive(false);
        }
        else GetComponent<AudioSource>().Play();
    }
}
