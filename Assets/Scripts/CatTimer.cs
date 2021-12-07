using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class CatTimer : NetworkBehaviour
{
    public TMP_Text m_catTime;

    private NetworkVariable<bool> m_isCat = new NetworkVariable<bool>(NetworkVariableReadPermission.Everyone, false);
    private NetworkVariable<float> m_timer = new NetworkVariable<float>(NetworkVariableReadPermission.Everyone, 0);

    public bool IsCat
    {
        get { return m_isCat.Value; }
    }

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
        m_isCat.Value = cat;
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
