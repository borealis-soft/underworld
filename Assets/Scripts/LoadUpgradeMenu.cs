using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadUpgradeMenu : MonoBehaviour
{
    [HideInInspector]
    public Towers[] NetTowerUpgrades;//Должны совпадать с TowerUpgrades !!!исправить!!!
    [HideInInspector]
    public GameObject[] TowerUpgrades;
    [HideInInspector]
    public GameObject LastTower;

    [SerializeField]
    private GameObject buttonPrefab;

    List<GameObject> buttons = new List<GameObject>();
    private void OnEnable()
    {
        for (int i = 0; i < TowerUpgrades.Length; i++)
        {
            buttons.Add(Instantiate(buttonPrefab, transform.GetChild(0)));
            BuildProc build = TowerUpgrades[i].GetComponent<BuildProc>();
            Button currentButton = buttons[i].GetComponent<Button>();
            currentButton.transform.GetChild(0).GetComponent<Text>().text = build.Title + "\nЦена: " + build.TowerCost;
            currentButton.GetComponent<Image>().sprite = build.SpritePrev;
            if (GameMode.Singleton == null || GameMode.Singleton.gameMod == GameMode.GameMods.SingleGame)
                currentButton.onClick.AddListener(() => TowerController.instance.TownUpgrade(LastTower, build));
            else
            {
                int id = (int)NetTowerUpgrades[i];
                currentButton.onClick.AddListener(() => NetworkTowerController.Instance.TownUpgrade(LastTower, id));
            }
        }
    }

    private void OnDisable()
    {
        foreach (var button in buttons)
            Destroy(button);
        buttons.Clear();
    }
}
