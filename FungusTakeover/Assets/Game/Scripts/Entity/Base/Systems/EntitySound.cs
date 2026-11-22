using System;
using Events;
using FMOD.Studio;
using Game.Sounds;
using UnityEngine;
using Zenject;

namespace Game.Entity
{
    public class EntitySound : EntitySystem
    {
        [Inject] private SoundManager _soundManager;
        [InjectOptional] private FMODEventsEntitySoundsConfiguration _soundConfig;
        [Inject] private EntityResourceManager _resourceManager;
        [Inject] private EntityBase _base;
        
        //Toggleable SFXs
        private SoundInstanceHandler _walking;
        
        #region UnityEvents

        private void Awake()
        {
            if (_soundConfig == null)
                this.enabled = false;
        }

        private void Start()
        {
            SpawnAllInstances();
            
        }

        private void OnEnable()
        {
            ToggleEvents(true);
        }

        private void OnDisable()
        {
            StopAll();
            
            ToggleEvents(false);
        }
        #endregion

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch(shouldSubscribe)
            {
                case true:
                    _events.EntityUnit.OnTakenEntityDamage += Damage;
                    _events.EntityUnit.OnTakenEnvironmentDamage += Damage;
                    _events.EntityUnit.TriggerAttack += Attack;
                    _events.State.StateChanged += StateChanged;
                    _events.InputActions.Moving += Movement;
                    _events.EntityUnit.EatAlly += Absorb;
                    //_events.EntityUnit.ConvertIntoAlly += TakeOver;
                    _events.ResourceOutput.EntityContext += TakeOver;
                    break;  
                case false:
                    _events.EntityUnit.OnTakenEntityDamage -= Damage;
                    _events.EntityUnit.OnTakenEnvironmentDamage -= Damage;
                    _events.EntityUnit.TriggerAttack -= Attack;
                    _events.State.StateChanged -= StateChanged;
                    _events.InputActions.Moving -= Movement;
                    _events.EntityUnit.EatAlly -= Absorb;
                    //_events.EntityUnit.ConvertIntoAlly -= TakeOver;
                    _events.ResourceOutput.EntityContext -= TakeOver;
                    break;
            }
        }

        private void TakeOver(EntityContextType type)
        {
            if (type != EntityContextType.Ally)
                return;
            
            Debug.Log("EntitySound: TakeOver");
            ResolveSoundType(EntitySFXType.TakeOver);
        }

        private void Absorb()
        {
            if (_resourceManager.EntityContext.ResourceValue != EntityContextType.Player)
                return;
            
            ResolveSoundType(EntitySFXType.EatAllies);
        }

        #region SoundMethods

        private void SpawnAllInstances()
        {
            _walking = _soundManager.FactorySoundAudioHandler.CreateEventInstance(_soundConfig.Sounds.Walking);
        }

        private void StopAll()
        {
            _walking?.StopLoop(STOP_MODE.IMMEDIATE);
        }

        private void ResolveSoundType(EntitySFXType type, bool triggerOn = false)
        {
            switch (type)
            {
                case EntitySFXType.Attack:
                case EntitySFXType.Damage:
                case EntitySFXType.Death:
                case EntitySFXType.CallAllies:
                case EntitySFXType.EatAllies:
                case EntitySFXType.SporeShoot:
                case EntitySFXType.TakeOver:
                    ResolveSingleSoundDemand(type);
                    break;
                case EntitySFXType.Walking:
                    ResolveLoopSoundDemand(type, triggerOn);
                    break;
            }
        }

        private void ResolveSingleSoundDemand(EntitySFXType type)
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf)
                return;
            _soundManager.SFXAudioHandler.PlayOneShot(_soundConfig.Sounds.GetEventReference(type), transform.position);
            
        }

        private void ResolveLoopSoundDemand(EntitySFXType type, bool isOn)
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf)
                return;

            SoundInstanceHandler instance = type switch
            {
                EntitySFXType.Walking => _walking,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            SwitchPlaybackState(instance, isOn);
        }

        private void SwitchPlaybackState(SoundInstanceHandler instance, bool turnOn)
        {
            switch (turnOn)
            {
                case true:
                    instance.StopLoop();
                    break;
                case false:
                    instance?.StopLoop(STOP_MODE.ALLOWFADEOUT);
                    break;
            }
        }
        #endregion

        private void StateChanged(EntityStateType obj)
        {
            if (obj == 0)
                return;
            
            switch (obj)
            {
                case EntityStateType.Awake:
                    break;
                case EntityStateType.Idle:
                    if (_resourceManager.EntityContext.ResourceValue == EntityContextType.Enemy)
                    {
                        if (_base.navMeshAgent != null)
                            Movement(_base.navMeshAgent.velocity);
                    }
                    break;
                case EntityStateType.Alert:
                    break;
                case EntityStateType.Attack:
                    break;
                case EntityStateType.Stun:
                    break;
                case EntityStateType.Dead:
                    ResolveSoundType(EntitySFXType.Death);
                    break;
                case EntityStateType.Formation:
                    if (_resourceManager.EntityContext.ResourceValue == EntityContextType.Player)
                    {
                        ResolveSoundType(EntitySFXType.CallAllies);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }

        private void Movement(Vector3 movement)
        {
            ResolveLoopSoundDemand(EntitySFXType.Walking, movement.magnitude > 0.05);
        }
        
        private void Movement(Vector2 movement)
        {
            ResolveLoopSoundDemand(EntitySFXType.Walking, movement.magnitude > 0.05);
        }
        
        
        private void Damage()
        {
            ResolveSoundType(EntitySFXType.Damage);
        }

        private void Attack()
        {
            ResolveSoundType(EntitySFXType.Attack);
        }
    }
}


