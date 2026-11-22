using Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class GameOverWinScreen : GameUIElement
    {
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private GameUIHolder _gameUIHolder;
        
        [SerializeField] private GameObject _gameWinScreen;
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private Button _mainMenuButton;
        

        #region UnityMethods

        private void Awake()
        {
            _gameUIHolder.gameOverWinScreen = this;
        }

        private void OnEnable()
        {
            ToggleEvents(true);
        }

        private void OnDisable()
        {
            ToggleEvents(false);
        }

        #endregion

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _mainMenuButton.onClick.AddListener(MainMenuButton);
                    _projectEvents.GameFlow.GameWin += ShowGameWinScreen;
                    _projectEvents.GameFlow.GameOver += ShowGameOverScreen;
                    break;
                case false:
                    _mainMenuButton.onClick.RemoveListener(MainMenuButton);
                    _projectEvents.GameFlow.GameWin -= ShowGameWinScreen;
                    _projectEvents.GameFlow.GameOver -= ShowGameOverScreen;
                    break;
            }
        }

        public void ShowGameOverScreen()
        {
            _gameOverScreen.SetActive(true);
            ToggleButton();
        }

        public void ShowGameWinScreen()
        {
            _gameWinScreen.SetActive(true);
            ToggleButton();
        }

        private void ToggleButton()
        {
            _mainMenuButton.gameObject.SetActive(true);
            _mainMenuButton.Select();
        }

        private void MainMenuButton()
        {
            _projectEvents.GameFlow.ReturnToMainMenu?.Invoke();
        }
    }
}