using System;
using Events;
using Helper;
using UnityEngine;
using Zenject;


namespace Game.Entity
{
    public class EntityMovement : EntitySystem
    {
        [Inject] private EntityBase _base;
        [Inject] private SubClassHelper _subClassHelper;
        [Inject] private EntityAllStatsConfiguration _allStatsConfigurations;
        [Inject] private GlobalProjectSettings _globalProjectSettings;
        private EntityMovement_Base _brain;

        private bool _stopMovement = false;
        private float _startYPos = 0f;

        #region UnityMethod
        private void Awake()
        {
            _events.ResourceOutput.EntityContext += GetNewInputBrain;
            
            GetNewInputBrain(_base.ContextType);
        }
        private void Start()
        {
            _startYPos = _base.transform.position.y;
        }

        private void OnEnable()
        {
            ToggleEvents(true);
        }

        private void Update()
        {
            ProcessUpdate();
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

        private void ProcessUpdate()
        {
            if (_stopMovement)
                return;

            _brain.ProcessMovement();
        }

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.EntityUnit.TriggerWindup += StopMovement;
                    _events.EntityUnit.AttackEnded += ResumeMovement;
                    _events.ResourceOutput.State += SwitchToDeath;
                    break;
                case false:
                    _events.EntityUnit.TriggerWindup -= StopMovement;
                    _events.EntityUnit.AttackEnded -= ResumeMovement;
                    _events.ResourceOutput.State -= SwitchToDeath;
                    break;
            }
        }

        private void StopMovement() { _stopMovement = true; _base.navMeshAgent.isStopped = true; _base.navMeshAgent.velocity = Vector3.zero; }
        private void ResumeMovement() { _stopMovement = false; _base.navMeshAgent.isStopped = false; _base.navMeshAgent.velocity = Vector3.zero; }
        private void SwitchToDeath(EntityStateType _state)
        {
            if (_state == EntityStateType.Dead)
            {
                _base.navMeshAgent.ResetPath();
                _base.ResourceManager.Target.ResourceValue = null;
                StopMovement();
            }
            if (_state == EntityStateType.Awake)
            {
                ResumeMovement();
            }
        }

        private void GetNewInputBrain(EntityContextType type)
        {

            _brain = _subClassHelper.GetNewInputBrain<EntityMovement_Base>(this, _allStatsConfigurations.GetStats(type).EntityStatContainer.movementBehaviourType.ToString());

            //Put in values into the Brain
            _brain.Initialize(_base.ResourceManager,_base,_globalProjectSettings);
        }
    }


}

