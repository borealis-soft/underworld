using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControls : MonoBehaviour
{
    [SerializeField]
    private GameObject inventory;
    [SerializeField]
    private KeyCode CloseKey = KeyCode.Escape;

    void Awake()
    {
        inventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory.activeSelf && Input.GetKeyDown(CloseKey))
        {
            inventory.SetActive(false);
            TowerController.instance.cell = null;
        }
    }
}
