using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CatManager : NetworkBehaviour
{
    private List<GameObject> m_connectedPlayers;

    public void UpdatePlayerList(List<GameObject> playerList)
    {
        m_connectedPlayers = playerList;
    }

    public void SetPlayerAsCat(GameObject catPlayer)
    {
        catPlayer.GetComponent<CatTimer>().SetPlayerStatus(true);
    }

}
