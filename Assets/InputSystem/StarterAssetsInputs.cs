using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool sprint;
		public bool pressedThrow;
		public bool pressedTreat;

		[Header("Movement Settings")]
		public bool analogMovement;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnThrow(InputValue value)
        {
			pressedThrow = value.isPressed;
        }

		public void OnTreat(InputValue value)
		{
			pressedTreat = value.isPressed;
		}
#endif

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void ThrowInput(bool newThrowState)
		{
			pressedThrow = newThrowState;
		}

		public void TreatInput(bool newTreatState)
		{
			pressedTreat = newTreatState;
		}
	}
}