using System;
using FMODUnity;
using UnityEngine;

namespace Game.Sounds
{
    [System.Serializable]
    public class FMODEventSounds
    {
        [field: Header("SFX")]
        [field: SerializeField] public EventReference GameOver { get; private set; }
        [field: SerializeField] public EventReference TakeOver { get; private set; }
        [field: SerializeField] public EventReference EatAllies { get; private set; }
        [field: SerializeField] public EventReference Portal { get; private set; }
        [field: SerializeField] public EventReference DoorOpen { get; private set; }
        [field: SerializeField] public EventReference Menu_Cancel { get; private set; }
        [field: SerializeField] public EventReference Menu_Okay { get; private set; }
        [field: SerializeField] public EventReference Menu_Select { get; private set; }
        [field: SerializeField] public EventReference Menu_StartPlaying { get; private set; }
        
        public EventReference GetEventReference(SFXType type)
        {
            return type switch
            {
                SFXType.GameOver => GameOver,
                SFXType.TakeOver => TakeOver,
                SFXType.EatAllies => EatAllies,
                SFXType.Portal => Portal,
                SFXType.DoorOpen => DoorOpen,
                SFXType.Menu_Cancel => Menu_Cancel,
                SFXType.Menu_Okay => Menu_Okay,
                SFXType.Menu_Select => Menu_Select,
                SFXType.Menu_StartPlaying => Menu_StartPlaying,
                _ => throw new InvalidOperationException("SoundType does not contain: " + type)
            };
        }
    }
}