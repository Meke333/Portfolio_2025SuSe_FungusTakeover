using System;
using UnityEngine;
using TMPro;
using Zenject;
using Events;
using Game.Entity;
using UnityEngine.Serialization;


namespace Game.UI
{
    public class SporesDisplay : GameUIElement
    {
        [Inject] private GlobalProjectSettings _globalProjectSettings;
        [Inject] private ProjectEvents _projectEvents;
        //Should be displayed in a different Color if the SporeAmount is depleted
        [SerializeField] private TextMeshProUGUI sporeAmountText;

        private int _currentAmount = 0;
        private int _maxAmount = 0;

        #region UnityMethods

        private void Awake()
        {
            TransmitMaxSporeData(_globalProjectSettings.StartSpores);
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
            switch(shouldSubscribe)
            {
                case true:
                    _projectEvents.Entity.SporeAmountChanged += TransmitCurrentSporeData;
                    _projectEvents.Entity.SporeMaximumChanged += TransmitMaxSporeData;
                    break;
                case false:
                    _projectEvents.Entity.SporeAmountChanged -= TransmitCurrentSporeData;
                    _projectEvents.Entity.SporeMaximumChanged -= TransmitMaxSporeData;
                    break;
            }
        }


        public void TransmitCurrentSporeData(int data)
        {
            _currentAmount = data;
            UpdateSporeDisplay();
        }

        public void TransmitMaxSporeData(int data)
        {
            _maxAmount = data;
            UpdateSporeDisplay();
        }

        private void UpdateSporeDisplay()
        {
            if (sporeAmountText == null)
                return;
            sporeAmountText.SetText($"{_currentAmount}   {_maxAmount}");
        }
    }
}


