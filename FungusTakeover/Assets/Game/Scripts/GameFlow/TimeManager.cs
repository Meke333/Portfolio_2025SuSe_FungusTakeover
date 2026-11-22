using Events;
using NUnit.Framework.Interfaces;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Zenject;

namespace Game.GameFlow
{
    public class TimeManager : MonoBehaviour
    {
      [Inject] private ProjectEvents _events;

        [SerializeField, DisplayAsString] private float _timescale = 1;

        private enum TimeScaleOption
        {
            Immediate = 0,
            HitStun = 1,
            Gradual = 2,
        }

        private TimeScaleOption _currentOption;
        private float _setTimer;
        private float _currentTimer;

        [SerializeField] private AnimationCurve _gradualInterpolationCurve;
        private float _previousScale;
        private float _targetScale;

        #region UnityMethods
        void OnEnable()
        {
          ToggleEvents(true);
        }

        void OnDisable()
        {
            ToggleEvents(false);
        }
        void Update()
        {
            
        }

        #endregion



        private void SetTimeScale(float scale)
        {
            scale = Mathf.Clamp(scale, 0, float.MaxValue);

            _timescale = scale;
        }

        private void GradualChangingTimeScale(float target)
        {
            
        }

        private void SetHitStun(float duration)
        {
            
        }

        private void SetTimeBehaviour(TimeScaleOption option, float value = 0)
        {
            _currentOption = option;
            switch (option)
            {
                case TimeScaleOption.Immediate:
                    SetTimeScale(value);
                    break;
                case TimeScaleOption.HitStun:
                    SetHitStun(value);
                    break;
                case TimeScaleOption.Gradual:
                    GradualChangingTimeScale(value);
                    break;
            }
        }
        
        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.GameFlow.GameFlowChange += GameFlowChange;
                    break;
                case false:
                    _events.GameFlow.GameFlowChange -= GameFlowChange;

                    break;
            }
        }

        private void GameFlowChange(GameFlowStateType type)
        {
            switch(type)
            {
                case GameFlowStateType.Gameplay:
                    SetTimeBehaviour(TimeScaleOption.Immediate, 1);
                    break;
                case GameFlowStateType.PauseMenu:
                    SetTimeBehaviour(TimeScaleOption.Immediate, 0);
                    break;
            }
        }
    }

}


