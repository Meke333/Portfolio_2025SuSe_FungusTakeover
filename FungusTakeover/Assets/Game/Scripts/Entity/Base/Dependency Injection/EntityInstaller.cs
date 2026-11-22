using Game.Entity;
using Game.Entity.Helper;
using Game.Entity.Events;
using Helper;
using Zenject;


namespace Game.Entity
{
    public class EntityInstaller : MonoInstaller
    {


        public override void InstallBindings()
        {
            Container.Bind<EntityBase>()
                .FromInstance(GetComponent<EntityBase>())
                .AsSingle();

            Container.Bind<EntityEvents>()
                .AsSingle()
                .NonLazy();

            Container.Bind<EntityResourceManager>()
                .AsSingle()
                .NonLazy();

            Container.Bind<EntityStateData>()
                .AsSingle()
                .NonLazy();

            Container.Bind<SubClassHelper>()
                .To<EntitySubClassHelper>()
                .AsSingle()
                .NonLazy();

            //Single States for Statemachine
            //SetUpPlayerStatemachine();        

        }

        private void SetUpPlayerStatemachine()
        {

        }
    }


}
