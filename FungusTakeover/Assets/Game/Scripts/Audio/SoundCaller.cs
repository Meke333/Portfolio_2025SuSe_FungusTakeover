using System.Collections.Generic;
using Events;
using Game.Entity;
using UnityEngine;
using Zenject;

namespace Game.Sounds
{
    public class SoundCaller : MonoBehaviour
    {
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private AllyEvents _allyEvents;
        [Inject] private SoundManager _soundManager;
        [Inject] private FMODEventsConfiguration _soundConfig;
        [Inject] private EntityManagers _managers;
        
        #region UnityMethods

        private void Awake()
        {
            
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
                    _projectEvents.GameFlow.GameOver += () => ResolveSingleSoundDemandFollowing(SFXType.GameOver);
                    _allyEvents.AddNewAllyToManager += (x) => ResolveSingleSoundDemandFollowing(SFXType.TakeOver);
                    //_allyEvents.GivePlayerHealth += (x) =>  ResolveSingleSoundDemandFollowing(SFXType.EatAllies);
                    _projectEvents.Dungeon.MovedIntoNextRoom += (x) => ResolveSingleSoundDemandFollowing(SFXType.Portal);
                    _projectEvents.Dungeon.OpenUpDoors += () => ResolveSingleSoundDemandFollowing(SFXType.DoorOpen);
                    _projectEvents.GameFlow.GameWin += () => ResolveSingleSoundDemandFollowing(SFXType.TakeOver);
                    _projectEvents.GameFlow.GameOver += () => ResolveSingleSoundDemandFollowing(SFXType.GameOver);
                    break;
                case false:
                    _projectEvents.GameFlow.GameOver -= () => ResolveSingleSoundDemandFollowing(SFXType.GameOver);
                    _allyEvents.AddNewAllyToManager -= (x) => ResolveSingleSoundDemandFollowing(SFXType.TakeOver);
                    //_allyEvents.GivePlayerHealth -= (x) =>  ResolveSingleSoundDemandFollowing(SFXType.EatAllies);
                    _projectEvents.Dungeon.MovedIntoNextRoom -= (x) => ResolveSingleSoundDemandFollowing(SFXType.Portal);
                    _projectEvents.Dungeon.OpenUpDoors -= () => ResolveSingleSoundDemandFollowing(SFXType.DoorOpen);
                    _projectEvents.GameFlow.GameWin -= () => ResolveSingleSoundDemandFollowing(SFXType.TakeOver);
                    _projectEvents.GameFlow.GameOver -= () => ResolveSingleSoundDemandFollowing(SFXType.GameOver);
                    break;
            }
        }

        private void ResolveSingleSoundDemand(SFXType type)
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf)
                return;
            
            _soundManager.SFXAudioHandler.PlayOneShot(_soundConfig.Sounds.GetEventReference(type), _managers.allyManager.Player.transform.position);
            
        }
        
        
        private void ResolveSingleSoundDemandFollowing(SFXType type)
        {
            Debug.Log("Audio: " + type);
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf)
                return;

            GameObject target =
                EqualityComparer<EntityBase>.Default.Equals(_managers.allyManager.Player, null)
                    ? _managers.allyManager.followPlayerTransform.gameObject
                    : _managers.allyManager.Player.gameObject;
            
            _soundManager.SFXAudioHandler.PlayFollowedShot(_soundConfig.Sounds.GetEventReference(type), target);
            
        }
        
        
        
    }
}