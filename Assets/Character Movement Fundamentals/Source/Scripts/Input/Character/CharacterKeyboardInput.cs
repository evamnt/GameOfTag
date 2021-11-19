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

		NetworkVariable<bool> m_jumpInput = new NetworkVariable<bool>();

		//If this is enabled, Unity's internal input smoothing is bypassed;
		public bool useRawInput = true;

        public override float GetHorizontalMovementInput()
		{
			if (useRawInput)
			{
				return Input.GetAxisRaw(horizontalInputAxis);
			}
			else
			{
				return Input.GetAxis(horizontalInputAxis);
			}
		}

		public override float GetVerticalMovementInput()
		{
			if (useRawInput)
			{
				return Input.GetAxisRaw(verticalInputAxis);
			}
			else
			{
				return Input.GetAxis(verticalInputAxis);
			}
		}

		public override bool IsJumpKeyPressed()
		{
			if (IsLocalPlayer)
            {
				bool jumpInput = Input.GetKey(jumpKey);
				RequestJumpInputChangeServerRpc(jumpInput);
				return jumpInput;
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
