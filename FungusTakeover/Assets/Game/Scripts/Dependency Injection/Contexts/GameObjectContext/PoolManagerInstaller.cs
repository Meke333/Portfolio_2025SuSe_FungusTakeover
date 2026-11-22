using Interface;
using Zenject;

namespace DependencyInjection
{
    public class PoolManagerInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            IPoolManager poolManager = gameObject.GetComponent<IPoolManager>();

            Container.Bind<IPoolManager>()
                .FromInstance(poolManager)
                .AsSingle()
                .NonLazy();
        }
    }
}

