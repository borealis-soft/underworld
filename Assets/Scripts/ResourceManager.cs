using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public Text GoldText;
    public Text HP_Text;
    [SerializeField]
    private int gold;
    [SerializeField]
    private int playerHP = 20;
    [SerializeField]
    private GameObject LoseMenu;

    private int defaulGold, defaultHP;
    private string goldStr, HPstr;
    public int Gold { get => gold; }
    void Start()
    {
        defaulGold = gold;
        defaultHP = playerHP;
        goldStr = GoldText.text;
        HPstr = HP_Text.text;
        MainMenuControls.Instance.ResetAll += Reset;
    }

    void Update()
    {
        GoldText.text = goldStr + gold;
        HP_Text.text = HPstr + playerHP;
    }

    public void BuildTower(int towerCost)
    {
        gold -= towerCost;
    }

    public void EnemyKill(int enemyCost)
    {
        gold += enemyCost;
    }

    public void ReduceHP(int damage)
    {
        playerHP -= Mathf.Min(damage, playerHP);
        if (playerHP <= 0)
        {
            Time.timeScale = 0;
            LoseMenu.SetActive(true);
        }
    }

    private void Reset()
    {
        gold = defaulGold;
        playerHP = defaultHP;
    }
}
