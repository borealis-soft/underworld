using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour
{
    public MeshRenderer[] baseRenderers;

    [SerializeField]
    private GameObject[] TowerUpgrades;
    [SerializeField]
    private Material selectMaterial;

    private List<Material[]> originalMaterials;
    private List<List<Material>> multiplyMaterials;

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
    }

    private void OnMouseOver()
    {
        if (enabled && !EventSystem.current.IsPointerOverGameObject())
        {
            for (int i = 0; i < baseRenderers.Length; i++)
                baseRenderers[i].materials = multiplyMaterials[i].ToArray();
        }
    }

    private void OnMouseExit()
    {
        if (enabled)
        {
            for (int i = 0; i < baseRenderers.Length; i++)
                baseRenderers[i].materials = originalMaterials[i];
        }
    }

    private void OnMouseUp()
    {
        if (enabled && !EventSystem.current.IsPointerOverGameObject())
        {
            GameObject UpgradeMenu = TowerController.instance.towerUpgradeMenu;
            UpgradeMenu.GetComponent<LoadUpgradeMenu>().TowerUpgrades = TowerUpgrades;
            UpgradeMenu.GetComponent<LoadUpgradeMenu>().LastTower = gameObject;
            UpgradeMenu.SetActive(false);
            UpgradeMenu.SetActive(true);
            TowerController.instance.towerInventoryMenu.SetActive(false);
        }
    }
}
