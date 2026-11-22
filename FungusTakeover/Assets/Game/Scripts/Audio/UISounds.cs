using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Sounds
{
    public class UISounds : MonoBehaviour
    {
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private SoundManager _soundManager;
        [Inject] private FMODEventsConfiguration _configuration;
        

        [SerializeField] private Button _button;
        [SerializeField] private Slider _slider;
        [SerializeField] private SFXType _type;
        
        #region UnityMethod

        private void Start()
        {
            if (_button != null)
                _button.onClick.AddListener(() => ResolveSingleSoundDemand(_type));
            
            if (_slider != null)
                _slider.onValueChanged.AddListener((x) => ResolveSingleSoundDemand(_type));
        }

        private void OnDestroy()
        {
            
            if (_button != null)
                _button.onClick.RemoveListener(() => ResolveSingleSoundDemand(_type));
            
            if (_slider != null)
                _slider.onValueChanged.RemoveListener((x) => ResolveSingleSoundDemand(_type));
        }

        #endregion

        public void ResolveSingleSoundDemand(SFXType type)
        {
            if (EqualityComparer<UnityEngine.Camera>.Default.Equals(UnityEngine.Camera.main, null))
                return; 
            
            _soundManager.SFXAudioHandler.PlayOneShot(_configuration.Sounds.GetEventReference(type), UnityEngine.Camera.main.transform.position);
        }
    }
}