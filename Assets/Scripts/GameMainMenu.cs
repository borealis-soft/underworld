using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;

public class GameMainMenu : MonoBehaviour
{

    public InputField inputIPAddrField;

    public void Exit()
    {
        Application.Quit();
    }

    public void StartHost()
    {
        GameMode.Singleton.gameMod = GameMode.GameMods.HostGame;
        SceneManager.LoadScene(4);
    }

    public void StartClient()
    {
        GameMode.Singleton.gameMod = GameMode.GameMods.ClientGame;
        UNetTransport transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        if (inputIPAddrField.text.Length <= 0)
            transport.ConnectAddress = "127.0.0.1";
        else
            transport.ConnectAddress = inputIPAddrField.text;

        SceneManager.LoadScene(4);
    }

    public void StartSingleGame(int sceneBuildIndex)
    {
        GameMode.Singleton.gameMod = GameMode.GameMods.SingleGame;
        SceneManager.LoadScene(sceneBuildIndex);
    }
}