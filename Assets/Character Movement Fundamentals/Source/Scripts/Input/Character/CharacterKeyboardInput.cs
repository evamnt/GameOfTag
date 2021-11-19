using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace CMF
{
	//This character movement input class is an example of how to get input from a keyboard to control the character;
    public class CharacterKeyboardInput : CharacterInput
	{
		public string horizontalInputAxis = "Horizontal";
		public string verticalInputAxis = "Vertical";
		public KeyCode jumpKey = KeyCode.Space;

		NetworkVariable<float> m_horizontalInput = new NetworkVariable<float>();
		NetworkVariable<float> m_verticalInput = new NetworkVariable<float>();
		NetworkVariable<bool> m_jumpInput = new NetworkVariable<bool>();

		//If this is enabled, Unity's internal input smoothing is bypassed;
		public bool useRawInput = true;

        public override float GetHorizontalMovementInput()
		{
			if (IsLocalPlayer)
            {
				if (IsHost)
                {
					if (useRawInput)
					{
						m_horizontalInput.Value = Input.GetAxisRaw(horizontalInputAxis);
						return Input.GetAxisRaw(horizontalInputAxis);
					}
					else
					{
						m_horizontalInput.Value = Input.GetAxis(horizontalInputAxis);
						return Input.GetAxis(horizontalInputAxis);
					}
                }
				else
                {
					if (useRawInput)
					{
						float horizontalInput = Input.GetAxisRaw(horizontalInputAxis);
						RequestHorizontalInputChangeServerRpc(horizontalInput);
						return horizontalInput;
					}
					else
					{
						float horizontalInput = Input.GetAxis(horizontalInputAxis);
						RequestHorizontalInputChangeServerRpc(horizontalInput);
						return horizontalInput;
					}
				}
			}
			else
            {
				return m_horizontalInput.Value;
            }
		}

		[ServerRpc]
		private void RequestHorizontalInputChangeServerRpc(float newHorizontalInput)
        {
			m_horizontalInput.Value = newHorizontalInput;
        }

		public override float GetVerticalMovementInput()
		{
			if (IsLocalPlayer)
            {
				if (IsHost)
                {
					if (useRawInput)
					{
						m_verticalInput.Value = Input.GetAxisRaw(verticalInputAxis);
						return Input.GetAxisRaw(verticalInputAxis);
					}
					else
					{
						m_verticalInput.Value = Input.GetAxis(verticalInputAxis);
						return Input.GetAxis(verticalInputAxis);
					}
                }
				else
                {
					if (useRawInput)
					{
						float verticalInput = Input.GetAxisRaw(verticalInputAxis);
						RequestVerticalInputChangeServerRpc(verticalInput);
						return verticalInput;
					}
					else
					{
						float verticalInput = Input.GetAxis(verticalInputAxis);
						RequestVerticalInputChangeServerRpc(verticalInput);
						return verticalInput;
					}
				}
			}
			else
            {
				return m_verticalInput.Value;
            }
		}

		[ServerRpc]
		private void RequestVerticalInputChangeServerRpc(float newVerticalInput)
		{
			m_verticalInput.Value = newVerticalInput;
		}

		public override bool IsJumpKeyPressed()
		{
			if (IsLocalPlayer)
            {
				if (IsHost)
                {
					m_jumpInput.Value = Input.GetKey(jumpKey);
					return Input.GetKey(jumpKey);
                }
				else
                {
					bool jumpInput = Input.GetKey(jumpKey);
					RequestJumpInputChangeServerRpc(jumpInput);
					return jumpInput;
                }
			}
			else
            {
				return m_jumpInput.Value;
            }
		}

		[ServerRpc]
		private void RequestJumpInputChangeServerRpc(bool newJumpInput)
		{
			m_jumpInput.Value = newJumpInput;
		}
	}
}
