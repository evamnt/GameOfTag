using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CatTimer : NetworkBehaviour
{
    private bool m_isCat = false;
    private NetworkVariable<float> m_timer = new NetworkVariable<float>();
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
            InvokeRepeating("IncreaseTimer",0,1);
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
