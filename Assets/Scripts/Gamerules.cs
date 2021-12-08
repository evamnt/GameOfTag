using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Gamerules : MonoBehaviour
{
    List<PlayerInfos> m_allPlayersInfos = new List<PlayerInfos>();

    public void RemovePlayer(ulong clientId)
    {
        for (int index = 0; index < m_allPlayersInfos.Count; index++)
        {
            if (m_allPlayersInfos[index].clientId == clientId)
            {
                m_allPlayersInfos.RemoveAt(index);
                break;
            }
        }
    }

    public void AddClient(ulong clientId, string nickname)
    {
        string formattedNickname = nickname.Remove(nickname.Length - 1);
        m_allPlayersInfos.Add(new PlayerInfos(clientId, formattedNickname));
    }

    public int GetPlayersNb()
    {
        return m_allPlayersInfos.Count;
    }

    public List<PlayerInfos> GetAllConnectedPlayers()
    {
        return m_allPlayersInfos;
    } 

    public void AssociatePlayerWithGameObject(ulong clientId, GameObject instantiatedPlayer)
    {
        int playerIndex = m_allPlayersInfos.FindIndex(p => p.clientId == clientId);
        PlayerInfos oldInfos = m_allPlayersInfos[playerIndex];
        m_allPlayersInfos[playerIndex] = new PlayerInfos(oldInfos.clientId, oldInfos.nickname, instantiatedPlayer);
    }

    public PlayerInfos GetWinner()
    {
        PlayerInfos winner = m_allPlayersInfos[0];
        for (int index = 1; index < m_allPlayersInfos.Count; index++)
        {
           if(m_allPlayersInfos[index].instantiatedPlayer.GetComponent<CatTimer>().Timer < winner.instantiatedPlayer.GetComponent<CatTimer>().Timer)
            {
                winner = m_allPlayersInfos[index];
            }
        }
        return winner;
    }
}
