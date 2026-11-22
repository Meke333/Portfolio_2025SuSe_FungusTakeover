using System;
using Events;
using FMOD.Studio;
using Game.GameFlow;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Sounds
{
    public class AmbienceAudioHandler : MonoBehaviour
    {
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private SoundManager _soundManager;
        [Inject] private FMODEventsConfiguration _eventsConfiguration;

        private SoundInstanceHandler _ambience;

        private SoundInstanceHandler _currentHandler;
        
        private GameFlowStateType _currentMusicGameFlowType;


        #region Unity Methods
        private void Start()
        {
            SpawnAllInstances();
            PlayAudio(AmbienceAudioType.Ambience);
            SetLevelParameter(Random.Range(0,3));
        }


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
                    _projectEvents.GameFlow.GameFlowChange += GameFlowChange;
                    _projectEvents.Dungeon.MovedIntoNextRoom += (x) => { SetLevelParameter(Random.Range(0, 3)); };
                    break;
                case false:
                    _projectEvents.GameFlow.GameFlowChange -= GameFlowChange;
                    _projectEvents.Dungeon.MovedIntoNextRoom -= (x) => { SetLevelParameter(Random.Range(0, 3)); };
                    break;
            }
        }

        private void SpawnAllInstances()
        {
            _ambience = _soundManager.FactorySoundAudioHandler.CreateEventInstance(
                _eventsConfiguration.Ambience.GetEventReference(AmbienceAudioType.Ambience));
        }
        
        private void GameFlowChange(GameFlowStateType type)
        {
            if (_currentMusicGameFlowType != 0 && type == _currentMusicGameFlowType)
                return;
            switch (type)
            {
                case GameFlowStateType.MainMenu:
                    StopAudio();
                    break;
                case GameFlowStateType.Gameplay:
                    PlayAudio(AmbienceAudioType.Ambience);
                    break;
                default:
                    return;
            }
            _currentMusicGameFlowType = type;
        }
        
        private void PlayAudio(AmbienceAudioType type, bool immediately = false)
        {
            StopAudio(immediately);
            
            _currentHandler = GetSoundInstance(type);
            _currentHandler.Play();
        }

        private void StopAudio(bool immediately = false)
        {
            if (_currentHandler != null)
                _currentHandler.Stop(immediately ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT);
        }
        
        private SoundInstanceHandler GetSoundInstance(AmbienceAudioType type)
        {
            return type switch
            {
                AmbienceAudioType.Ambience => _ambience,
                _ => throw new InvalidOperationException("MusicType does not contain: " + type)
            };       
        }

        private void SetLevelParameter(int level)
        {
            SetParameter("Level", level);
        }
        
        private void SetParameter(string parameterName, float value)
        {
            _currentHandler.SetParameters(parameterName, value);
        }
    }
}