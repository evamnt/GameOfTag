using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PositionMaintainer : NetworkBehaviour
{
    NetworkVariable<Vector3> m_networkPosition = new NetworkVariable<Vector3>();
    NetworkVariable<Quaternion> m_networkRotation = new NetworkVariable<Quaternion>();

    public float m_maxDistance = 2;

    private void Start()
    {
        InvokeRepeating("UpdateTransform", 0, 1);
    }

    void UpdateTransform()
    {
        if (IsLocalPlayer)
        {
            UpdateRemoteTransformServerRpc(transform.position, transform.rotation);
        }
        else if (Vector3.Distance(transform.position, m_networkPosition.Value) > m_maxDistance) 
        { 
            transform.position = m_networkPosition.Value;
            transform.rotation = m_networkRotation.Value;
        }
    }

    [ServerRpc] void UpdateRemoteTransformServerRpc(Vector3 newPosition, Quaternion newRotation)
    {
        m_networkPosition.Value = newPosition;
        m_networkRotation.Value = newRotation;
    }
}
