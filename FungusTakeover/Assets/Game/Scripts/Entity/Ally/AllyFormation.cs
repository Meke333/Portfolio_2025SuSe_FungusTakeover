using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Entity
{
    public class AllyFormation : AllySystem
    {
        [Inject] private AllyEvents _allyEvents;
        [Inject] private EntityResourceManager resourceManager;
        [Inject] private EntityManagers entityManagers;

        public bool isInFormation { get; private set; }
        private float _timer = 0;
        private float _maxTimer = 0;
        

        private void Update()
        {
            if (!isInFormation)
                return;

            _timer += MyTime.deltaTime;

            if (_timer >= _maxTimer)
            {
                FinishFormation(_base.ResourceManager.EntityContext.ResourceValue);
            }
        }

        protected override void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.InputActions.CallingAllies += StartFormation;
                    _allyEvents.CallFormation += StartFormation;
                    //_events.ResourceOutput.EntityContext += FinishFormation;
                    break;
                case false:
                    _events.InputActions.CallingAllies -= StartFormation;
                    _allyEvents.CallFormation -= StartFormation;
                    //_events.ResourceOutput.EntityContext -= FinishFormation;
                    break;
            }
        }

        private void StartFormation()
        {
            StartFormation(true);
        }
        private void StartFormation(bool input)
        {
            if (!input || isInFormation)
                return;

            _maxTimer = entityManagers.allyManager.Player.StatsConfiguration.Player.PlayerValueContainer.formationTime;

            if (resourceManager.EntityContext.ResourceValue == EntityContextType.Ally)
            {
                _base.navMeshAgent.speed = _base.StatsConfiguration.Ally.AllyValueContainer.callSpeedMultiplier;
                _base.navMeshAgent.isStopped = false;
            }

            resourceManager.Target.ResourceValue = entityManagers.allyManager.Player.transform;

            isInFormation = true;
        }
        
        private void FinishFormation(EntityContextType contextType)
        {
            isInFormation = false;
            _timer = 0f;
            _maxTimer = 0f;

            if (_base == null)
                throw new MissingReferenceException("Base isn't injected");
            if (_base.ResourceManager == null)
                throw new MissingReferenceException("Resource manager from base inject isn't injected");
            if (_base.ResourceManager.EntityContext.ResourceValue == 0)
                throw new System.NotImplementedException("Context isn't set");

            if (_base.ResourceManager.EntityContext.ResourceValue == EntityContextType.Ally)
            {
                _base.navMeshAgent.speed = _base.StatsConfiguration.Ally.EntityStatContainer.speedStat;
                _base.navMeshAgent.ResetPath();
                _base.navMeshAgent.isStopped = true;
            }

            if (resourceManager.Target == null)
                throw new MissingReferenceException("Target of ResourceManager isn't initialized");

            _base.ResourceManager.Target.ResourceValue = null;
        }
        /*
        private void FinishFormation(EntityContextType contextType)
        {
            Debug.Log("FinishFormation: " + 1);
            isInFormation = false;
            Debug.Log("FinishFormation: " + 2);

            _timer = 0f;
            Debug.Log("FinishFormation: " + 3);

            _maxTimer = 0f;
            Debug.Log("FinishFormation: " + 4 + "; " + (resourceManager.Target == null));

            Debug.Log("FinishFormation: " + 5 + "; " + (resourceManager.Target.ResourceValue == null));

            Debug.Log("FinishFormation: " + 6 + "; " + resourceManager.EntityContext.ResourceValue);

            if (resourceManager.EntityContext.ResourceValue == EntityContextType.Ally)
            {
                Debug.Log("FinishFormation: " + 7);
                Debug.Log("FinishFormation: " + 8 + "; " + (_base == null));

                _base.navMeshAgent.speed = _base.StatsConfiguration.Ally.EntityStatContainer.speedStat;
                Debug.Log("FinishFormation: " + 9);

                _base.navMeshAgent.ResetPath();
                Debug.Log("FinishFormation: " + 10);

                _base.navMeshAgent.isStopped = true;
                Debug.Log("FinishFormation: " + 11);

            }

            Debug.Log("FinishFormation: " + 12);
        }*/
    }

}
