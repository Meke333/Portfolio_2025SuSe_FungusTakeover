using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Entity
{
    public class EnemyManager : MonoBehaviour
    {
        //This Manager manages EnemyWaves for each Room
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private EnemyEvents _enemyEvents;
        [Inject] private EntityManagers _managers;

        [SerializeField] private bool _isFirstRoom;
        [field: SerializeField] public EnemyWave[] Waves { get; private set; }
        [SerializeField, ReadOnly] private int _currentWave = 0;

        private float _spawnTimer = 0;
        private bool _waveSpawned = false;
        private bool _isReady = false;

        private bool _isFinished;

        #region UnityMethods

        void Start()
        {
            _managers.currentEnemyManager = this;
        }

        void OnEnable()
        {
            ToggleEvents(true);
        }

        void OnDisable()
        {
            ToggleEvents(false);
        }

        void Update()
        {
            UpdateWave();
        }
        
        #endregion

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _enemyEvents.OnWaveCleared += WaveCleared;
                    _enemyEvents.OnEnemyDead += EnemyDied;
                    _projectEvents.Dungeon.FinishedRoomTransition += StartManager;
                    break;
                case false:
                    _projectEvents.Dungeon.FinishedRoomTransition -= StartManager;
                    _enemyEvents.OnWaveCleared -= WaveCleared;
                    _enemyEvents.OnEnemyDead -= EnemyDied;
                    break;
            }
        }

        private void StartManager()
        {
            _isReady = true;
            _projectEvents.Dungeon.WavePercentageChanged(0);
        }

        private void UpdateWave()
        {
            if (!_isFirstRoom && !_isReady)
                return;
            
            if (_isFinished)
                return;

            if (_waveSpawned)
                return;

            _spawnTimer += MyTime.deltaTime;

            if (_spawnTimer < Waves[_currentWave]._spawnTime)
                return;

            StartNextWave();
        }

        private void EnemyDied(EntityBase entity)
        {
            //Debug.Log("EnemyWave: Entitymanager Received");
            Waves[_currentWave].EnemyDied();
        }

        private void WaveCleared(EnemyWave wave)
        {
            PrepareNextWave();
        }

        private void PrepareNextWave()
        {
            _currentWave++;

            if (_currentWave >= Waves.Length)
            {
                _projectEvents.Entity.CurrentRoomCleared?.Invoke();
                _projectEvents.Dungeon.OpenUpDoors?.Invoke();
                _isFinished = true;
                return;
            }

            _spawnTimer = 0;
            _waveSpawned = false;

        }

        private void StartNextWave()
        {
            Waves[_currentWave].gameObject.SetActive(true);
            _waveSpawned = true;
            
            _projectEvents.Dungeon.WavePercentageChanged((float) _currentWave / (Waves.Length - 1));
        }

    }
}

