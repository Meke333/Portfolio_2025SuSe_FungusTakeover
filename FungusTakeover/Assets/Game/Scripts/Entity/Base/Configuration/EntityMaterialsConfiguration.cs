using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Entity
{
    [CreateAssetMenu(fileName = nameof(EntityMaterialsConfiguration), menuName = Game.Constants.Project.AssetMenuStart + "Entity/" + nameof(EntityMaterialsConfiguration))]
    public class EntityMaterialsConfiguration : ScriptableObject
    {
        [AssetsOnly, AssetSelector] public Material Normal;
        [AssetsOnly, AssetSelector] public Material Enemy;
        [AssetsOnly, AssetSelector] public Material Ally;  
        
        [AssetsOnly, AssetSelector] public Material Hurt;
        [AssetsOnly, AssetSelector] public Material Attack;
        [AssetsOnly, AssetSelector] public Material Dead;
    }
}

