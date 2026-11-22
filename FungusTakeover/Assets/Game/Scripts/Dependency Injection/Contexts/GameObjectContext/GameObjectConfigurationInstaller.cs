using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
    [CreateAssetMenu(fileName = nameof(GameObjectConfigurationInstaller), menuName = Game.Constants.Project.AssetMenuStart + nameof(GameObjectConfigurationInstaller))]
    public class GameObjectConfigurationInstaller : ScriptableObjectInstaller<GameObjectConfigurationInstaller>
    {
        [SerializeField] private List<ScriptableObject> _configurations = new List<ScriptableObject>();
        public List<ScriptableObject> Configurations { get => _configurations; }

#if DEBUG || UNITY_EDITOR
        [SerializeField] private List<ScriptableObject> _debugConfigurations = new List<ScriptableObject>();
        public List<ScriptableObject> DebugConfigurations { get => _debugConfigurations; }
#endif
#if !DEBUG || UNITY_EDITOR
        [SerializeField] private List<ScriptableObject> _releaseConfigurations = new List<ScriptableObject>();
        public List<ScriptableObject> ReleaseConfigurations { get => _releaseConfigurations; }
#endif

        public override void InstallBindings()
        {
            for (int i = 0; i < _configurations.Count; i++)
            {
                BinConfig(Container, _configurations[i]);
            }
#if DEBUG
            for (int i = 0; i < _debugConfigurations.Count; i++)
            {
                BinConfig(Container, _debugConfigurations[i]);
            }
#else
        for (int i = 0; i < _releaseConfigurations.Count; i++)
        {
            BinConfig(Container, _releaseConfigurations[i]);
        }
#endif
        }

        private static void BinConfig(DiContainer diContainer, ScriptableObject scriptableObject)
        {
            if (scriptableObject == null)
                throw new NullReferenceException("Configuration in Config Installer is null!");

            diContainer.Bind(scriptableObject.GetType())
                .FromInstance(scriptableObject)
                .AsSingle()
                .NonLazy();
        }

    }
}
