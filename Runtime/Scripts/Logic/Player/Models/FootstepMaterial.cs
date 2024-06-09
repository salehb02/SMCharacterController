using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SMCharacterController
{
    [Serializable]
    public class FootstepMaterial
    {
        [SerializeField] private string materialName;

        [Header("Clips")]
        [SerializeField] private AudioClip[] walking;
        [SerializeField] private AudioClip[] running;
        [SerializeField] private AudioClip[] landing;

        public bool IsMaterialMatched(string name)
        {
            return string.Equals(materialName.ToLower(), name.ToLower());
        }

        public AudioClip GetRandomWalkingClip()
        {
            if (walking.Length == 0)
                return null;

            return walking[Random.Range(0, walking.Length)];
        }
        public AudioClip GetRandomRunningClip()
        {
            if (running.Length == 0)
                return null;

            return running[Random.Range(0, running.Length)];
        }

        public AudioClip GetRandomLandingClip()
        {
            if (landing.Length == 0)
                return null;

            return landing[Random.Range(0, landing.Length)];
        }
    }
}