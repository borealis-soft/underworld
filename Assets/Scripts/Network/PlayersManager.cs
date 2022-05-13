using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayersManager : Singleton<PlayersManager>
{
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
                if (playersInGame.Value == 1)
                {
                    PlayerResourses player = NetworkManager.ConnectedClients[id].PlayerObject.gameObject.GetComponent<PlayerResourses>();
                    player.callbackApponentRpcParams.Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { 0 }
                    };
                }
            }
            else
            {
                Debug.Log($"Я подключился...");
            }
        };

        //Вызывается в момент подключения на сервере и на клиенте который подключился
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"Игрок {id} отключился...");
                playersInGame.Value--;
            }
        };
    }
}
