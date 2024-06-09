using UnityEngine;

namespace SMCharacterController
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField] private Transform headPivot;
        [SerializeField] private float mouseSensitivity = 1f;
        [SerializeField] private Vector2 minMaxVerticalLookRotation;

        private PlayerInput input;

        private void Start()
        {
            input = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            transform.Rotate(Vector3.up * input.LookDelta.x * Time.deltaTime * mouseSensitivity);

            var newRot = headPivot.transform.localRotation * Quaternion.Euler(-input.LookDelta.y * Time.deltaTime * mouseSensitivity, 0, 0);
            newRot.eulerAngles = new Vector3(ClampAngle(newRot.eulerAngles.x, minMaxVerticalLookRotation.x, minMaxVerticalLookRotation.y), 0, 0);

            headPivot.transform.localRotation = newRot;
        }

        private float ClampAngle(float angle, float from, float to)
        {
            if (angle < 0f) angle = 360 + angle;
            if (angle > 180f) return Mathf.Max(angle, 360 + from);
            return Mathf.Min(angle, to);
        }
    }
}