using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Collections;

public struct PlayerInfos
{
    //Variable declaration
    public ulong clientId;
    public string nickname;
    public bool exists;
    public GameObject instantiatedPlayer;

    //public GameObject instantiatedPlayer;

    //Constructor
    public PlayerInfos(ulong clientId = 0, string nickname = "", GameObject instantiatedPlayer = null)
    {
        this.clientId = clientId;
        this.nickname = nickname;
        this.instantiatedPlayer = instantiatedPlayer;
        
        if (clientId != 0)
        {
            exists = true;
        }
        else
        {
            exists = false;
        }
    }
}