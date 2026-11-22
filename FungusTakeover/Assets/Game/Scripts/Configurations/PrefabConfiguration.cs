using UnityEngine;

namespace Game.Configuration
{
    [CreateAssetMenu(fileName = nameof(PrefabConfiguration), menuName = Game.Constants.Project.AssetMenuStart + nameof(PrefabConfiguration))]
    public class PrefabConfiguration : ScriptableObject
    {
        [field: SerializeField] public GameObject SporePrefab { get; private set; }
    }
}

