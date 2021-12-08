using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CatManager : NetworkBehaviour
{
    private List<PlayerInfos> m_connectedPlayers;

    public void SetPlayerList(List<PlayerInfos> playerList)
    {
        m_connectedPlayers = playerList;
        SelectRandomCat();
    }

    void SelectRandomCat()
    {
        m_connectedPlayers[Random.Range(0, m_connectedPlayers.Count)].instantiatedPlayer.GetComponent<CatTimer>().SetPlayerStatus(true);
    }

    public void SetPlayerAsCat(GameObject catPlayer)
    {
        catPlayer.GetComponent<CatTimer>().SetPlayerStatus(true);
    }

}
