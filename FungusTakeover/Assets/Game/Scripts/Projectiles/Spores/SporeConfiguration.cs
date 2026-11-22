using UnityEngine;


namespace Game.Projectiles
{
    //To-Do: Put into 
    [CreateAssetMenu(fileName = nameof(SporeConfiguration), menuName = Game.Constants.Project.AssetMenuStart + nameof(SporeConfiguration))]
    public class SporeConfiguration : ScriptableObject
    {
        public SporeStats stats;
    }
}

