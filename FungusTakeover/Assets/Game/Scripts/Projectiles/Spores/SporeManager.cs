using Game.Configuration;
using ObjectPool;
using UnityEngine;
using Zenject;


namespace Game.Projectiles
{
    public class SporeManager : PoolManagerBase<SporeUnit>
    {
        [Inject] private PrefabConfiguration _prefabConfiguration;

        [SerializeField] private int objectAmount;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            pool.Construct(transform, _prefabConfiguration.SporePrefab, objectAmount, this);

            _dataBase.APoolIsFilled();
        }
    }

}

