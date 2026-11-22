using System;
using FMODUnity;
using UnityEngine;

namespace Game.Sounds
{
    [System.Serializable]
    public class FMODEventAmbience
    {
        [field: Header("Ambience")]
        [field: SerializeField] public EventReference Ambience { get; private set; }
        
        public EventReference GetEventReference(AmbienceAudioType type)
        {
            return type switch
            {
                AmbienceAudioType.Ambience => Ambience,
                _ => throw new InvalidOperationException("SoundType does not contain: " + type)
            };
        }
        
        
    }
}