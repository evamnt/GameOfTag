using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class ServerManager : NetworkBehaviour
{
    [Header("UIs")]
    public GameObject m_chooseModeUI;
    public GameObject m_startGameUI;
    public GameObject m_connectedPlayersUI;

    [Header("Texts")]
    public GameObject m_warningName;
    public TMP_Text m_name;
    
    [Header("Games Infos")]
    public Transform m_spawnPositions;
    public EnvironmentSpawner m_environmentSpawner;
    public CatManager m_catManager;

    private List<TMP_Text> m_connectedPlayersTextBoxes = new List<TMP_Text>();
    private TMP_Text m_playersNb;

    private Dictionary<ulong, GameObject> m_connectedPlayers = new Dictionary<ulong, GameObject>();
    private Dictionary<ulong, string> m_connectedPlayersNames = new Dictionary<ulong, string>();
    private List<Transform> m_spawnPositionsList = new List<Transform>();
    private List<int> m_freeSpawnPoints = new List<int>();

    private void Start()
    {
        m_warningName.SetActive(false);
        for (int i = 0; i < 6; i++)
        {
            m_spawnPositionsList.Add(m_spawnPositions.GetChild(i));
            m_freeSpawnPoints.Add(i);
        }

        for (int i = 1; i < 7; i++)
        {
            m_connectedPlayersTextBoxes.Add(m_connectedPlayersUI.transform.GetChild(i).GetComponent<TMP_Text>());
            m_connectedPlayersTextBoxes[m_connectedPlayersTextBoxes.Count - 1].text = "";
        }
        m_playersNb = m_connectedPlayersUI.transform.GetChild(7).GetComponent<TMP_Text>();
        m_playersNb.text = "";
    }

    private void RefreshConnectedPlayersUI()
    {
        ResetPlayerNameUIClientRpc();
        int i = 0;
        foreach (string name in m_connectedPlayersNames.Values)
        {
            AddPlayerNameToUIClientRpc(name, i);
            i++;
        }
    }

    [ClientRpc]
    void ResetPlayerNameUIClientRpc()
    {
        int i = 0;
        foreach (string name in m_connectedPlayersNames.Values)
        {
            m_connectedPlayersTextBoxes[i].text = "";
            i++;
        }
        m_playersNb.text = "0 / 6";
    }

    [ClientRpc]
    void AddPlayerNameToUIClientRpc(string playerName, int i)
    {
        m_connectedPlayersTextBoxes[i].text = "- " + playerName;
        m_playersNb.text = i + 1 + " / 6";
    }

    private bool CheckNickname()
    {
        if (m_name.text.Length <= 1)
        {
            m_warningName.SetActive(true);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ButtonHost()
    {
        if (CheckNickname())
        {
            m_chooseModeUI.SetActive(false);
            m_connectedPlayersUI.SetActive(true);
            m_startGameUI.SetActive(true);
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(m_name.text);
            if (NetworkManager.Singleton.StartHost()) 
            {
                RefreshConnectedPlayersUI();
            }
        }
    }

    public void ButtonClient()
    {
        if (CheckNickname())
        {
            m_chooseModeUI.SetActive(false);
            m_connectedPlayersUI.SetActive(true);
            //If we want to request a password
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(m_name.text);
            if (NetworkManager.Singleton.StartClient())
            {
                RefreshConnectedPlayersUI();
            }
        }
    }

    public void StartHost()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_environmentSpawner.InitializeEnvironmentClientRpc();
    }

    public void StartClient()
    {
        Cursor.lockState = CursorLockMode.Locked;

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
            //If approve is true, the connection gets added. If it's false. The client gets disconnected
            callback(false, null, true, Vector3.zero, Quaternion.identity);

            string connectedPlayerName = System.Text.Encoding.ASCII.GetString(connectionData);
            m_connectedPlayers.Add(clientId, null);
            m_connectedPlayersNames.Add(clientId, connectedPlayerName);
        }
    }
    
    // Select a random player from the list that will become the cat for the beginning of the game
    public void SelectRandomCat()
    {
        //int index = Random.Range(0, m_connectedPlayers.Count);
        //m_catManager.SetPlayerAsCat(m_connectedPlayers[index]);
    }
}

