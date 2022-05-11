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
        //Вызывается в момент подключения на сервере и на клиенте который подключился
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"Игрок {id} подключился...");
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

        //Вызывается в момент подключения на сервере и на клиенте который подключился
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"Игрок {id} отключился...");
                playersInGame.Value--;
                if (player2Id == id)
                    player2Id = ulong.MaxValue;
            }
        };
    }
}
