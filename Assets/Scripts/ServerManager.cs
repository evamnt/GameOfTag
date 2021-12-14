using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System.Net;
using System.Linq;

public class ServerManager : NetworkBehaviour
{
    [Header("Generic UIs")]
    public GameObject m_chooseModeUI;

    [Header("Server UIs")]
    public GameObject m_serverUI;
    public GameObject m_connectedPlayersUI;

    [Header("Client UIs")]
    public GameObject m_clientUI;

    [Header("Prefabs")]
    public GameObject m_playerPrefab;

    [Header("Texts")]
    public GameObject m_warningMsg;
    public TMP_Text m_name;
    public TMP_Text m_ipAddress;

    [Header("Games Infos")]
    public Transform m_spawnPositions;
    public BonusManager m_bonusManager;
    public EnvironmentSpawner m_environmentSpawner;

    [Header("Prefabs")]
    //Only used by the server
    public GameObject m_gamerulesPrefab;
    private Gamerules m_gamerules;
    public GameObject m_catManagerPrefab;
    private CatManager m_catManager;

    private List<Transform> m_spawnPositionsList = new List<Transform>();
    private List<int> m_freeSpawnPoints = new List<int>();
    private bool m_creatingHost;
    private bool m_gameStarted = false;
    private bool m_gameFinished = false;

    private List<TMP_Text> m_connectedPlayersTextBoxes = new List<TMP_Text>();
    private TMP_Text m_playersNb;
    private List<PlayerInfos> m_connectedPlayers = new List<PlayerInfos>();

    public bool GameStarted
    {
        get { return m_gameStarted; }
        set { m_gameStarted = value; }
    }

    public bool GameFinished
    {
        set { m_gameFinished = value; }
        private get { return m_gameFinished; }
    }
    private void Start()
    {
        m_warningMsg.SetActive(false);
        m_chooseModeUI.SetActive(true);
        m_serverUI.SetActive(false);
        m_clientUI.SetActive(false);
        for (int i = 0; i < 6; i++)
        {
            m_spawnPositionsList.Add(m_spawnPositions.GetChild(i));
            m_freeSpawnPoints.Add(i);
        }

        for (int i = 1; i < 7; i++)
        {
            TMP_Text currentText = m_connectedPlayersUI.transform.GetChild(i).GetComponent<TMP_Text>();
            currentText.text = "";
            m_connectedPlayersTextBoxes.Add(currentText);
        }
        m_playersNb = m_connectedPlayersUI.transform.GetChild(7).GetComponent<TMP_Text>();
        m_playersNb.text = "0 / " + m_connectedPlayersTextBoxes.Count;
    }

    private void Update()
    {
        if ((GameStarted || GameFinished) && Camera.allCamerasCount == 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Disconnect();
        }
    }

