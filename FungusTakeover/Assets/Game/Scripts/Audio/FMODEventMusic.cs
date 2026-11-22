using System;
using FMODUnity;
using UnityEngine;

namespace Game.Sounds
{
    [System.Serializable]
    public class FMODEventMusic
    {
        [field: Header("Music")]
        [field: SerializeField] public EventReference Menu { get; private set; }
        [field: SerializeField] public EventReference Gameplay_0 { get; private set; }
        [field: SerializeField] public EventReference Gameplay_1 { get; private set; }
        [field: SerializeField] public EventReference Gameplay_2 { get; private set; }
        [field: SerializeField] public EventReference Gameplay_3 { get; private set; }
        
        public EventReference GetEventReference(MusicType type)
        {
            return type switch
            {
                MusicType.Menu => Menu,
                MusicType.Gameplay0 => Gameplay_0,
                MusicType.Gameplay1 => Gameplay_1,
                MusicType.Gameplay2 => Gameplay_2,
                MusicType.Gameplay3 => Gameplay_3,
                _ => throw new InvalidOperationException("SoundType does not contain: " + type),
            };
        }
    }
}