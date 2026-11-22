using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Entity
{
    [CreateAssetMenu(fileName = nameof(EntityStatsConfiguration), menuName = Game.Constants.Project.AssetMenuStart + "Entity/" + nameof(EntityStatsConfiguration))]
    public class EntityStatsConfiguration : ScriptableObject
    {
        [EnumToggleButtons] public EntityContextType entityContextType;

        [field: SerializeField] 
        public EntityStatContainer EntityStatContainer { get; private set; }

        [field: SerializeField, ShowIf(nameof(entityContextType), EntityContextType.Player)]
        public PlayerValueContainer PlayerValueContainer { get; private set; }

        [field: SerializeField, ShowIf(nameof(entityContextType), EntityContextType.Enemy)]
        public EnemyValueContainer EnemyValueContainer { get; private set; }

        [field: SerializeField, ShowIf(nameof(entityContextType), EntityContextType.Ally)]
        public AllyValueContainer AllyValueContainer { get; private set; }

        
    }
}

