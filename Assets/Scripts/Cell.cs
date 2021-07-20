using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private Material mainMaterial;
    [SerializeField]
    private Material overMaterial;
    public bool CanBuild;
    [SerializeField]
    private Vector3 boxSize;

    bool defaultFlagBuild;
    void Start()
    {
        CheckObj();
        defaultFlagBuild = CanBuild;
    }

    private void OnEnable()
    {
        CanBuild = defaultFlagBuild;
    }

    private void OnMouseOver()
    {
        if (CanBuild && !EventSystem.current.IsPointerOverGameObject())
            GetComponent<MeshRenderer>().material = overMaterial;
    }

    private void OnMouseExit()
    {
        if (CanBuild)
            GetComponent<MeshRenderer>().material = mainMaterial;
    }

    private void OnMouseUp()
    {
        if (CanBuild && !EventSystem.current.IsPointerOverGameObject())
        {
            TowerController.instance.cell = this;
            TowerController.instance.towerInventoryMenu.SetActive(true);
            TowerController.instance.towerUpgradeMenu.SetActive(false);
        }
    }

    public void CheckObj()
    {
        var hitColliders = Physics.OverlapBox(transform.position, boxSize * 0.5f, Quaternion.identity, LayerMask.GetMask("Default"));
        CanBuild = hitColliders.Length == 0;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(transform.position, boxSize);
    //}
}
