using ObjectPool;
using Zenject;

namespace DependencyInjection
{
    public class InteractableSceneInstaller : MonoInstaller
	{
        public override void InstallBindings()
        {
            Container.Bind<PoolManagerData>()
                .AsSingle()
                .NonLazy();
        }

    }
}