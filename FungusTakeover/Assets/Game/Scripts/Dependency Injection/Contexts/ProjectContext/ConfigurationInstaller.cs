using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif


namespace DependencyInjection
{
    [CreateAssetMenu(fileName = nameof(ConfigurationInstaller), menuName = Game.Constants.Project.AssetMenuStart + nameof(ConfigurationInstaller))]
    public class ConfigurationInstaller : ScriptableObjectInstaller
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
#if DEBUG || UNITY_EDITOR
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

            diContainer.BindInterfacesToInstance(scriptableObject, true).AsCached();
        }
    }
}

