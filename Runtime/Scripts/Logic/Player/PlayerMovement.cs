using UnityEngine;

namespace SMCharacterController
{
    [RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        private const float GRAVITY = -9.8f;

        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private bool canRun;
        [SerializeField] private float runSpeed = 10f;
        private float finalSpeed;

        public MovementType MovementType { get; private set; }
        public bool IsGrounded { get; private set; }

        private CharacterController controller;
        private PlayerInput input;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<PlayerInput>();

            finalSpeed = walkSpeed;
        }

        private void Update()
        {
            finalSpeed = Mathf.Lerp(finalSpeed, GetSpeed(), Time.deltaTime * 5f);

            var direction = transform.rotation * new Vector3(input.Move.x, 0, input.Move.y) * Time.deltaTime * finalSpeed;
            var gravity = Vector3.up * GRAVITY * Time.deltaTime;

            controller.Move(direction + gravity);

            IsGrounded = controller.isGrounded;
        }

        private float GetSpeed()
        {
            MovementType = MovementType.Walk;
            var speed = walkSpeed;

            if (canRun && input.Run)
            {
                MovementType = MovementType.Run;
                speed = runSpeed;
            }

            if (input.Move.magnitude == 0)
                MovementType = MovementType.Idle;

            return speed;
        }
    }
}