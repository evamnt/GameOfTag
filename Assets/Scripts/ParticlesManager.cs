using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ParticlesManager : NetworkBehaviour
{
	//Particles for abilites
	public GameObject partDash;
	public GameObject partHaste;
	public GameObject partHasteCD;

	public void SetPartDash(bool enabled)
	{
		if (IsHost)
		{
			SetPartDashClientRpc(enabled);
		}
		else
		{
			SetPartDashServerRpc(enabled);
		}
	}

	[ClientRpc]
	void SetPartDashClientRpc(bool enabled)
	{
		partDash.SetActive(enabled);
	}

	[ServerRpc]
	void SetPartDashServerRpc(bool enabled)
	{
		SetPartDashClientRpc(enabled);
	}

	public void SetPartHaste(bool enabled)
	{
		if (IsHost)
		{
			SetPartHasteClientRpc(enabled);
		}
		else
		{
			SetPartHasteServerRpc(enabled);
		}
	}

	[ClientRpc]
	void SetPartHasteClientRpc(bool enabled)
	{
		partHaste.SetActive(enabled);
	}

	[ServerRpc]
	void SetPartHasteServerRpc(bool enabled)
	{
		SetPartHasteClientRpc(enabled);
	}

	public void SetPartHasteCD(bool enabled)
	{
		if (IsHost)
		{
			SetPartHasteCDClientRpc(enabled);
		}
		else
		{
			SetPartHasteCDServerRpc(enabled);
		}
	}

	[ClientRpc]
	void SetPartHasteCDClientRpc(bool enabled)
	{
		partHasteCD.SetActive(enabled);
	}

	[ServerRpc]
	void SetPartHasteCDServerRpc(bool enabled)
	{
		SetPartHasteCDClientRpc(enabled);
	}
}
