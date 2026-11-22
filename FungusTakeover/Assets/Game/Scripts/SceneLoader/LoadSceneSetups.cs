using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(LoadSceneSetups), menuName = Game.Constants.Project.AssetMenuStart + nameof(LoadSceneSetups))]
    public class LoadSceneSetups : ScriptableObject
    {
#if UNITY_EDITOR
        public General.MultiSceneSetup multiSceneSetup;
#endif
        [ReadOnly]
        public SceneInformation[] scenes;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (multiSceneSetup == null)
                return;

            scenes = new SceneInformation[multiSceneSetup.Setups.Length];

            for (int i = 0; i < multiSceneSetup.Setups.Length; i++)
            {
                SceneInformation temp;

                temp.path = multiSceneSetup.Setups[i].path;
                temp.isLoaded = multiSceneSetup.Setups[i].isLoaded;
                temp.isActive = multiSceneSetup.Setups[i].isActive;
                temp.isSubScene = multiSceneSetup.Setups[i].isSubScene;

                scenes[i] = temp;
            }
        }
#endif
    }

    [System.Serializable]
    public struct SceneInformation
    {
        public string path;
        public bool isLoaded;
        public bool isActive;
        public bool isSubScene;

    }
}