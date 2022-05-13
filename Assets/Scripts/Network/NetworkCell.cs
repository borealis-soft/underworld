using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class NetworkCell : MonoBehaviour
{
    public bool CanBuild = false;
    public Side side;
    public Towers[] buildTowers;

    [SerializeField]
    private Material mainMaterial;
    [SerializeField]
    private Material overMaterial;
    [SerializeField]
    private Vector3 boxSize;
    private bool IsCellOwner
    {
        get 
        {
            return PlayerResourses.Singleton != null && side == PlayerResourses.Singleton.side.Value; 
        }
    } 

    bool defaultFlagBuild;
    void Start()
    {
        CheckObj();
        defaultFlagBuild = CanBuild;
        MainMenuControls.Instance.ResetAll += () => CanBuild = defaultFlagBuild;
    }

    private void OnEnable()
    {
        CanBuild = defaultFlagBuild;
    }

    private void OnMouseOver()
    {
        if (CanBuild && IsCellOwner&& !EventSystem.current.IsPointerOverGameObject())
            GetComponent<MeshRenderer>().material = overMaterial;
    }

    private void OnMouseExit()
    {
        if (GetComponent<MeshRenderer>().material != mainMaterial)
            GetComponent<MeshRenderer>().material = mainMaterial;
    }

    private void OnMouseUp()
    {
        if (CanBuild && IsCellOwner && !EventSystem.current.IsPointerOverGameObject())
        {
            NetworkTowerController.Instance.cell = this;
            NetworkTowerController.Instance.towerInventoryMenu.GetComponent<LoadBuildMenu>().buildTowers = buildTowers;
            NetworkTowerController.Instance.towerInventoryMenu.SetActive(false);
            NetworkTowerController.Instance.towerInventoryMenu.SetActive(true);
            NetworkTowerController.Instance.towerUpgradeMenu.SetActive(false);
        }
    }

    public void CheckObj()
    {
        var hitColliders = Physics.OverlapBox(transform.position, boxSize * 0.5f, Quaternion.identity, LayerMask.GetMask("Default"));
        CanBuild = hitColliders.Length == 0;
    }
}

public enum Side
{
    None,
    Dire,
    Radiant,
    Viewer
}