using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

using static GameMode;
public class StartConnect : MonoBehaviour
{

    private void Start()
    {
        if (Singleton != null && Singleton.gameMod != GameMods.SingleGame)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;

            switch (Singleton.gameMod)
            {
                case GameMods.HostGame:
                    NetworkManager.Singleton.StartHost();
                    break;
                case GameMods.ClientGame:
                    NetworkManager.Singleton.StartClient();
                    break;
                default:
                    Debug.LogError("Неверно выставленный режим!");
                    break;
            }
        }
    }

    private void ApprovalCheck(byte[] connettionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {
        bool approved = true;
        Vector3 newPos = Vector3.zero;
        Quaternion newQat = Quaternion.identity;

        switch (clientID)
        {
            case 0:// first player
                newPos = new Vector3(10, 10, 15.5f);
                newQat = Quaternion.Euler(64, 0, 0);
                break;
            case 1:// second player
                newPos = new Vector3(10, 10, 15.5f);
                newQat = Quaternion.Euler(64, 180, 0);
                break;
            default:// other players
                newPos = new Vector3(10, 10, 15.5f);
                newQat = Quaternion.Euler(64, 90, 0);
                break;
        }

        callback(true, null, approved, newPos, newQat);
    }

}
