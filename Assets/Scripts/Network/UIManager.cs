using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Text GoldText;
    public Text HP_Text;
    public TextMeshProUGUI usersCountText;

    private string goldStr, HPstr;

    void Start()
    {
        goldStr = GoldText.text;
        HPstr = HP_Text.text;
    }

    void Update()
    {
        if (PlayerResourses.Singleton != null)
        {
            GoldText.text = goldStr + PlayerResourses.Singleton.Gold;
            HP_Text.text = HPstr + PlayerResourses.Singleton.PlayerHP;
        }
        usersCountText.text = $"Игроков подключено: {PlayersManager.Instance.PlayersInGame}";
    }
}
