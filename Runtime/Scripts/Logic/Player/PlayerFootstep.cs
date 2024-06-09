using System.Linq;
using UnityEngine;

namespace SMCharacterController
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerFootstep : MonoBehaviour
    {
        [SerializeField] private float walkFootstepLength = 0.7f;
        [SerializeField] private float runFootstepLength = 0.7f;
        [SerializeField] private float minFallingDistance = 2f;
        [SerializeField] private AudioSource footstepAS;
        [SerializeField] private FootstepMaterial[] materials;
        private Vector3 lastGroundPos;
        private bool isFalling;

        private PlayerMovement playerMovement;

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            lastGroundPos = transform.position;

            if (footstepAS == null)
                throw new System.Exception("PlayerFootstep:: footstepAS is not assigned");
        }

        private void Update()
        {
            if (!playerMovement.IsGrounded)
            {
                if (Vector3.Distance(transform.position, lastGroundPos) > minFallingDistance)
                    isFalling = true;

                return;
            }

            if (Vector3.Distance(transform.position, lastGroundPos) > GetFootstepLength() || isFalling)
            {
                lastGroundPos = transform.position;

                var groundMaterial = GetGroundMaterial();
                if (groundMaterial == null)
                    return;

                switch (playerMovement.MovementType)
                {
                    case MovementType.Idle:
                    case MovementType.Walk:
                        footstepAS.clip = groundMaterial.GetRandomWalkingClip();
                        break;
                    case MovementType.Run:
                        footstepAS.clip = groundMaterial.GetRandomRunningClip();
                        break;
                    default:
                        break;
                }

                if (isFalling)
                    footstepAS.clip = groundMaterial.GetRandomLandingClip();

                footstepAS.Play();
            }

            isFalling = false;
        }

        private float GetFootstepLength()
        {
            var length = 0f;

            switch (playerMovement.MovementType)
            {
                case MovementType.Idle:
                case MovementType.Walk:
                    length = walkFootstepLength;
                    break;
                case MovementType.Run:
                    length = runFootstepLength;
                    break;
                default:
                    break;
            }

            return length;
        }

        private FootstepMaterial GetGroundMaterial()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hit, Mathf.Infinity))
            {
                var om = hit.collider.gameObject.GetComponent<ObjectMaterial>();

                if (om != null)
                {
                    return materials.SingleOrDefault(x => x.IsMaterialMatched(om.Material));
                }
            }

            return null;
        }
    }
}