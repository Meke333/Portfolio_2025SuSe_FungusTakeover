using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


#if UNITY_EDITOR
#endif


namespace DependencyInjection
{
    [CreateAssetMenu(fileName = nameof(OverarchingConfigurationInstaller), menuName = Game.Constants.Project.AssetMenuStart + nameof(OverarchingConfigurationInstaller))]
    public class OverarchingConfigurationInstaller : EditorScriptableObjectInstaller
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


        protected override void InstallBindings(DiContainer container)
        {

            for (int i = 0; i < _configurations.Count; i++)
            {
                BinConfig(container, _configurations[i]);
            }
#if DEBUG || UNITY_EDITOR
            for (int i = 0; i < _debugConfigurations.Count; i++)
            {
                BinConfig(container, _debugConfigurations[i]);
            }
#else
            for (int i = 0; i < _releaseConfigurations.Count; i++)
            {
                BinConfig(container, _releaseConfigurations[i]);
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

