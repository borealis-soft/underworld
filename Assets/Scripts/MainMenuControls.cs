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

    public GameObject WinMenu;
    public GameObject LoseMenu;
    public Spawner[] spawners;
    public Transform Towers;
    public Transform directionalLight;
    public static MainMenuControls Instance;

    public delegate void ResetFunc();
    public event ResetFunc ResetAll;

    private Quaternion defaultLigtRot;
    private int EnemyCountUntilWin = 0;
    private int DefaultEnemyUntilWin = 0;

    [SerializeField]
    private KeyCode BackKey = KeyCode.Escape;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Debug.LogError("Создано два MainMenuControls");
        ResetAll += Reset;
    }

    void Start()
    {
        EnemyCountUntilWin = 0;
        foreach (Spawner spawner in spawners)
            EnemyCountUntilWin += spawner.MaxCountEnemy;
        DefaultEnemyUntilWin = EnemyCountUntilWin;
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
                if (GameMode.Singleton == null || GameMode.Singleton.gameMod == GameMode.GameMods.SingleGame)
                    TowerController.instance.cell = null;
                else
                    NetworkTowerController.Instance.cell = null;
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

    public IEnumerator WaitOfWin()
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
        WinMenu.SetActive(true);
    }

    public void DecrementCountUntilWin(int _)
    {
        if (--EnemyCountUntilWin <= 0)
            StartCoroutine(WaitOfWin());
    }

    public void GamePause(bool flag)
    {
        MenuPanel.SetActive(flag);
        MiniMenu.SetActive(flag);
        Time.timeScale = flag ? 0 : 1;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void ResetGame() => ResetAll();

    private void Reset()
    {
        directionalLight.rotation = defaultLigtRot;
        EnemyCountUntilWin = DefaultEnemyUntilWin;
        for (int i = 0; i < Towers.childCount; i++)
            Destroy(Towers.GetChild(i).gameObject);
        Time.timeScale = 1;
    }
}
