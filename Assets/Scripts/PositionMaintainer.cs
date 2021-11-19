using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PositionMaintainer : NetworkBehaviour
{
    NetworkVariable<Vector3> m_networkPosition = new NetworkVariable<Vector3>();
    NetworkVariable<Quaternion> m_networkRotation = new NetworkVariable<Quaternion>();

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
        else
        {
            transform.position = m_networkPosition.Value;
            transform.rotation = m_networkRotation.Value;
        }
    }

    [ServerRpc] void UpdateRemoteTransformServerRpc(Vector3 newPosition, Quaternion newRotation)
    {
        m_networkPosition.Value = transform.position;
        m_networkRotation.Value = transform.rotation;
    }
}
