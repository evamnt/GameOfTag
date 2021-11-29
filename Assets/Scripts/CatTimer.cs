using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class CatTimer : NetworkBehaviour
{
    public TMP_Text m_catTime;

    private bool m_isCat = false;
    private NetworkVariable<float> m_timer = new NetworkVariable<float>(NetworkVariableReadPermission.Everyone, 0);

    private void Update()
    {
        if (IsLocalPlayer)
            m_catTime.text = "Cat Timer: " + m_timer.Value.ToString();
    }

    private void Start()
    {
        m_catTime.text = "Cat Timer: 0";
    }

    public void SetPlayerStatus(bool cat)
    {
        m_isCat = cat;
        if (cat)
        {
            StartTimer();
        }
        else
        {
            StopTimer();
        }
    }
    public void StartTimer()
    {
        if (IsHost)
        {
            InvokeRepeating("IncreaseTimer", 0, 1);
        }
    }

    public void StopTimer()
    {
        if (IsHost)
        {
            CancelInvoke("IncreaseTimer");
        }
    }
    private void IncreaseTimer()
    {
        m_timer.Value += 1f;
    }
}
