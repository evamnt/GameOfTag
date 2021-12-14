using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnvironmentSpawner : NetworkBehaviour
{
    public List<GameObject> m_startingPrefabs;

    public void SpawnPrefabs()
    {
        foreach (GameObject prefab in m_startingPrefabs)
        {
            GameObject instantiatedObject = Instantiate(prefab, transform);
            instantiatedObject.GetComponent<NetworkObject>().Spawn();
        }
    }
}
