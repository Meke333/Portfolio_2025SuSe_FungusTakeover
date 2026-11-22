using Helper;
using System;
using UnityEngine;
using Zenject;
using Game.Entity.Events;

namespace Game.Entity
{

    public class EntityAttack : EntitySystem
    {
        [Inject] private EntityBase _base;
        [Inject] private SubClassHelper _subClassHelper;
        [Inject] private EntityAllStatsConfiguration _allStatsConfigurations;
        [Inject] private EntityResourceManager _resourceManager;
        [Inject] private EntityEvents _entityEvents;

        //[SerializeField] private Transform _hitboxParent;
        [SerializeField] private Transform _attackHitbox;
        

        private EntityAttack_Base _brain;

        #region UnityMethod
        private void Awake()
        {
            _events.ResourceOutput.EntityContext += GetNewInputBrain;
            GetNewInputBrain(_base.ContextType);
        }

        private void OnEnable()
        {
            ToggleEvents(true);
        }

        private void OnDisable()
        {
            ToggleEvents(false);
        }

        private void OnDestroy()
        {
            _events.ResourceOutput.EntityContext -= GetNewInputBrain;
        }
        #endregion

        private void FixedUpdate()
        {
            if (_brain == null || _base == null)
                return;

            if (_brain.AnimatedCollider && _brain._timer > 0) //animated attack
            {
                _brain._timer -= MyTime.deltaTime;

                _brain.ProcessAnimatedAttack();
                if (_brain._timer <= 0)
                    _events.EntityUnit.FinishTransmittingAttackData?.Invoke();
            }
            
        }
        private void Process()
        {
            _brain.ProcessAttack();
        }

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.EntityUnit.TriggerAttack += Process;
                    break;
                case false:
                    _events.EntityUnit.TriggerAttack -= Process;
                    break;
            }
        }

        private void GetNewInputBrain(EntityContextType type)
        {
            _brain = _subClassHelper.GetNewInputBrain<EntityAttack_Base>(this, _allStatsConfigurations.GetStats(type).EntityStatContainer.attackBehaviourType.ToString());
            

            //_hitboxParent.localScale = Vector3.one * _allStatsConfigurations.GetStats(type).EntityStatContainer.attackRange;
            //Put in values into the Brain
            _brain.Initialize(_allStatsConfigurations.GetStats(type), _attackHitbox, type);
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                if (_brain == null)
                    return;
                _brain.DrawGizmos();
            }
        }
    }
}

