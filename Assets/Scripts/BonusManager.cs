using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BonusManager : NetworkBehaviour
{
    public GameObject[] m_bonusPrefabs;

    public Transform m_bonusSpawnPositions;
    private List<Transform> m_spawnPositions = new List<Transform>();
    private List<GameObject> m_instantiatedBonuses = new List<GameObject>();

    private ServerManager m_serverManager;
    private bool m_gameWasStarted;

    private void Start()
    {
        m_serverManager = GameObject.FindGameObjectWithTag("ServerManager").GetComponent<ServerManager>();

        for (int i = 0; i < m_bonusSpawnPositions.childCount; i++)
        {
            m_spawnPositions.Add(m_bonusSpawnPositions.GetChild(i));
            m_instantiatedBonuses.Add(null);
        }
    }

    private void Update()
    {
        if (!m_gameWasStarted && m_serverManager.GameStarted)
        {
            m_gameWasStarted = true;
            InvokeRepeating("SpawnBonus", 0, 10);
        }
    }

    private void SpawnBonus()
    {
        if (m_bonusPrefabs.Length > 0)
        {
            List<int> freePositions = new List<int>();
            for (int i = 0; i < m_spawnPositions.Count; i++)
            {
                if (m_instantiatedBonuses[i] == null)
                {
                    freePositions.Add(i);
                }
            }

            if (freePositions.Count > 0)
            {
                int spawnPosition = freePositions[Random.Range(0, freePositions.Count)];
                m_instantiatedBonuses[spawnPosition] = Instantiate(m_bonusPrefabs[Random.Range(0, m_bonusPrefabs.Length)], m_spawnPositions[spawnPosition].position, m_spawnPositions[spawnPosition].rotation);
                m_instantiatedBonuses[spawnPosition].GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
