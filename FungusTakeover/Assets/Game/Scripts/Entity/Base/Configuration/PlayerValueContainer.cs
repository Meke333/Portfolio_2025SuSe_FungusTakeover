using UnityEngine;
using Sirenix.OdinInspector;

namespace Game.Entity
{
    [System.Serializable]
    public class PlayerValueContainer
    {
        [Tooltip("CURRENTLY UNUSED! NO SHOOTABLE SPORES")] public float sporeSpeed;
        [Tooltip("Currently used for the 'collider size' so to speak")] public float sporeRange;
        [MinValue(0f)] public float formationTime;
        [EnumToggleButtons] public PlayerActions playerActions;
    }
}

