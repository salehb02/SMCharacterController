using UnityEngine;
using UnityEngine.InputSystem;

namespace SMCharacterController
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 MoveDirection { get; private set; }
        public Vector2 LookDelta { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsJumping { get; private set; }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void OnMove(InputValue context)
        {
            MoveDirection = context.Get<Vector2>();
        }

        public void OnLook(InputValue context)
        {
            LookDelta = context.Get<Vector2>();
        }

        public void OnRun(InputValue context)
        {
            IsRunning = context.isPressed;
        }

        public void OnJump(InputValue context)
        {
            IsJumping = context.isPressed;
        }
    }
}