using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    private float m_catTime;
    private bool m_isCat;

    private void Start()
    {
        m_catTime = 0;
        m_isCat = false;
    }

    public bool IsCat()
    {
        return m_isCat;
    }

    public float GetCatTime()
    {
        return m_catTime;
    }

    public void SetCat()
    {
        m_isCat = true;
        InvokeRepeating("IncreaseTimerByOneSecond", 1, 1);
    }

    public void SetNotCat()
    {
        m_isCat = false;
        CancelInvoke();
    }

    private void IncreaseTimerByOneSecond()
    {
        m_catTime += 1;
    }
}
