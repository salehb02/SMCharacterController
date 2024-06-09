using System.Collections;
using UnityEngine;

namespace SMCharacterController
{
    [RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Gravity")]
        private const float GRAVITY = -11.5f;
        private float targetGravity;
        private float finalGravity;

        [Header("Movement")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private bool canRun;
        [SerializeField] private float runSpeed = 10f;
        private float finalSpeed;
        private Vector3 lastDirection;

        [Header("Jump")]
        [SerializeField] private bool canJump;
        [SerializeField] private AnimationCurve jumpGravity;
        [SerializeField] private float jumpSpeed = 1;
        private bool canJumpAgain;
        private bool isJumped;

        public MovementType MovementType { get; private set; }
        public bool IsGrounded { get; private set; }

        private CharacterController controller;
        private PlayerInput input;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<PlayerInput>();

            finalSpeed = walkSpeed;
            canJumpAgain = canJump;
        }

        private void Update()
        {
            finalSpeed = Mathf.Lerp(finalSpeed, GetSpeed(), Time.deltaTime * 5f);

            if (IsGrounded)
                lastDirection = transform.rotation * new Vector3(input.MoveDirection.x, 0, input.MoveDirection.y) * Time.deltaTime * finalSpeed;

            if (!isJumped)
                targetGravity = GRAVITY;

            finalGravity = Mathf.Lerp(finalGravity, targetGravity, Time.deltaTime * 5f);

            var gravity = Vector3.up * finalGravity * Time.deltaTime;

            controller.Move(lastDirection + gravity);

            IsGrounded = controller.isGrounded;

            Jump();
        }

        private void Jump()
        {
            if (!IsGrounded)
                return;

            if (!input.IsJumping)
            {
                canJumpAgain = true;
                return;
            }

            if (!canJumpAgain)
                return;

            if (isJumped)
                return;

            StartCoroutine(JumpCoroutine());
        }

        private IEnumerator JumpCoroutine()
        {
            var progress = 0f;
            isJumped = true;
            canJumpAgain = false;

            while (progress < 1)
            {
                targetGravity = jumpGravity.Evaluate(progress);
                progress += Time.deltaTime * jumpSpeed;
                yield return null;
            }

            isJumped = false;
        }

        private float GetSpeed()
        {
            MovementType = MovementType.Walk;
            var speed = walkSpeed;

            if (canRun && input.IsRunning)
            {
                MovementType = MovementType.Run;
                speed = runSpeed;
            }

            if (input.MoveDirection.magnitude == 0)
                MovementType = MovementType.Idle;

            return speed;
        }
    }
}