using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Environment
{
    [System.Serializable]
    public struct EnvironmentEffect
    {
        [EnumButtons] public EnvironmentEffectType environmentType;


        [field: SerializeField, ShowIf(nameof(showPush)), PropertyTooltip("If this is set to 0,0,0 it instead takes the foward direction of the object")] public Vector3 PushDirection;
        [field: SerializeField, ShowIf(nameof(showPush))] public float PushForce { get; private set; }
        [field: SerializeField, ShowIf(nameof(showSpeed))] public float SpeedModifier { get; private set; }
        [field: SerializeField, ShowIf(nameof(showPoison))] public float PoisonDamage { get; private set; }

        private bool showPush => environmentType.HasFlag(EnvironmentEffectType.Pushing);
        private bool showSpeed => environmentType.HasFlag(EnvironmentEffectType.Speed);
        private bool showPoison => environmentType.HasFlag(EnvironmentEffectType.Poison);
    }
}

