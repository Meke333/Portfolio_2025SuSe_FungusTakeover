using System;
using UnityEngine;
using Zenject;

namespace Game.Entity
{
    public class EnemyDeathNotifier : EnemySystem
    {
        [InjectOptional] EnemyEvents _enemyEvents;
        [Inject] EntityResourceManager _resourceManager;

        #region UnityMethods

        public void Awake()
        {
            //Debug.Log("EnemyWave: Awake Hello: " + _base.ContextType); 
            if (_base.ContextType != EntityContextType.Enemy)
            {
                this.enabled = false;
                return;
            }

            if (_enemyEvents == null)
            {
                this.enabled = false;
                throw new InvalidOperationException("EnemyEvent could not be grasped for this EnemyEntity: " + _base.gameObject.ToString());
            }

        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _events.State.StateChanged += StateChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _events.State.StateChanged -= StateChanged;
        }
        #endregion
        private void StateChanged(EntityStateType type)
        {
            //Debug.Log("EnemyWave: StateChanged: " + type);
            if (!this.enabled)
                return;

            if (type != EntityStateType.Dead)
                return;

            //Debug.Log("EnemyWave: EntityDead Notification");
            _enemyEvents.OnEnemyDead?.Invoke(_base);

            this.enabled = false;
        }

    }

}
