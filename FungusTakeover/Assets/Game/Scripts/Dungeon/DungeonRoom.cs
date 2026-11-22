using Events;
using UnityEngine;
using Zenject;


namespace Game.Dungeon
{

    //This depends on how we will do it, but for MVP we only need one room
    public class DungeonRoom : MonoBehaviour
    {

        [Inject] private ProjectEvents _projectEvents;

        [SerializeField] private DungeonTeleporter[] _teleporters;
        [field: SerializeField] public Transform StartPoint { get; private set; }

        #region UnityMethods

        void OnEnable()
        {
            ToggleEvents(true);
        }

        void OnDisable()
        {
            ToggleEvents(false);
        }
        #endregion

        void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    //_projectEvents.Entity.CurrentRoomCleared += EnemyWaveCleared;
                    break;
                case false:
                    //_projectEvents.Entity.CurrentRoomCleared -= EnemyWaveCleared;
                    break;
            }
        }

        void EnemyWaveCleared()
        {
            OpenDoor();
        }

        public void OpenDoor()
        {
            //To-Do: Create a field for the Teleporter and activate it here
            
            for (int i = 0; i < _teleporters.Length; i++)
            {
                _teleporters[i].OpenDoor();
            }
        }


    }
}

