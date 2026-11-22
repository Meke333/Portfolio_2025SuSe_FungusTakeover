using UnityEngine;

namespace Game.Sounds
{
    [CreateAssetMenu(fileName = nameof(FMODEventsConfiguration), menuName = Game.Constants.Project.AssetMenuStart + nameof(FMODEventsConfiguration))]
    public class FMODEventsConfiguration : ScriptableObject
    {
        [field: SerializeField] public FMODEventMusic Music { get; private set; }
        [field: SerializeField] public FMODEventSounds Sounds { get; private set; }
        [field: SerializeField] public FMODEventAmbience Ambience { get; private set; }
     
    }
}

