using Events;
using Game.Entity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class AllyDisplay : GameUIElement
    {
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private EntityManagers _entityManager;
        [Inject] private GlobalProjectSettings _globalSettings;

        [SerializeField] private RectTransform _allyHeader;
        
        [SerializeField] private RectTransform _displayParent;

        private AllySporeCostDisplay[] _allSporeCostDisplay;

        private List<EntityBase> _savedList = new List<EntityBase>();
        private int _maxAmount;

        #region UnityMethods
        
        private void Awake()
        {
            _allSporeCostDisplay = _displayParent.GetComponentsInChildren<AllySporeCostDisplay>(true);
            TransmitMaxSporeData(_globalSettings.StartSpores);
        }
        private void OnValidate()
        {
            _allSporeCostDisplay = _displayParent.GetComponentsInChildren<AllySporeCostDisplay>(true);
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
                    _projectEvents.Entity.OnAllyListChanged += TransmitAllyListData;
                    _projectEvents.Entity.SporeMaximumChanged += TransmitMaxSporeData;

                    break;
                case false:
                    _projectEvents.Entity.OnAllyListChanged -= TransmitAllyListData;
                    _projectEvents.Entity.SporeMaximumChanged -= TransmitMaxSporeData;
                    break;
            }
        }


        private void TransmitAllyListData(List<EntityBase> data)
        {
            /*
            // Check which Entity is gone
            List<EntityBase> lostEntities = new List<EntityBase>();
            for (int i = 0; i < _savedList.Count; i++)
            {
                if (data.Contains(_savedList[i]))
                    continue;

                lostEntities.Add(_savedList[i]);
            }


            // Remove Entities 
            for (int i = 0; i < lostEntities.Count; i++)
            {
                //Add Animations?
            }
            */

            /*
            // Check for new Entities
            List<EntityBase> newEntities = new List<EntityBase>();
            for (int i = 0; i < data.Count; i++)
            {
                if (_savedList.Contains(data[i]))
                    continue;

                newEntities.Add(data[i]);
            }

            //Add Entities
            for (int i = 0; i < newEntities.Count; i++)
            {
                //Add Animations?
            }*/

            //Update SporeDisplays
            int count = 0;
            int offset = 0;
            for (; count < data.Count; count++)
            {
                if (data[count] == _entityManager.allyManager.Player && _entityManager.allyManager.Player.StatsConfiguration.Player.PlayerValueContainer.playerActions.HasFlag(PlayerActions.All))
                {
                    offset = -1;
                    continue;
                }

                _allSporeCostDisplay[count + offset].SetEntity = data[count];
                _allSporeCostDisplay[count + offset].ChangeSporeDisplay(data[count].StatsConfiguration.Enemy.EntityStatContainer.sporeCost);
            }
            for (; count < _allSporeCostDisplay.Length; count++)
            {
                _allSporeCostDisplay[count + offset].SetEntity = null;
                _allSporeCostDisplay[count + offset].ChangeSporeDisplay(0);
            }

            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_displayParent);
        }
        
        private void TransmitMaxSporeData(int value)
        {
            _maxAmount = value;
            _allyHeader.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Max(200, _maxAmount * 40));
        }
    }

}


