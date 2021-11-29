using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Collections;

public struct PlayerInfos : INetworkSerializable
{
    //Variable declaration
    public ulong clientId;
    public FixedString64Bytes nickname;
    public bool exists;

    //public GameObject instantiatedPlayer;

    //Constructor
    public PlayerInfos(ulong clientId = 0, string nickname = "")
    {
        this.clientId = clientId;
        this.nickname = new FixedString64Bytes(nickname);
        
        if (clientId != 0)
        {
            exists = true;
        }
        else
        {
            exists = false;
        }
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref nickname);
    }
}