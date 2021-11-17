using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ServerManager : MonoBehaviour
{
    public GameObject m_serverButtons;
    private bool m_modeSelected = false;

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
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        m_modeSelected = true;
        Cursor.lockState = CursorLockMode.Locked;
        m_serverButtons.SetActive(false);
        NetworkManager.Singleton.StartClient();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    //static void SubmitNewPosition()
    //{
    //    if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
    //    {
    //        var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
    //        //var player = playerObject.GetComponent<HelloWorldPlayer>();
    //        //player.Move();
    //    }
    //}
}
