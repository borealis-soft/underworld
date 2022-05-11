using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public static GameMode Singleton;
    public GameMods gameMod;

    public enum GameMods
    {
        HostGame,
        ClientGame,
        SingleGame
    }

    void Awake()
    {
        if (Singleton == null)
            Singleton = this;
    }
}
