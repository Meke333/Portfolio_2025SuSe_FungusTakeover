using SceneLoader;
using UnityEngine.SceneManagement;

namespace Interface
{
    public interface ISceneLoader
    {
        public void LoadSceneSetUp(SceneSetups setups);
        public void CloseGame();
        public void LoadExtraAdditiveScene(Scene scene);
        public void LoadExtraAdditiveScene(string path);
        public void UnloadExtraAdditiveScene(Scene scene);
        public void UnloadExtraAdditiveScene(string path);
        public void ToggleSceneLoadOrUnload(string path);

        public SceneSetups GetCurrentSceneSetup();
    }
}

