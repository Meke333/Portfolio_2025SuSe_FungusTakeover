using System;
using System.Collections.Generic;
using Events;
using FMOD.Studio;
using Game.Entity;
using Game.GameFlow;
using UnityEngine;
using Zenject;

namespace Game.Sounds
{
    public class MusicAudioHandler : MonoBehaviour
    {
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private GlobalProjectSettings _globalProjectSettings;
        [Inject] private SoundManager _soundManager;
        [Inject] private FMODEventsConfiguration _configuration;

        private SoundInstanceHandler _music_MainMenu;
        private SoundInstanceHandler _music_Gameplay_Level0;
        private SoundInstanceHandler _music_Gameplay_Level1;
        private SoundInstanceHandler _music_Gameplay_Level2;
        private SoundInstanceHandler _music_Gameplay_Level3;
        

        private SoundInstanceHandler _currentHandler;

        private GameFlowStateType _currentMusicGameFlowType;

        [SerializeField] private AnimationCurve _intensityCurve;
        [SerializeField] private AnimationCurve _amountAlliesCurve;
        
        private float _intensity;
        private float _amountAllies;
        private int _currentLevelIndex = 0;

        #region Unity Methods
        private void Start()
        {
            SpawnAllInstances();
            
            PlayAudio(MusicType.Menu);
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
                    _projectEvents.Entity.OnAllyListChanged += SetNumberOfAlliesParameter;
                    _projectEvents.Dungeon.WavePercentageChanged += SetIntensityParameter;
                    _projectEvents.Dungeon.CurrentRoom += PlayLevelMusic;
                    _projectEvents.Dungeon.OpenUpDoors += OpenDoor;
                    
                    break;
                case false:
                    _projectEvents.GameFlow.GameFlowChange -= GameFlowChange;
                    _projectEvents.Entity.OnAllyListChanged -= SetNumberOfAlliesParameter;
                    _projectEvents.Dungeon.WavePercentageChanged -= SetIntensityParameter;
                    _projectEvents.Dungeon.CurrentRoom -= PlayLevelMusic;
                    _projectEvents.Dungeon.OpenUpDoors -= OpenDoor;
                    break;
            }
        }


        private void SpawnAllInstances()
        {
            _music_MainMenu = _soundManager.FactorySoundAudioHandler.CreateEventInstance(_configuration.Music.GetEventReference(MusicType.Menu));
            _music_Gameplay_Level0 = _soundManager.FactorySoundAudioHandler.CreateEventInstance(_configuration.Music.GetEventReference(MusicType.Gameplay0));
            _music_Gameplay_Level1 = _soundManager.FactorySoundAudioHandler.CreateEventInstance(_configuration.Music.GetEventReference(MusicType.Gameplay1));
            _music_Gameplay_Level2 = _soundManager.FactorySoundAudioHandler.CreateEventInstance(_configuration.Music.GetEventReference(MusicType.Gameplay2));
            _music_Gameplay_Level3 = _soundManager.FactorySoundAudioHandler.CreateEventInstance(_configuration.Music.GetEventReference(MusicType.Gameplay3));
            
        }
        
        private void GameFlowChange(GameFlowStateType type)
        {
            if (type == _currentMusicGameFlowType)
                return;
            
            
            //SetIntensityParameter(0);
            //SetNumberOfAlliesParameter(0);
            
            MusicType play = 0;
            switch (type)
            {
                case GameFlowStateType.MainMenu:
                    _intensity = 0;
                    _amountAllies = 0;
                    play = MusicType.Menu;
                    break;
                case GameFlowStateType.Gameplay:
                    _currentLevelIndex = 0;
                    _intensity = 0;
                    _amountAllies = 0;
                    play = SwitchLevelType(_currentLevelIndex);
                    break;
                default:
                    return;
            }
            _currentMusicGameFlowType = type;
            
            PlayAudio(play, true);
            
        }
        private void OpenDoor()
        {
            StopAudio(false);
        }

        private void PlayLevelMusic(int levelIndex)
        {
            MusicType play = SwitchLevelType(levelIndex);
            Debug.Log("Music Change: " + play);
            
            if (EqualityComparer<MusicType>.Default.Equals(play, 0))
                return;
            
            PlayAudio(play);

            _intensity = 0;            
            SetIntensityParameter(_intensity);
            
            _currentLevelIndex = levelIndex;
        }
        
        private MusicType SwitchLevelType(int index)
        {
            return index switch
            {
                0 => MusicType.Gameplay0,
                1 => MusicType.Gameplay1,
                2 => MusicType.Gameplay2,
                3 => MusicType.Gameplay3,
                _ => 0
            };
            
        }
        
        private void PlayAudio(MusicType type, bool immediately = false)
        {
            SoundInstanceHandler temp = GetSoundInstance(type);
            if (!EqualityComparer<SoundInstanceHandler>.Default.Equals(_currentHandler, null))
            {
                if (_currentHandler.IsPlaying() 
                    && EqualityComparer<SoundInstanceHandler>.Default.Equals(temp, _currentHandler))
                    return;
            
                if (_currentHandler.IsPlaying())
                    StopAudio(immediately);

            }
            
            _currentHandler = temp;
            
            SetIntensityParameter(_intensity);
            SetNumberOfAlliesParameter(_amountAllies);
            
            _currentHandler.Play();
        }
        
        private void StopAudio(bool immediately = false)
        {
            if (EqualityComparer<SoundInstanceHandler>.Default.Equals(_currentHandler, null))
                return;
            
            if (immediately)
                _currentHandler.Stop(STOP_MODE.IMMEDIATE);
            else
                _currentHandler.FadeOut(2);
        }

        private SoundInstanceHandler GetSoundInstance(MusicType type)
        {
            return type switch
            {
                MusicType.Menu => _music_MainMenu,
                MusicType.Gameplay0 => _music_Gameplay_Level0,
                MusicType.Gameplay1 => _music_Gameplay_Level1,
                MusicType.Gameplay2 => _music_Gameplay_Level2,
                MusicType.Gameplay3 => _music_Gameplay_Level3,
                _ => throw new InvalidOperationException("MusicType does not contain: " + type)
            };       
        }

        private void SetNumberOfAlliesParameter(List<EntityBase> list)
        {
            int spores = -1;
            for (int i = 0; i < list.Count; i++)
            {
                spores += list[i].StatsConfiguration.Enemy.EntityStatContainer.sporeCost;
            }

            spores = Mathf.Max(0, spores);
            
            _amountAllies = Mathf.Lerp(0, 10, _amountAlliesCurve.Evaluate((float)spores / _globalProjectSettings.StartSpores));
            SetParameter("music_numberOfAllies", _amountAllies);
        }
        
        private void SetNumberOfAlliesParameter(float amount)
        {
            _amountAllies = amount;
            SetParameter("music_numberOfAllies", amount);
        }

        private void SetIntensityParameter(float value)
        {
            _intensity = Mathf.Lerp(1, 3, _intensityCurve.Evaluate(value));
            SetParameter("Intensity", _intensity);
        }

        private void SetParameter(string parameterName, float value)
        {
            _currentHandler?.SetParameters(parameterName, value);
        }
        
    }
}