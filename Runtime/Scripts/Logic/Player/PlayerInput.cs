using UnityEngine;
using UnityEngine.InputSystem;

namespace SMCharacterController
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Run { get; private set; }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void OnMove(InputValue context)
        {
            Move = context.Get<Vector2>();
        }

        public void OnLook(InputValue context)
        {
            Look = context.Get<Vector2>();
        }

        public void OnRun(InputValue context)
        {
            Run = context.isPressed;
        }
    }
}