    private bool CheckNickname()
    {
        string nickname = m_name.text.Remove(m_name.text.Length - 1);
        if (string.IsNullOrEmpty(nickname))
        {
            m_warningMsg.SetActive(true);
            m_warningMsg.GetComponent<TMP_Text>().text = "Please enter a nickname";
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckIPAddress()
    {
        string ipString = m_ipAddress.text.Remove(m_ipAddress.text.Length - 1);
        if (string.IsNullOrEmpty(ipString))
        {
            m_warningMsg.SetActive(true);
            m_warningMsg.GetComponent<TMP_Text>().text = "Please enter a IPv4";
            return false;
        }

        //The only different format that we accept is "localhost"
        if (ipString.ToLower() == "localhost")
        {
            m_ipAddress.text = ipString.ToLower();
            return true;
        }

        string[] splitValues = ipString.Split('.');
        if (splitValues.Length != 4)
        {
            m_warningMsg.SetActive(true);
            m_warningMsg.GetComponent<TMP_Text>().text = "A valid IPv4 address contains 4 short int values separated by '.' characters";
            return false;
        }

        byte tempForParsing;
        if (splitValues.All(r => byte.TryParse(r, out tempForParsing))) {
            return true;
        }
        else
        {
            m_warningMsg.SetActive(true);
            m_warningMsg.GetComponent<TMP_Text>().text = "A valid IPv4 address contains only short int values";
            return false;
        }
    }

    private void UpdateConnectedPlayersUI()
    {
        m_connectedPlayers = m_gamerules.GetAllConnectedPlayers();
        int clientNum;
        for (clientNum = 0; clientNum < m_connectedPlayers.Count; clientNum++)
        {
            m_connectedPlayersTextBoxes[clientNum].text = "- " + m_connectedPlayers[clientNum].nickname;
        }
        for (int clientToReset = clientNum; clientToReset < m_connectedPlayersTextBoxes.Count; clientToReset++)
        {
            m_connectedPlayersTextBoxes[clientToReset].text = "";
        }

        m_playersNb.text = clientNum + " / " + m_connectedPlayersTextBoxes.Count;
    }

    private void UpdateIPAddress()
    {
        NetworkManager.Singleton.gameObject.GetComponent<Unity.Netcode.Transports.UNET.UNetTransport>().ConnectAddress = m_ipAddress.text.Remove(m_ipAddress.text.Length - 1);
    }

    public void ButtonHost()
    {
        if (CheckIPAddress())
        {
            if (CheckNickname())
            {
                NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
                NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(m_name.text);
                NetworkManager.Singleton.OnClientDisconnectCallback += DisconnectionCallback;
                m_creatingHost = true;

                UpdateIPAddress();
                if (NetworkManager.Singleton.StartHost())
                {
                    m_chooseModeUI.SetActive(false);
                    m_serverUI.SetActive(true);

                    //We have instantiated the host, now we wait for clients
                    m_creatingHost = false;
                }
            }
        }
    }

    public void ButtonClient()
    {
        if (CheckIPAddress())
        {
            if (CheckNickname())
            {
                NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(m_name.text);
                m_creatingHost = false;

                UpdateIPAddress();
                if (NetworkManager.Singleton.StartClient())
                {
                    m_chooseModeUI.SetActive(false);
                    m_clientUI.SetActive(true);
                }
            }
        }
    }

    public void ButtonStartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;

        for(int i = 0; i < m_connectedPlayers.Count; i++)
        {
            int positionIndex = Random.Range(0, m_freeSpawnPoints.Count);
            Transform currentPlayerPosition = m_spawnPositionsList[m_freeSpawnPoints[positionIndex]];
            m_freeSpawnPoints.RemoveAt(positionIndex);
            GameObject instantiatedPlayer = Instantiate(m_playerPrefab, currentPlayerPosition.position, currentPlayerPosition.rotation);
            instantiatedPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(m_connectedPlayers[i].clientId);
            //Give the instantiated player its nickname

            m_gamerules.AssociatePlayerWithGameObject(m_connectedPlayers[i].clientId, instantiatedPlayer);
        }

        m_connectedPlayers = m_gamerules.GetAllConnectedPlayers();

        GameObject instantiatedCatManager = Instantiate(m_catManagerPrefab);
        m_catManager = instantiatedCatManager.GetComponent<CatManager>();
        m_catManager.SetPlayerList(m_connectedPlayers);
        m_environmentSpawner.SpawnPrefabs();
        GameStarted = true;
        m_bonusManager.enabled = true;
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
        if (m_creatingHost)
        {
            //If approve is true, the connection gets added. If it's false. The client gets disconnected
            callback(false, null, true, Vector3.zero, Quaternion.identity);

            string connectedPlayerName = System.Text.Encoding.ASCII.GetString(connectionData);

            GameObject instantiatedGamerule = Instantiate(m_gamerulesPrefab);
            m_gamerules = instantiatedGamerule.GetComponent<Gamerules>();

            m_gamerules.AddClient(clientId, connectedPlayerName);
            UpdateConnectedPlayersUI();
        }
        else if (m_gamerules.GetPlayersNb() >= 6)
        {
            Debug.Log("Limit of players reached");
            //If approve is true, the connection gets added. If it's false. The client gets disconnected
            callback(false, null, false, Vector3.zero, Quaternion.identity);
        }
        else if (!GameStarted)
        {
            //If approve is true, the connection gets added. If it's false. The client gets disconnected
            callback(false, null, true, Vector3.zero, Quaternion.identity);

            string connectedPlayerName = System.Text.Encoding.ASCII.GetString(connectionData);

            m_gamerules.AddClient(clientId, connectedPlayerName);
            UpdateConnectedPlayersUI();
        }
    }
    
    private void DisconnectionCallback(ulong clientId)
    {
        m_gamerules.RemovePlayer(clientId);
        m_connectedPlayers = m_gamerules.GetAllConnectedPlayers();
        UpdateConnectedPlayersUI();
    }

    public void Disconnect()
    {
        GameObject.FindObjectOfType<CMF.DemoMenu>().RestartScene();
    }

    public GameObject GetPlayerFromId(ulong clientId)
    {
        List<PlayerInfos> players = m_gamerules.GetAllConnectedPlayers();

        foreach (PlayerInfos player in players)
        {
            if (player.clientId == clientId)
            {
                return player.instantiatedPlayer;
            }
        }

        return null;
    }
}

