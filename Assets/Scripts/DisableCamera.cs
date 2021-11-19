using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DisableCamera : NetworkBehaviour
{
    private void Start()
    {
        if (!IsLocalPlayer)
        {
            GetComponentInChildren<Camera>().enabled = false;
        }
    }
}
