using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayersManager : Singleton<PlayersManager>
{
    public NetworkVariable<int> playersReady = new NetworkVariable<int>(0);

    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    public int PlayersInGame
    {
        get { return playersInGame.Value; }
    }

    private void Start()
    {
        playersReady.OnValueChanged += (previousValue, newValue) =>
        {
            if (newValue == 2)
            {
                SetActiveWaitPanelsCLientRpc(false);
            }
            else if (newValue == 0)
            {
                EditTextOnWaitPanelsCLientRpc("�������� ���������...");
                SetActiveWaitPlayersControllerCLientRpc(false);
                SetActiveWaitPanelsCLientRpc(true);
            }
        };

        //���������� � ������ ����������� �� ������� � �� ������� ������� �����������
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"����� {id} �����������...");
                playersInGame.Value++;
                if (playersInGame.Value == 1)
                {
                    PlayerResourses player = NetworkManager.ConnectedClients[id].PlayerObject.gameObject.GetComponent<PlayerResourses>();
                    player.callbackApponentRpcParams.Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { 0 }
                    };
                    EditTextOnWaitPanelsCLientRpc("������� ������ ����� ������!");
                    SetActiveWaitPlayersControllerCLientRpc(true);
                }

            }
            else
            {
                Debug.Log($"� �����������...");
                MainMenuControls.Instance.WaitPlayersPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        };

        //���������� � ������ ����������� �� ������� � �� ������� ������� �����������
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"����� {id} ����������...");
                playersInGame.Value--;
                playersReady.Value = 0;
            }
        };
    }

    public void PlayerReady()
    {
        playersReady.Value++;
    }

    [ClientRpc]
    private void EditTextOnWaitPanelsCLientRpc(string text)
    {
        MainMenuControls.Instance.WaitPlayersPanel.GetComponentInChildren<Text>().color = Color.red;
        MainMenuControls.Instance.WaitPlayersPanel.GetComponentInChildren<Text>().text = text;
    }

    [ClientRpc]
    private void SetActiveWaitPlayersControllerCLientRpc(bool flag)
    {
        MainMenuControls.Instance.WaitPlayersPanel.GetComponent<WaitPlayersController>().enabled = flag;
    }
    [ClientRpc]
    private void SetActiveWaitPanelsCLientRpc(bool flag)
    {
        MainMenuControls.Instance.WaitPlayersPanel.SetActive(flag);
        Time.timeScale = flag ? 0f : 1f;
    }
}
