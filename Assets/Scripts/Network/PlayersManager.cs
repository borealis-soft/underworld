using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayersManager : Singleton<PlayersManager>
{
    public static ulong player1Id = 0, player2Id = ulong.MaxValue;

    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    public int PlayersInGame
    {
        get { return playersInGame.Value; }
    }

    private void Start()
    {
        //���������� � ������ ����������� �� ������� � �� ������� ������� �����������
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"����� {id} �����������...");
                playersInGame.Value++;
                var player = NetworkManager.Singleton.ConnectedClients[id].PlayerObject.GetComponent<PlayerResourses>();
                if (player2Id == ulong.MaxValue)
                {
                    player2Id = id;
                    player.side.Value = Side.Radiant;
                }
                else
                    player.side.Value = Side.Viewer;
            }
        };

        //���������� � ������ ����������� �� ������� � �� ������� ������� �����������
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"����� {id} ����������...");
                playersInGame.Value--;
                if (player2Id == id)
                    player2Id = ulong.MaxValue;
            }
        };
    }
}
