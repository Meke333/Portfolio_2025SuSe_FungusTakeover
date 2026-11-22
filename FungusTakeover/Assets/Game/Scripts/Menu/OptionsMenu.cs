using System;
using Events;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;


namespace Game.Menu
{
    public abstract class OptionsMenu : MonoBehaviour
    {
        [Inject] protected ProjectEvents _projectEvents;

        [SerializeField] protected GameObject _optionsMenuGameObject;

        [SerializeField] protected Button _mainBackButton; //Must be set to different Events in specific cases!!


        [Header("ControllerRebind")]
        [SerializeField] private GameObject _controllerSettingsGameObject;
        [SerializeField] private GameObject _controllerBackgroundGameObject;
        [SerializeField] private GameObject _keyButtonSettingGameObject;
        [SerializeField] private Button _controllerRebindButton;

        [SerializeField] private GameObject _keyboardSettingsObject;
        [SerializeField] private GameObject _gamepadSettingsObject;

        [SerializeField] private Button _keyboardSettingsButton;
        [SerializeField] private Button _gamepadSettingsButton;
        [SerializeField] private Button _controlsKeyboardBackButton;
        [SerializeField] private Button _controlsGamepadBackButton;


        [Header("AudioSettings")]
        [SerializeField] private GameObject _audioSettingsGameObject;
        [SerializeField] private GameObject _audioBackgroundGameObject;
        [SerializeField] private Button _audioSettingsButton;
        
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;

        private Bus masterBus;
        private Bus musicBus;
        private Bus sfxBus;
        

        protected enum OptionsType
        {
            CONTROLLERREBIND = 1,
            AUDIOSETTINGS = 2
        }

        protected enum ControlType
        {
            NONE = 0, //Control Selection Screen
            KEYBOARD = 1,
            GAMEPAD = 2,
        }

        protected OptionsType _currentOption = OptionsType.CONTROLLERREBIND;
        protected ControlType _controlContext = ControlType.NONE;

        #region UnityMethods

        protected virtual void Awake()
        {
            _mainBackButton.onClick.AddListener(BackButtonPressed);
        }

        protected virtual void OnEnable()
        {
            SwitchOptions(OptionsType.CONTROLLERREBIND);
            ToggleEvents(true);
            SetUpFMOD();
            _mainBackButton.Select();
        }

        protected virtual void OnDisable()
        {
            ToggleEvents(false);
        }

        protected virtual void OnDestroy()
        {
            _mainBackButton.onClick.RemoveListener(BackButtonPressed);
        }


        #endregion

        protected virtual void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _controllerRebindButton.onClick.AddListener(() => { SwitchOptions(OptionsType.CONTROLLERREBIND); });
                    _audioSettingsButton.onClick.AddListener(() => { SwitchOptions(OptionsType.AUDIOSETTINGS); });

                    _keyboardSettingsButton.onClick.AddListener(() => GoToControlsRebind(ControlType.KEYBOARD));
                    _gamepadSettingsButton.onClick.AddListener(() => GoToControlsRebind(ControlType.GAMEPAD));
                    _controlsKeyboardBackButton.onClick.AddListener(GoBackFromRebinding);
                    _controlsGamepadBackButton.onClick.AddListener(GoBackFromRebinding);
                    
                    _masterVolumeSlider.onValueChanged.AddListener((x) => SetVolume(AudioSliderType.Master, x));
                    _musicVolumeSlider.onValueChanged.AddListener((x) => SetVolume(AudioSliderType.Music, x));
                    _sfxVolumeSlider.onValueChanged.AddListener((x) => SetVolume(AudioSliderType.SFX, x));
                    
                    break;
                case false:
                    _controllerRebindButton.onClick.RemoveListener(() => { SwitchOptions(OptionsType.CONTROLLERREBIND); });
                    _audioSettingsButton.onClick.RemoveListener(() => { SwitchOptions(OptionsType.AUDIOSETTINGS); });

