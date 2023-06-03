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
        public bool interacting, climbing;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        //[SerializeField] Game.Player.Player owner;
        [SerializeField] UnityEngine.Events.UnityEvent InteractEvent;
        [SerializeField] UnityEngine.Events.UnityEvent ClimbEvent;


#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
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

        public void OnInteract(InputValue value)
        {
            if (value.isPressed && !interacting)
            {
                InteractEvent.Invoke();
                interacting = true;

            }
            if (!value.isPressed && interacting)
            {
                interacting = false;
            }
        }
        public void OnClimb(InputValue value)
        {
            if (value.isPressed && !climbing)
            {
                ClimbEvent.Invoke();
                climbing = true;
            }
            if (!value.isPressed && climbing)
            {
                climbing = false;
            }
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

        public void ClimbInput(bool newClimbState)
        {

            if (newClimbState && !climbing)
            {
                ClimbEvent.Invoke();
                climbing = true;
            }
            if (!newClimbState && climbing)
            {
                climbing = false;
            }
        }

        public void InteractInput(bool newInteractState)
        {
            if (newInteractState && !interacting)
            {
                InteractEvent.Invoke();
                interacting = true;

            }
            if (!newInteractState && interacting)
            {
                interacting = false;
            }
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