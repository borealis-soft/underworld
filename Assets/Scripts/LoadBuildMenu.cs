using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBuildMenu : MonoBehaviour
{
    [HideInInspector]
    public Towers[] buildTowers;

    [SerializeField]
    private GameObject ButtonPrifab;

    List<GameObject> buttons = new List<GameObject>();

    private void OnEnable()
    {
        for (int i = 0; i < buildTowers.Length; i++)
        {
            buttons.Add(Instantiate(ButtonPrifab, transform.GetChild(0)));
            BuildProc tower = (NetworkTowerController.towersUpgradeMap[buildTowers[i]] as GameObject).GetComponent<BuildProc>();
            Button currentButton = buttons[i].GetComponent<Button>();
            currentButton.transform.GetChild(0).GetComponent<Text>().text = tower.Title + "\n÷ÂÌ‡: " + tower.TowerCost;
            currentButton.GetComponent<Image>().sprite = tower.SpritePrev;
            int id = (int)buildTowers[i];
            currentButton.onClick.AddListener(() => NetworkTowerController.Instance.BuildInCell(id));
        }
    }

    private void OnDisable()
    {
        foreach (GameObject button in buttons)
        {
            Destroy(button);
        }
        buttons.Clear();
    }
}
