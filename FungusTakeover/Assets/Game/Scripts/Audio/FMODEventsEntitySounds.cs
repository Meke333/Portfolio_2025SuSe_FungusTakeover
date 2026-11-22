using System;
using FMODUnity;
using UnityEngine;

namespace Game.Sounds
{
    [System.Serializable]
    public class FMODEventsEntitySounds
    {
        [field: SerializeField] public EventReference Attack { get; private set; }
        [field: SerializeField] public EventReference Damage { get; private set; }
        [field: SerializeField] public EventReference Death { get; private set; }
        [field: SerializeField] public EventReference Walking { get; private set; }
        [field: SerializeField] public EventReference CallAllies { get; private set; }
        [field: SerializeField] public EventReference EatAllies { get; private set; }
        [field: SerializeField] public EventReference SporeShoot { get; private set; }
        [field: SerializeField] public EventReference TakeOver { get; private set; }
       

        public EventReference GetEventReference(EntitySFXType type)
        {

            return type switch
            {
                EntitySFXType.Attack => Attack,
                EntitySFXType.Damage => Damage,
                EntitySFXType.Death => Death,
                EntitySFXType.Walking => Walking,
                EntitySFXType.CallAllies => CallAllies,
                EntitySFXType.EatAllies => EatAllies,
                EntitySFXType.SporeShoot => SporeShoot,
                EntitySFXType.TakeOver => TakeOver,
                _ => throw new InvalidOperationException("SoundType does not contain: " + type),
            };
        }
        
    }
}