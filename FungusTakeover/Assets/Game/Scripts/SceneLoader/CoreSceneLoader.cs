using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interface;
using ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoader
{
    public class CoreSceneLoader : MonoBehaviour, ISceneLoader
    {
        private Dictionary<string, Scene> _loadedScenes = new Dictionary<string, Scene>();
        private Dictionary<string, Scene> _allScenes = new Dictionary<string, Scene>();

        private Dictionary<string, Scene> _additionalScenes = new Dictionary<string, Scene>();

        [SerializeField, ReadOnly] private LoadSceneSetups _activeLoadSceneSetups;

        [SerializeField] private LoadSceneSetups[] allSceneSetups;

        public Scene[] removeScenes;

        [SerializeField, ReadOnly] private SceneSetups _currentSceneSetUp;

        public async void Awake()
        {
            
            int length = SceneManager.sceneCount;

            for (int i = 0; i < length; i++)
            {
                AddSceneInDictionary(SceneManager.GetSceneAt(i), _loadedScenes);
            }
            
            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
            string[] scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = UnityEngine.SceneManagement.SceneUtility
                    .GetScenePathByBuildIndex(i);
                
                //Debug.Log(scenes[i]);
                
                AddSceneInDictionary(scenes[i], _allScenes);
                
            }

            await Task.Yield();
            
            //Debug.LogWarning("Awake");

            if (allSceneSetups == null || allSceneSetups.Length <= 0)
                throw new InvalidOperationException("No SceneSetups established!");

            LoadMultiSceneSetups(allSceneSetups[0], _loadedScenes);
            
                
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoadedEvent;
            SceneManager.sceneUnloaded += SceneUnloadedEvent;
            SceneManager.activeSceneChanged += ActiveSceneChangedEvent;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneLoadedEvent;
            SceneManager.sceneUnloaded -= SceneUnloadedEvent;
            SceneManager.activeSceneChanged -= ActiveSceneChangedEvent;
        }

        #region LoadSceneMode.Single

        public void LoadSceneAlone(Scene scene)
        {
            //_activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(scene.path, LoadSceneMode.Single);
            
            RemoveAllScenesInDictionary(_loadedScenes);
            AddSceneInDictionary(scene, _loadedScenes);
        }

        public void LoadSceneAlone(string path)
        {
            if (!_allScenes.ContainsKey(path) || !_loadedScenes.ContainsKey(path))
                return;

            Scene scene = _loadedScenes[path];

            //_activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(path, LoadSceneMode.Single);
            
            RemoveAllScenesInDictionary(_loadedScenes);
            AddSceneInDictionary(scene, _loadedScenes);
        }

        #endregion

        #region LoadSceneMode.Additive

        public void LoadSceneAdditive(Scene scene)
        {
            //_activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(scene.path, LoadSceneMode.Additive);
            
            AddSceneInDictionary(scene, _loadedScenes);
        }
        
        public void LoadSceneAdditive(string path)
        {
            if (!_allScenes.ContainsKey(path))
                return;
            
            //_activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
            AddSceneInDictionary(path, _loadedScenes);
        }
        
        public void LoadExtraAdditiveScene(Scene scene)
        {
            //_activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(scene.path, LoadSceneMode.Additive);
            
            AddSceneInDictionary(scene, _additionalScenes);
        }

        public void LoadExtraAdditiveScene(string path)
        {
            if (!_allScenes.ContainsKey(path))
                return;
            
            //_activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
            
            AddSceneInDictionary(path, _additionalScenes);
        }

        public void LoadMultiSceneSetups(LoadSceneSetups loadSceneSetup, Dictionary<string, Scene> dictionaryToPutInNewScenes)
        {
            Debug.LogWarning("LOADMULTISCENESETUPS");
            _activeLoadSceneSetups = loadSceneSetup;

            //
            Dictionary<string, Scene> temp = new Dictionary<string, Scene>();

            for (int i = 0; i< loadSceneSetup.scenes.Length; i++)
            {
                string path = loadSceneSetup.scenes[i].path;

                if (dictionaryToPutInNewScenes.ContainsKey(path))
                {
                    temp.Add(path, dictionaryToPutInNewScenes[path]);
                    dictionaryToPutInNewScenes.Remove(path);
                    continue;
                }

                SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
                
                AddSceneInDictionary(SceneManager.GetSceneByPath(path), temp);
                
            }

            /*Scene[]*/ removeScenes = new Scene[dictionaryToPutInNewScenes.Count];
            dictionaryToPutInNewScenes.Values.CopyTo(removeScenes, 0);

            for (int i = 0; i < removeScenes.Length; i++)
            {
                UnloadExtraAdditiveScene(removeScenes[i]);
            }

            _loadedScenes = temp;

            /*foreach (SceneInformation s in loadSceneSetup.scenes)
            {
                if (dictionaryToPutInNewScenes.ContainsKey(s.path))
                    continue;

                SceneManager.LoadSceneAsync(s.path, LoadSceneMode.Additive);

                AddSceneInDictionary(SceneManager.GetSceneByPath(s.path), _loadedScenes);

            }*/
            

        }

        public void UnloadAdditiveScene(Scene scene)
        {
            SceneManager.UnloadSceneAsync(scene);

            RemoveSceneInDictionary(scene, _loadedScenes);
        }

        public void UnloadAdditiveScene(string path)
        {
            if (!_loadedScenes.ContainsKey(path))
                return;
            
            SceneManager.UnloadSceneAsync(path);

            RemoveSceneInDictionary(path, _loadedScenes);
        }
        
        public void UnloadExtraAdditiveScene(Scene scene)
        {
            SceneManager.UnloadSceneAsync(scene);

            RemoveSceneInDictionary(scene, _additionalScenes);
        }

        public void UnloadExtraAdditiveScene(string path)
        {
            if (!_additionalScenes.ContainsKey(path))
                return;
            
            SceneManager.UnloadSceneAsync(path);

            RemoveSceneInDictionary(path, _additionalScenes);
        }

        public void ToggleSceneLoadOrUnload(string path)
        {
            
            //bool isLoaded = _loadedScenes.ContainsKey(path);
            bool isLoadedAdditional = _additionalScenes.ContainsKey(path);

            /*switch (isLoaded)
            {
                case true:
                    UnloadAdditiveScene(path);
                    break;
                case false:
                    LoadSceneAdditive(path);
                    break;
            }*/

            switch (isLoadedAdditional)
            {
                case true:
                    
                    UnloadExtraAdditiveScene(path);
                    break;
                case false:
                    LoadExtraAdditiveScene(path);
                    break;
            }

        }

        #endregion

        #region Misc
        public void LoadSceneSetUp(SceneSetups setup)
        {
            Debug.LogWarning("OnNewSceneSetupLoad Fired");
            LoadSceneSetups newSceneSetup = null;

            Debug.LogWarning((int)setup);

            //if (!((int)setup < allSceneSetups.Length))
            //return;

            newSceneSetup = allSceneSetups[(int)setup];

            LoadMultiSceneSetups(newSceneSetup, _loadedScenes);

            _currentSceneSetUp = setup;
        }

        public void CloseGame()
        {
            Application.Quit();
        }
        #endregion

        public SceneSetups GetCurrentSceneSetup()
        {
            return _currentSceneSetUp;
        }

        #region Events

        async void SceneLoadedEvent(Scene scene, LoadSceneMode loadSceneMode)
        {
            await Task.Yield();
            
            if (loadSceneMode == LoadSceneMode.Single)
                return;
            
            if (_activeLoadSceneSetups == null)
                return;

            foreach (SceneInformation s in _activeLoadSceneSetups.scenes)
            {
                if (!scene.path.Equals(s.path))
                    continue;
                
                if (!s.isLoaded)
                {
                    UnloadExtraAdditiveScene(s.path);
                    continue;
                }

                if (!s.isActive) continue;
                
                SceneManager.SetActiveScene(_loadedScenes[s.path]);

            }
        }

        void SceneUnloadedEvent(Scene scene)
        {
            
        }

        void ActiveSceneChangedEvent(Scene previous, Scene active)
        {
            
        }

        

        #endregion

        #region Dictionary Methods

        void AddSceneInDictionary(Scene scene, Dictionary<string, Scene> dictionary)
        {
            if (dictionary.ContainsKey(scene.path))
                return;
            
            dictionary.Add(scene.path, scene);
        }

        void AddSceneInDictionary(string path, Dictionary<string, Scene> dictionary)
        {
            if (dictionary.ContainsKey(path))
                return;
            
            dictionary.Add(path, SceneManager.GetSceneByPath(path));
        }

        void RemoveSceneInDictionary(Scene scene, Dictionary<string, Scene> dictionary)
        {
            dictionary.Remove(scene.path);
        }

        void RemoveSceneInDictionary(string path, Dictionary<string, Scene> dictionary)
        {
            dictionary.Remove(path);
        }

        void RemoveAllScenesInDictionary(Dictionary<string, Scene> dictionary)
        {
            dictionary.Clear();
        }

        #endregion

    }

}
