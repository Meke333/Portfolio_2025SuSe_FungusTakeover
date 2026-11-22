using Events;
using FMODUnity;
using Game.Entity;
using UnityEngine;
using Zenject;

namespace Game.Sounds
{
    public class CameraListenerHandler : MonoBehaviour
    {
        [Inject] private ProjectEvents _projectEvents;
        [SerializeField] public StudioListener _studioListener;

        #region UnityMethod

        
        private void OnEnable()
        {
            ToggleEvents(true);
        }

        private void OnDisable()
        {
            ToggleEvents(false);
        }

        #endregion

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _projectEvents.Entity.OnPlayerChanged += UpdateCurrentPlayer;
                    break;
                case false:
                    _projectEvents.Entity.OnPlayerChanged -= UpdateCurrentPlayer;
                    break;
            }
        }
        
        private void UpdateCurrentPlayer(EntityBase player)
        {
            _studioListener.AttenuationObject = player.gameObject;
        }
    }
}