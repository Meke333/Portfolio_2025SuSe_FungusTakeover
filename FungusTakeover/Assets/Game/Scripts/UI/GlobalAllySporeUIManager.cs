using Events;
using UnityEngine;
using Zenject;


namespace Game.UI
{
    public class GlobalAllySporeUIManager : MonoBehaviour
    {
        [Inject] private ProjectEvents _events;

        [SerializeField] private SporesDisplay _sporesDisplay;


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

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.Entity.SporeAmountChanged += SporeAmountChanged;
                    break;
                case false:
                    _events.Entity.SporeAmountChanged -= SporeAmountChanged;
                    break;
            }
        }

        private void SporeAmountChanged(int data)
        {
            //Transmit Data
            _sporesDisplay.TransmitCurrentSporeData(data);

        }
    }


}


