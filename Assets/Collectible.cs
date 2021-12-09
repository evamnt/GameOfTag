using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Collectible : NetworkBehaviour
{
    private List<ulong> m_playerTouches = new List<ulong>();
    private ServerManager m_serverManager;

    private void Start()
    {
        if (IsHost)
        {
            m_serverManager = GameObject.FindGameObjectWithTag("ServerManager").GetComponent<ServerManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up, 180 * Time.deltaTime);

        if (IsHost)
        {
            CheckTouches();
        }
    }

    public void NotifyPlayerTouch(ulong clientId)
    {
        m_playerTouches.Add(clientId);
    }

    private void CheckTouches()
    {
        if (m_playerTouches.Count > 0)
        {
            GameObject closestPlayer = m_serverManager.GetPlayerFromId(m_playerTouches[0]);

            for (int i = 1; i < m_playerTouches.Count; i++)
            {
                GameObject currentPlayer = m_serverManager.GetPlayerFromId(m_playerTouches[i]);
                if (Vector3.Distance(currentPlayer.transform.position, transform.position) < Vector3.Distance(transform.position, closestPlayer.transform.position))
                {
                    closestPlayer = currentPlayer;
                }
            }

            closestPlayer.GetComponent<CMF.AdvancedWalkerController>().HasteClientRpc();

            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }
}
