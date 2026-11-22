using UnityEngine;

namespace Game.Environment
{
    [CreateAssetMenu(fileName = nameof(EnvironmentEffectConfiguration), menuName = Game.Constants.Project.AssetMenuStart + nameof(EnvironmentEffectConfiguration))] 
    public class EnvironmentEffectConfiguration : ScriptableObject
    {
        [field: SerializeField] public EnvironmentEffect Effect { get; private set; }
    }
}

