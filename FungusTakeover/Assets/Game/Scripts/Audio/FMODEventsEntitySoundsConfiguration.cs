using UnityEngine;

namespace Game.Sounds
{
    [CreateAssetMenu(fileName = nameof(FMODEventsEntitySoundsConfiguration), menuName = Game.Constants.Project.AssetMenuStart + nameof(FMODEventsEntitySoundsConfiguration))]
    public class FMODEventsEntitySoundsConfiguration : ScriptableObject
    {
        [field: SerializeField] public FMODEventsEntitySounds Sounds { get; private set; }
    }
}

