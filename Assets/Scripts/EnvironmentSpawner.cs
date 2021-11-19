using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnvironmentSpawner : NetworkBehaviour
{
    public List<GameObject> m_startingPrefabs;

    [ClientRpc]
    public void InitializeEnvironmentClientRpc()
    {
        foreach (GameObject prefab in m_startingPrefabs)
        {
            GameObject instantiatedObject = Instantiate(prefab, transform);
            if (instantiatedObject.GetComponent<NetworkObject>() != null) {
                instantiatedObject.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
