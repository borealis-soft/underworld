using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour
{
    public MeshRenderer[] baseRenderers;
    public Side side;


    [SerializeField]
    private GameObject[] TowerUpgrades;
    [SerializeField]
    private Towers[] NetTowerUpgrades;
    [SerializeField]
    private GameObject Radius;
    [SerializeField]
    private Material selectMaterial;

    private List<Material[]> originalMaterials;
    private List<List<Material>> multiplyMaterials;
    private bool CanUpgrade => TowerUpgrades.Length != 0;
    private bool IsTowerOwner
    {
        get
        {
            if (GameMode.Singleton == null || GameMode.Singleton.gameMod == GameMode.GameMods.SingleGame)
                return true;
            return PlayerResourses.Singleton != null & side == PlayerResourses.Singleton.side.Value;
        }
    }


    private void Start()
    {
        originalMaterials = new List<Material[]>();
        multiplyMaterials = new List<List<Material>>();
        for (int i = 0; i < baseRenderers.Length; i++)
        {
            originalMaterials.Add(baseRenderers[i].materials);
            multiplyMaterials.Add(new List<Material>(baseRenderers[i].materials));
            multiplyMaterials[i].Add(selectMaterial);
        }
        if (tag != "NotReset" && GetComponent<Костыльище>() != null)
            MainMenuControls.Instance.ResetAll += Reset;
    }

    private void OnMouseOver()
    {
        if (enabled && IsTowerOwner && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Radius) Radius.SetActive(true);
            for (int i = 0; i < baseRenderers.Length; i++)
                baseRenderers[i].materials = multiplyMaterials[i].ToArray();
        }
    }

    private void OnMouseExit()
    {
        if (enabled)
        {
            if (Radius) Radius?.SetActive(false);
            for (int i = 0; i < baseRenderers.Length; i++)
                baseRenderers[i].materials = originalMaterials[i];
        }
    }

    private void OnMouseUp()
    {
        if (enabled && CanUpgrade && IsTowerOwner && !EventSystem.current.IsPointerOverGameObject())
        {
            if (GameMode.Singleton == null || GameMode.Singleton.gameMod == GameMode.GameMods.SingleGame)
            {
                GameObject UpgradeMenu = TowerController.instance.towerUpgradeMenu;
                UpgradeMenu.GetComponent<LoadUpgradeMenu>().TowerUpgrades = TowerUpgrades;
                UpgradeMenu.GetComponent<LoadUpgradeMenu>().LastTower = gameObject;
                UpgradeMenu.SetActive(false);
                UpgradeMenu.SetActive(true);
                TowerController.instance.towerInventoryMenu.SetActive(false);
            }
            else
            {
                GameObject UpgradeMenu = NetworkTowerController.Instance.towerUpgradeMenu;
                UpgradeMenu.GetComponent<LoadUpgradeMenu>().TowerUpgrades = TowerUpgrades;
                UpgradeMenu.GetComponent<LoadUpgradeMenu>().NetTowerUpgrades = NetTowerUpgrades;
                UpgradeMenu.GetComponent<LoadUpgradeMenu>().LastTower = gameObject;
                UpgradeMenu.SetActive(false);
                UpgradeMenu.SetActive(true);
                NetworkTowerController.Instance.towerInventoryMenu.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        if (tag != "NotReset")
            MainMenuControls.Instance.ResetAll -= Reset;
    }

    public void Reset()
    {
        Destroy(gameObject);
    }
}
