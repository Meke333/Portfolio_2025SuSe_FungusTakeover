using Game.Entity;
using Zenject;

public class EnemyManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<EnemyEvents>()
        .AsSingle()
        .NonLazy();
    }

}