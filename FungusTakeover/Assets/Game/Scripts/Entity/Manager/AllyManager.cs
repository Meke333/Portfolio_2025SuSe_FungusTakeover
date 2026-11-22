using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Entity
{
    /// <summary>
    /// This Manages a list of all Allies
    /// And is also responsible for spores
    /// and picking a new Player
    /// and cooldown for calling allies
    /// </summary>
    public class AllyManager : MonoBehaviour
    {
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private AllyEvents _allyEvents;
        [Inject] private EntityManagers _entityManagers;
        [Inject] private GlobalProjectSettings _globalSettings;

        [field: SerializeField]
        public EntityBase Player { get; private set; }
        [field: SerializeField, ReadOnly]
        public List<EntityBase> AlliesList { get; private set; } = new List<EntityBase>();

        [field: SerializeField, ReadOnly] public int MaxSporeAmount { get; private set; }
        [SerializeField, ReadOnly] private int _sporeAmount;

        [field: SerializeField] public Transform followPlayerTransform { get; private set; } 

        public int SporeAmount
        {
            get => _sporeAmount;
            set
            {
                _sporeAmount = value;
                //Call Event -> successfully changed?
                if (_sporeAmount < 0) //cap it at 0
                    _sporeAmount = 0;

                if (_sporeAmount > MaxSporeAmount) //cap it at max spores
                    _sporeAmount = MaxSporeAmount;

                _projectEvents.Entity.SporeAmountChanged?.Invoke(_sporeAmount);
            }
        }
        public void UpdateSporesFromSettings()
        {
            MaxSporeAmount = _globalSettings.StartSpores;
            _projectEvents.Entity.SporeMaximumChanged?.Invoke(MaxSporeAmount);

            SporeAmount = 0;
        }

        #region UnityMethods

        void Awake()
        {
            _entityManagers.allyManager = this;

            if (Player == null)
                throw new InvalidOperationException("There is no Player inserted!!");

            AddAlly(Player);
            Player.OnDeath += () => { TryGetNewNewPlayer(Player); };
            //grab spores
            SporeAmount = _globalSettings.StartSpores;
            MaxSporeAmount = _globalSettings.StartSpores;
        }

        public void Start()
        {
            _projectEvents.Entity.SporeMaximumChanged?.Invoke(MaxSporeAmount);
        }


        void OnEnable()
        {
            ToggleEvents(true);
        }

        void OnDisable()
        {
            ToggleEvents(false);
        }


        #endregion

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _projectEvents.Entity.AddNewAllyToManager += AddAlly;
                    _projectEvents.Entity.RemoveNewAllyInManager += DeleteAlly;
                    //_projectEvents.Entity.PlayerDied += TryGetNewNewPlayer;
                    _projectEvents.Entity.PlayerHealthChanged += GivePlayerHealth;
                    _projectEvents.Entity.UseSpores += UseSpores;
                    _projectEvents.Entity.GetSpores += GetSpores;
                    break;
                case false:
                    _projectEvents.Entity.AddNewAllyToManager -= AddAlly;
                    _projectEvents.Entity.RemoveNewAllyInManager -= DeleteAlly;
                    //_projectEvents.Entity.PlayerDied -= TryGetNewNewPlayer;
                    _projectEvents.Entity.PlayerHealthChanged -= GivePlayerHealth;
                    _projectEvents.Entity.UseSpores -= UseSpores;
                    _projectEvents.Entity.GetSpores -= GetSpores;
                    break;
            }
        }

        private void GivePlayerHealth(float obj)
        {
            float _addedHealth = Player.ResourceManager.Health.ResourceValue + obj;
            if (_addedHealth > Player.StatsConfiguration.Player.EntityStatContainer.healthPoints)
                _addedHealth = Player.StatsConfiguration.Player.EntityStatContainer.healthPoints;
            Player.ResourceManager.Health.ResourceValue = _addedHealth;
        }

        /// <summary> Subtracts spores (e.g. UseSpores(3) subtracts 3 spores) </summary>
        public void UseSpores(int _spores)
        {
            SporeAmount -= _spores;
        }
        /// <summary> Adds spores (e.g. GetSpores(3) adds 3 spores) </summary>
        public void GetSpores(int _spores) { SporeAmount += _spores; }

        /// <summary> Adds an ally to the tracker list </summary>
        public void AddAlly(EntityBase entity)
        {
            AlliesList.Add(entity);
            //Dirty ass add it to the ally manager
            entity.transform.parent = this.transform;
            _projectEvents.Entity.OnAllyListChanged?.Invoke(AlliesList);
        }
        /// <summary> Deletes an ally from the tracker list </summary>
        public void DeleteAlly(EntityBase entity)
        {
            AlliesList.Remove(entity);
            GetSpores(entity.StatsConfiguration.Ally.EntityStatContainer.sporeCost);
            //dirty ass kill it
            Destroy(entity.gameObject);
            _projectEvents.Entity.OnAllyListChanged?.Invoke(AlliesList);
        }

        /// <summary> Decides on a new player, if ally list is empty instead fires GameOver </summary>
        private void TryGetNewNewPlayer(EntityBase _player)
        {
            followPlayerTransform.position = Player.transform.position;
            
            Player.OnDeath -= () => { TryGetNewNewPlayer(Player); };

            Vector3 deadPlayerPosition = Player.transform.position;
            Player = null;
            AlliesList.Remove(_player);
            GetSpores(_player.StatsConfiguration.Player.EntityStatContainer.sporeCost);
            Destroy(_player.gameObject);

            if (AlliesList.Count <= 0)
            {
                Debug.Log("GameOver");
                followPlayerTransform.position = deadPlayerPosition;
                _projectEvents.GameFlow.GameOver?.Invoke();
                return;
            }
            

            //Pick new player based on the ally with the most health
            int _index = 0; //select the first ally by default in case there's an error or sth idk
            float _hp = 0f;
            for (int i = 0; i < AlliesList.Count; i++)
            {
                EntityBase _entity = AlliesList[i];
                _hp = _entity.ResourceManager.Health.ResourceValue;
                if (AlliesList[_index].ResourceManager.Health.ResourceValue < _hp)
                    _index = i;
            }

            //_index = UnityEngine.Random.Range(0, AlliesList.Count); //random ally

            //Pick Ally and put it into Player Reference
            Player = AlliesList[_index];
            Player.ResourceManager.EntityContext.ResourceValue = EntityContextType.Player;
            _projectEvents.Entity.OnPlayerChanged?.Invoke(Player); //fire event that player has changed
            _projectEvents.Entity.OnAllyListChanged?.Invoke(AlliesList);
            
            Player.OnDeath += () => { TryGetNewNewPlayer(Player); };
        }

        #region GetAllyFunctions
        /// <summary> Grabs nearest ally and returns its transform, default to player if no close target in range </summary>
        public Transform GetClosestAlly(EntityBase _enemy)
        {
            if (AlliesList.Count < 1) //Exit early if no allies are left
                return null;

            float _distance = 0;
            float _tempDistance;
            Transform _targetTransform = null;

            for (int i = 0; i < AlliesList.Count; i++)
            {
                EntityBase _ally = AlliesList[i];
                if (_ally == null)
                    continue;

                _tempDistance = Vector3.Distance(_enemy.transform.position, _ally.transform.position);
                if (i == 0 || _tempDistance < _distance) //initialize or set new distance
                {
                    _distance = _tempDistance;
                    _targetTransform = _ally.transform;
                }
            }
            //If the closest ally enemy isn't close enough to notice range or notice range is disabled, target player
            EnemyValueContainer _enemyValues = _enemy.StatsConfiguration.Enemy.EnemyValueContainer;
            if (_enemyValues.noticeRange < _distance || !_enemyValues.enableNoticeRange)
                _targetTransform = Player.transform;

            return _targetTransform;
        }
        /// <summary> Grabs nearest ally and returns its transform, default to null if no target in range </summary>
        public Transform GetClosestAllyNoPlayer(EntityBase _enemy)
        {
            if (AlliesList.Count < 1) //Exit early if no allies are present apart from player
                return null;

            float _distance = 0;
            float _tempDistance;
            Transform _targetTransform = null;

            for (int i = 0; i < AlliesList.Count; i++)
            {
                EntityBase _ally = AlliesList[i];

                _tempDistance = Vector3.Distance(_enemy.transform.position, _ally.transform.position);
                if (i == 0 || _tempDistance < _distance) //initialize or set new distance
                {
                    _distance = _tempDistance;
                    _targetTransform = _ally.transform;
                }
            }
            //If the closest ally enemy isn't close enough to notice range or notice range is disabled, return null
            EnemyValueContainer _enemyValues = _enemy.StatsConfiguration.Enemy.EnemyValueContainer;
            if (_enemyValues.noticeRange < _distance)
                _targetTransform = null;
            return _targetTransform;
        }
        /// <summary> Grabs ally with the least hp and returns its transform </summary>
        public Transform GetWeakestAlly(EntityBase _enemy)
        {
            if (AlliesList.Count <= 0) //Exit early if no allies are left
                return null;
            float _distance = 0;
            float _hp = 0;
            Transform _targetTransform = null;
            EnemyValueContainer _enemyValues = _enemy.StatsConfiguration.Enemy.EnemyValueContainer;

            for (int i = 0; i < AlliesList.Count; i++)
            {
                EntityBase _ally = AlliesList[i];

                _distance = Vector3.Distance(_enemy.transform.position, _ally.transform.position);
                if (_distance < _enemyValues.noticeRange || !_enemyValues.enableNoticeRange)
                    continue;
                if (_ally.ResourceManager.Health.ResourceValue < _hp) //initialize or set new distance
                {
                    _hp = _ally.ResourceManager.Health.ResourceValue;
                    _targetTransform = _ally.transform;
                }
            }
            return _targetTransform;
        }
        /// <summary> Checks if Player HP is below a certain threshhold </summary>
        public bool IsPlayerBelowHPPercent(float _hpPercent)
        {
            float currentHP = Player.ResourceManager.Health.ResourceValue / Player.StatsConfiguration.Player.EntityStatContainer.healthPoints;
            if (currentHP*100 <= _hpPercent)
                return true;

            return false;
        }
        #endregion

        public async void TeleportAllies(Transform point)
        {
            
            for (int i = 0; i < AlliesList.Count; i++)
            {
                AlliesList[i].navMeshAgent.Warp(point.position + Player.transform.forward.normalized * (-i));

                await Task.Yield();
            }

            await Task.Delay(100);
            _allyEvents.CallFormation?.Invoke();
        }
    }
}

