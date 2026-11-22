using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Entity
{
    [CreateAssetMenu(fileName = nameof(EntityAllStatsConfiguration), menuName = Game.Constants.Project.AssetMenuStart + "Entity/" + nameof(EntityAllStatsConfiguration))]
    public class EntityAllStatsConfiguration : ScriptableObject
    {
        [field: Title("Stats")]
        [field: SerializeField] public EntityStatsConfiguration Player { get; private set; }
        [field: SerializeField] public EntityStatsConfiguration Ally { get; private set; }
        [field: SerializeField] public EntityStatsConfiguration Enemy { get; private set; }

        public EntityStatsConfiguration GetStats(EntityContextType type)
        {
            EntityStatsConfiguration result = type switch
            {
                EntityContextType.Player => Player,
                EntityContextType.Ally => Ally,
                EntityContextType.Enemy => Enemy,
            };
            
            return result;
        }
    }
}

