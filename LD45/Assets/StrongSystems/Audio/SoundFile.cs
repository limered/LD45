using System;
using UnityEngine;

namespace StrongSystems.Audio
{
    [Serializable]
    public class SoundFile
    {
        public string Name;
        public AudioClip[] Files;

        [Range(0f, 1f)]
        public float Volume;
    }
}