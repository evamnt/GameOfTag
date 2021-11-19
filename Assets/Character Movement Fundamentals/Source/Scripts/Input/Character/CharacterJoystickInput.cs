using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace CMF
{
	//This character movement input class is an example of how to get input from a gamepad/joystick to control the character;
	//It comes with a dead zone threshold setting to bypass any unwanted joystick "jitter";
	public class CharacterJoystickInput : CharacterInput {

		public string horizontalInputAxis = "Horizontal";
		public string verticalInputAxis = "Vertical";
		public KeyCode jumpKey = KeyCode.Joystick1Button0;

		//If this is enabled, Unity's internal input smoothing is bypassed;
		public bool useRawInput = true;

		//If any input falls below this value, it is set to '0';
        //Use this to prevent any unwanted small movements of the joysticks ("jitter");
		public float deadZoneThreshold = 0.2f;

		NetworkVariable<float> m_horizontalInput = new NetworkVariable<float>();
		NetworkVariable<float> m_verticalInput = new NetworkVariable<float>();
		NetworkVariable<bool> m_jumpInput = new NetworkVariable<bool>();

		public override float GetHorizontalMovementInput()
		{
			float _horizontalInput;

			if (IsLocalPlayer)
            {
				if (useRawInput)
					_horizontalInput = Input.GetAxisRaw(horizontalInputAxis);
				else
					_horizontalInput = Input.GetAxis(horizontalInputAxis);

				//Set any input values below threshold to '0';
				if (Mathf.Abs(_horizontalInput) < deadZoneThreshold)
					_horizontalInput = 0f;

				m_horizontalInput.Value = _horizontalInput;
			}
			else
            {
				_horizontalInput = m_horizontalInput.Value;
            }

			return _horizontalInput;
		}

		public override float GetVerticalMovementInput()
		{
			float _verticalInput;

			if (IsLocalPlayer)
            {
				if (useRawInput)
					_verticalInput = Input.GetAxisRaw(verticalInputAxis);
				else
					_verticalInput = Input.GetAxis(verticalInputAxis);

				//Set any input values below threshold to '0';
				if (Mathf.Abs(_verticalInput) < deadZoneThreshold)
					_verticalInput = 0f;

				m_verticalInput.Value = _verticalInput;
			}
			else
            {
				_verticalInput = m_verticalInput.Value;
            }

			return _verticalInput;
		}

		public override bool IsJumpKeyPressed()
		{
			if (IsLocalPlayer)
            {
				m_jumpInput.Value = Input.GetKey(jumpKey);
				return Input.GetKey(jumpKey);
			}
			else
            {
				return m_jumpInput.Value;
            }
		}

	}
}
