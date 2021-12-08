using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Collections;

public class TotalTimer : NetworkBehaviour
{
    public TMP_Text m_totalTime;
    public GameObject winMenu;
    private NetworkVariable<float> m_timerMinute = new NetworkVariable<float>(NetworkVariableReadPermission.Everyone, 0);
    private NetworkVariable<float> m_timerSecond = new NetworkVariable<float>(NetworkVariableReadPermission.Everyone, 0);

    private bool isOver = false;

    private void Update()
    {
        if (IsLocalPlayer)
        {
            if (m_timerSecond.Value < 10f)
            {
                m_totalTime.text = "Total time: " + m_timerMinute.Value.ToString() + ":0" + m_timerSecond.Value.ToString();
            }
            else
            {
                m_totalTime.text = "Total time: " + m_timerMinute.Value.ToString() + ":" + m_timerSecond.Value.ToString();
            }
        }
        if (IsHost && m_timerMinute.Value == 0f && m_timerSecond.Value == 0f && !isOver)
        {

            isOver = true;
            StopTimer();
            GetComponent<CatTimer>().StopTimer();
            GameObject gameRules = GameObject.FindGameObjectWithTag("Gamerules");
            string winnerName = gameRules.GetComponent<Gamerules>().GetWinner().nickname;
            ShowWinnerClientRpc(winnerName);
        }
    }

    [ClientRpc]
    void ShowWinnerClientRpc(FixedString64Bytes winnerName)
    {
        GameObject.FindObjectOfType<ServerManager>().GameStarted = false;
        GameObject.FindObjectOfType<ServerManager>().GameFinished = true;
        Cursor.lockState = CursorLockMode.None;
        winMenu.GetComponentInChildren<TMP_Text>().text = "And the winner is: " + winnerName + " !";
        winMenu.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        GameObject.FindObjectOfType<CMF.DemoMenu>().RestartScene();
    }

    private void Start()
    {
        if (IsLocalPlayer)
        {
            m_totalTime.text = "Total time: 5:00";
            
        }
        else
        {
            m_totalTime.enabled = false;
        }
        if (IsHost)
        {
            m_timerMinute.Value = 0f;
            m_timerSecond.Value = 10f;
            StartTimer();
        }
            
    }

    public void StartTimer()
    {
        if (IsHost)
        {
            InvokeRepeating("DecreaseTimer", 0, 1);
        }
    }

    public void StopTimer()
    {
        if (IsHost)
        {
            CancelInvoke("DecreaseTimer");
        }
    }

    private void DecreaseTimer()
    {
        m_timerSecond.Value -= 1f;
        if(m_timerSecond.Value < 0f)
        {
            m_timerSecond.Value = 59f;
            m_timerMinute.Value -= 1f;
        }
    }
}
