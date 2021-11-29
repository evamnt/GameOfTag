using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        GameObject[] menuUis = GameObject.FindGameObjectsWithTag("MainMenuUI");

        foreach (GameObject menuUI in menuUis)
        {
            menuUI.SetActive(false);
        }

        GameObject.FindObjectOfType<ServerManager>().GameStarted = true;
    }
}
