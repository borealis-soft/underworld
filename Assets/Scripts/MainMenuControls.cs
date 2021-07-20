using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControls : MonoBehaviour
{
    [SerializeField]
    private GameObject TowerUpgrades;
    [SerializeField]
    private GameObject TowerShops;
    [SerializeField]
    private GameObject MiniMenu;
    [SerializeField]
    private GameObject MenuPanel;
    [SerializeField]
    private GameObject Settings;

    public Transform Cells;
    public ResourceManager rm;
    public Spawner spawner;
    public Transform Towers;
    public Transform directionalLight;

    private Quaternion defaultLigtRot;

    [SerializeField]
    private KeyCode BackKey = KeyCode.Escape;

    void Start()
    {
        defaultLigtRot = directionalLight.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(BackKey))
            if (TowerUpgrades.activeSelf)
            {
                TowerUpgrades.SetActive(false);
            }
            else if (TowerShops.activeSelf)
            {
                TowerShops.SetActive(false);
                TowerController.instance.cell = null;
            }
            else if (Settings.activeSelf)
            {
                Settings.SetActive(false);
                MiniMenu.SetActive(true);
            }
            else if (MiniMenu.activeSelf)
                GamePause(false);
            else GamePause(true);
    }

    public void GamePause(bool flag)
    {
        MenuPanel.SetActive(flag);
        MiniMenu.SetActive(flag);
        Time.timeScale = flag ? 0 : 1;
    }

    public void ResetGame()
    {
        Cells.gameObject.SetActive(false);
        rm.Reset();
        spawner.Reset();
        directionalLight.rotation = defaultLigtRot;
        for (int i = 0; i < Towers.childCount; i++)
            Destroy(Towers.GetChild(i).gameObject);
        Cells.gameObject.SetActive(true);
        Time.timeScale = 1;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
