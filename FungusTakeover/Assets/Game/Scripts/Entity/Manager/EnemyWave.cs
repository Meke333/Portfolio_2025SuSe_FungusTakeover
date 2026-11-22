using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Entity
{
    [System.Serializable]
    public class EnemyWave : MonoBehaviour
    {
        [Inject] private EnemyEvents _enemyEvents;

        [field: SerializeField, ReadOnly] public EntityBase[] enemies { get; private set; }

        [ReadOnly] public int _originalAmount;
        [ReadOnly] public int _activeEnemies;
        //When a wave cleared this variable determines how long it will take for the next one to start
        public float _spawnTime;


        #region UnityMethods
        void Awake()
        {
            Initialize();
        }


        #endregion


        private void Initialize()
        {
            enemies = this.transform.GetComponentsInChildren<EntityBase>(true);
            _originalAmount = enemies.Length;
            _activeEnemies = _originalAmount;
        }

        public void EnemyDied()
        {
            _activeEnemies--;

            if (_activeEnemies > 0)
                return;

            _enemyEvents.OnWaveCleared?.Invoke(this);
        }

    }
}

