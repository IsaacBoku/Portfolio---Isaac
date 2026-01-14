using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		InputAction m_Interaction;
		InputAction m_EscapeUI;
		InputAction m_ScrollWeel;


        private void Start()
        {
            m_Interaction = InputSystem.actions.FindAction("Player/Interaction");
			m_EscapeUI = InputSystem.actions.FindAction("Player/Escape");
			m_ScrollWeel = InputSystem.actions.FindAction("Player/ScrollWheel");

            m_Interaction.Enable();
			m_EscapeUI.Enable();
			m_ScrollWeel.Enable();
        }
        public bool CanProcessInput()
        {
            return Cursor.lockState == CursorLockMode.Locked;
        }


        public bool GetInteraction()
        {
            if (CanProcessInput())
            {
                return m_Interaction.WasPressedThisFrame();
            }

            return false;
        }

		public bool GetEscapeUI()
		{
			if (m_EscapeUI.WasPressedThisFrame())
			{
				return true;
			}
			return false;
        }

		public bool GetScrollWeel(out float scrollValue)
		{
            scrollValue = 0f;
            if (CanProcessInput())
            {
                // CAMBIO: Leemos Vector2 y usamos la 'y'
                Vector2 scrollVector = m_ScrollWeel.ReadValue<Vector2>();
                scrollValue = scrollVector.y;

                if (scrollValue != 0f)
                {
                    return true;
                }
            }
            return false;
        }


#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}