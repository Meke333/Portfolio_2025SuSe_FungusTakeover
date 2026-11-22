using Zenject;
using Helper;
using Game.GameFlow.Helper;

namespace Game.GameFlow
{
    public class GameFlowManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameFlowStateData>()
            .AsSingle()
            .NonLazy();

            //DON'T FORGET YOU DUMB SHIT ME
            Container.Bind<SubClassHelper>()
                .To<GameFlowSubClassHelper>()
                .AsSingle()
                .NonLazy();
        }
    }
}

namespace Game.GameFlow.Helper
{
        public class GameFlowSubClassHelper : SubClassHelper
    {
        
    }
}

