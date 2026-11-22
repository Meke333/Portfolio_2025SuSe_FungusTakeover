using System;
using Events;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class HealthBarCallIcon : GameUIElement
    {
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private GameUIHolder _gameUIHolder;
        
        [SerializeField] private MMProgressBar _playerHealthBar;
        [SerializeField] private MMProgressBar _allyHealthBar;
        [SerializeField] private MMF_Player _callAlliesFeedback;
        [SerializeField] private MMFeedbacks _newAllyPlayerFeedback;

        private bool _isMC = true;
        
        #region UnityMethods

        private void Awake()
        {
            _gameUIHolder.healthBarCallIcon = this;
        }

        private void OnEnable()
        {
            ToggleEvent(true);
        }

        private void OnDisable()
        {
            ToggleEvent(false);
        }

        #endregion

        public void ToggleEvent(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    break;
                case false:
                    break;
            }
        }

        public void UpdateHealthBars(float value, float min, float max, bool show = false)
        {
            if (_isMC)
                _playerHealthBar?.UpdateBar(value, min, max);
            else
                _allyHealthBar?.UpdateBar(value, min, max);
        }
        
        public void SwitchOutPlayer(bool isMC)
        {
            _playerHealthBar?.gameObject.SetActive(isMC);
            _allyHealthBar?.gameObject.SetActive(!isMC);
            _callAlliesFeedback?.gameObject.SetActive(isMC);
            _newAllyPlayerFeedback?.PlayFeedbacks();

            _isMC = isMC;
        }

        public void TriggerAllyCalling()
        {
            if (_isMC)
                _callAlliesFeedback.PlayFeedbacks();
        }
        
    }
}