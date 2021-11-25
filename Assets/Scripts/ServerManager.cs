using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ServerManager : MonoBehaviour
{
    public GameObject m_serverButtons;
    private bool m_modeSelected = false;

    private List<GameObject> m_connectedPlayers = new List<GameObject>();
    public Transform m_spawnPositions;
    private List<Transform> m_spawnPositionsList = new List<Transform>();
    private List<int> m_freeSpawnPoints = new List<int>();

    public EnvironmentSpawner m_environmentSpawner;
    public CatManager m_catManager;

    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            m_spawnPositionsList.Add(m_spawnPositions.GetChild(i));
            m_freeSpawnPoints.Add(i);
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50, 50, 800, 1600));

        if (m_modeSelected)
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    public void StartHost()
    {
        m_modeSelected = true;
        Cursor.lockState = CursorLockMode.Locked;
        m_serverButtons.SetActive(false);

        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;

        NetworkManager.Singleton.StartHost();

        m_environmentSpawner.InitializeEnvironmentClientRpc();
    }

    public void StartClient()
    {
        m_modeSelected = true;
        Cursor.lockState = CursorLockMode.Locked;
        m_serverButtons.SetActive(false);

        //If we want to request a password
        //NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("room password");

        NetworkManager.Singleton.StartClient();

        m_environmentSpawner.InitializeEnvironmentClientRpc();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        Debug.Log("Entered approval check");

        if (m_connectedPlayers.Count >= 6)
        {
            Debug.Log("Limit of players reached");
            //If approve is true, the connection gets added. If it's false. The client gets disconnected
            callback(false, null, false, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.Log("Player approved");
            int index = Random.Range(0, m_freeSpawnPoints.Count);
            Transform spawnPosition = m_spawnPositionsList[m_freeSpawnPoints[index]];
            m_freeSpawnPoints.RemoveAt(index);
            Debug.Log("Position attributed : " + spawnPosition.position);
            //If approve is true, the connection gets added. If it's false. The client gets disconnected
            callback(true, null, true, spawnPosition.position, spawnPosition.rotation);

            m_connectedPlayers.Add(NetworkManager.Singleton.ConnectedClientsList[NetworkManager.Singleton.ConnectedClientsList.Count - 1].PlayerObject.gameObject);
            m_catManager.UpdatePlayerList(m_connectedPlayers);
        }
    }
    
    // Select a random player from the list that will become the cat for the beginning of the game
    public void SelectRandomCat()
    {
        int index = Random.Range(0, m_connectedPlayers.Count);
        m_catManager.SetPlayerAsCat(m_connectedPlayers[index]);
    }
}

