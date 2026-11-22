using System;
using Game.Entity.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using UnityEngine.AI;

namespace Game.Entity
{
    /// <summary>
    /// This Initializes the whole Entity and manages the Entity
    /// If EntityScripts need to initialize values before the
    /// GameObject is spawned.
    /// or if the EntityContext is changed
    /// </summary>
    public class EntityBase : MonoBehaviour
    {
        [Inject] private EntityEvents _events;
        [Inject] public EntityResourceManager ResourceManager { get; private set; }
        [Inject] public EntityAllStatsConfiguration StatsConfiguration { get; private set; }

        [field: SerializeField, EnumButtons] public EntityContextType ContextType { get; private set; }


        public Vector2 InputVector { get; private set; }
        [field: SerializeField] public NavMeshAgent navMeshAgent { get; private set; }

        [field: SerializeField] public AllyFormation _allyFormation { get; private set; }

        public Vector3 StartPosition { get; private set; }

        public Action OnDeath;


        void Awake()
        {
            StartPosition = transform.position;

            SetUpContext();

            SetUpEntity();

            _events.ResourceOutput.EntityContext += ChangeEntityContext;
        }

        private void OnEnable() { ToggleEvents(true); }
        private void OnDisable() { ToggleEvents(false); }
        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.InputActions.Moving += SaveInputVector;
                    break;
                case false:
                    _events.InputActions.Moving -= SaveInputVector;
                    break;
            }
        }

        void SaveInputVector(Vector2 inputVector)
        {
            InputVector = inputVector;
        }

        void OnDestroy()
        {
            _events.ResourceOutput.EntityContext -= ChangeEntityContext;
        }

        void SetUpEntity()
        {
            if (navMeshAgent == null)
                throw new ArgumentNullException("no navmesh agent.");
            if (_allyFormation == null)
                throw new ArgumentNullException("no ally formation script.");
            navMeshAgent.speed = StatsConfiguration.GetStats(ContextType).EntityStatContainer.speedStat;
            navMeshAgent.angularSpeed = StatsConfiguration.GetStats(ContextType).EntityStatContainer.rotationSpeed;
        }

        public void ChangeEntityContext(EntityContextType type)
        {
            ContextType = type;
            if (navMeshAgent != null)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = StatsConfiguration.GetStats(ContextType).EntityStatContainer.speedStat;
                navMeshAgent.angularSpeed = StatsConfiguration.GetStats(ContextType).EntityStatContainer.rotationSpeed;
            }
        }

        void SetUpContext()
        {
            ResourceManager.EntityContext.ResourceValue = ContextType;
        }

    }
}
