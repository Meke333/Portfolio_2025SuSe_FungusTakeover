using Events;
using Game.Entity;
using Game.Sounds;
using Game.UI;
using Helper;
using Interface;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
    public class PersistentContextInstaller : MonoInstaller
    {
        [SerializeField] private SceneLoader.CoreSceneLoader sceneLoader;
        [SerializeField] private MusicAudioHandler audioHandler;
        [SerializeField] private AmbienceAudioHandler ambienceHandler;
        
        public override void InstallBindings()
        {
            Container.Bind<ProjectEvents>().AsSingle().NonLazy();
            
            Container.Bind<FactoryHelper>().AsSingle().NonLazy();
            Container.Bind<SubClassHelper>().AsSingle().NonLazy();
            
            Container.Bind<ISceneLoader>().FromInstance(sceneLoader).AsSingle().NonLazy();
            
            Container.Bind<EntityManagers>().AsSingle().NonLazy();
            Container.Bind<AllyEvents>().AsSingle().NonLazy();

            Container.Bind<SoundManager>().AsSingle().NonLazy();
            Container.Bind<MusicAudioHandler>().FromInstance(audioHandler).AsSingle().NonLazy();
            Container.Bind<AmbienceAudioHandler>().FromInstance(ambienceHandler).AsSingle().NonLazy();
            
            Container.Bind<GameUIHolder>().AsSingle().NonLazy();
        }

    }
}