                    _keyboardSettingsButton.onClick.RemoveListener(() => GoToControlsRebind(ControlType.KEYBOARD));
                    _gamepadSettingsButton.onClick.RemoveListener(() => GoToControlsRebind(ControlType.GAMEPAD));
                    _controlsKeyboardBackButton.onClick.RemoveListener(GoBackFromRebinding);
                    _controlsGamepadBackButton.onClick.RemoveListener(GoBackFromRebinding);
                    
                    _masterVolumeSlider.onValueChanged.AddListener((x) => SetVolume(AudioSliderType.Master, x));
                    _musicVolumeSlider.onValueChanged.AddListener((x) => SetVolume(AudioSliderType.Music, x));
                    _sfxVolumeSlider.onValueChanged.AddListener((x) => SetVolume(AudioSliderType.SFX, x));
                    
                    break;
            }
        }

        #region FMOD

        private void SetUpFMOD()
        {
            GetFMODReferences();
            SetFMODSlider();

        }

        private void GetFMODReferences()
        {
            masterBus = RuntimeManager.GetBus(GetBusString(AudioSliderType.Master));
            musicBus = RuntimeManager.GetBus(GetBusString(AudioSliderType.Music));
            sfxBus = RuntimeManager.GetBus(GetBusString(AudioSliderType.SFX));
        }

        private void SetFMODSlider()
        {
            float volume;
            GetBus(AudioSliderType.Master).getVolume(out volume);
            _masterVolumeSlider.value = volume;
            
            GetBus(AudioSliderType.Music).getVolume(out volume);
            _musicVolumeSlider.value = volume;
            
            GetBus(AudioSliderType.SFX).getVolume(out volume);
            _sfxVolumeSlider.value = volume;
        }
            
        
        private void SetVolume(AudioSliderType type, float value)
        {
            GetBus(type).setVolume(value);
        }

        private string GetBusString(AudioSliderType type)
        {
            return type switch
            {
                AudioSliderType.Master => "bus:/",
                AudioSliderType.Music => "bus:/Music",
                AudioSliderType.SFX => "bus:/SFX",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private Bus GetBus(AudioSliderType type)
        {
            return type switch
            {
                AudioSliderType.Master => masterBus,
                AudioSliderType.Music => musicBus,
                AudioSliderType.SFX => sfxBus,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private Slider GetAudioSlider(AudioSliderType type)
        {
            return type switch
            {
                AudioSliderType.Master => _masterVolumeSlider,
                AudioSliderType.Music => _musicVolumeSlider,
                AudioSliderType.SFX => _sfxVolumeSlider,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        #endregion
        
        private void SwitchOptions(OptionsType type)
        {
            if (_currentOption == type)
                return;

            //_audioSettingsButton.interactable = type == OptionsType.AUDIOSETTINGS;
            _audioSettingsGameObject.SetActive(type == OptionsType.AUDIOSETTINGS);
            _audioBackgroundGameObject.SetActive(type == OptionsType.AUDIOSETTINGS);

            //_controllerRebindButton.interactable = type == OptionsType.CONTROLLERREBIND;
            _controllerSettingsGameObject.SetActive(type == OptionsType.CONTROLLERREBIND);
            _controllerBackgroundGameObject.SetActive(type == OptionsType.CONTROLLERREBIND);

            _currentOption = type;
        }

        private void GoToControlsRebind(ControlType type)
        {
            if (_controlContext == type)
                return;

            _keyboardSettingsObject.SetActive(type == ControlType.KEYBOARD);
            _gamepadSettingsObject.SetActive(type == ControlType.GAMEPAD);

            _controlsKeyboardBackButton.gameObject.SetActive(type != ControlType.NONE);
            _controlsGamepadBackButton.gameObject.SetActive(type != ControlType.NONE);
            _keyButtonSettingGameObject.SetActive(type != ControlType.NONE);

            _controlContext = type;
        }

        private void GoBackFromRebinding()
        {
            GoToControlsRebind(ControlType.NONE);
        }

        protected virtual void BackButtonPressed()
        {
            SwitchOptions(OptionsType.CONTROLLERREBIND); 
            _optionsMenuGameObject.SetActive(false);
        }


    }

    public enum AudioSliderType
    {
        Master = 1,
        Music = 2,
        SFX = 3
    }
    
}

