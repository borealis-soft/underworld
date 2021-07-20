using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadUpgradeMenu : MonoBehaviour
{
    public GameObject[] TowerUpgrades;
    public GameObject LastTower;

    [SerializeField]
    private GameObject buttonPrefab;

    List<GameObject> buttons;
    private void OnEnable()
    {
        buttons = new List<GameObject>();
        for (int i = 0; i < TowerUpgrades.Length; i++)
        {
            buttons.Add(Instantiate(buttonPrefab, transform.GetChild(0)));
            BuildProc build = TowerUpgrades[i].GetComponent<BuildProc>();
            Button currentButton = buttons[i].GetComponent<Button>();
            currentButton.transform.GetChild(0).GetComponent<Text>().text = build.Title + "\n÷ÂÌ‡: " + build.TowerCost;
            currentButton.GetComponent<Image>().sprite = build.SpritePrev;
            currentButton.onClick.AddListener(() => TowerController.instance.TownUpgrade(LastTower, build));
        }
    }

    private void OnDisable()
    {
        foreach (var button in buttons)
            Destroy(button);
        buttons.Clear();
    }
}
