using Events;
using Interface;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static UnityEngine.UI.Toggle;


namespace Game.Menu
{
    public class MainMenuDisplay : MonoBehaviour
    {
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private ISceneLoader _sceneLoader;

        [SerializeField] private GameObject _mainMenuGameObject;

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _quitButton;



        #region UnityMethods

        public void Awake()
        {
            _projectEvents.MainMenu.StateChanged += MainMenuStateChanged;
        }

        public void OnEnable()
        {
            ToggleEvents(true);
        }

        private void Start()
        {
            _startButton.Select();
        }

        public void OnDisable()
        {
            ToggleEvents(false);
        }

        private void OnDestroy()
        {

            _projectEvents.MainMenu.StateChanged -= MainMenuStateChanged;
        }

        #endregion

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _startButton.onClick.AddListener(() => { ButtonsPressed(MainMenuStateType.Start); });
                    _optionsButton.onClick.AddListener(() => { ButtonsPressed(MainMenuStateType.Options); });
                    _creditsButton.onClick.AddListener(() => { ButtonsPressed(MainMenuStateType.Credits); });
                    _quitButton.onClick.AddListener(() => { ButtonsPressed(MainMenuStateType.Quit); });
                    _projectEvents.MainMenu.StateChanged += MainMenuStateChanged;
                    break;
                case false:
                    _startButton.onClick.RemoveListener(() => { ButtonsPressed(MainMenuStateType.Start); });
                    _optionsButton.onClick.RemoveListener(() => { ButtonsPressed(MainMenuStateType.Options); });
                    _creditsButton.onClick.RemoveListener(() => { ButtonsPressed(MainMenuStateType.Credits); });
                    _quitButton.onClick.RemoveListener(() => { ButtonsPressed(MainMenuStateType.Quit); });
                    _projectEvents.MainMenu.StateChanged -= MainMenuStateChanged;
                    break;
            }
        }

        private void ButtonsPressed(MainMenuStateType buttons)
        {

            //Debug.Log("ButtonsPressed: " + buttons);
            switch (buttons)
            {
                case MainMenuStateType.Start:
                    //_projectEvents.GameFlow.GameFlowChange?.Invoke(Game.GameFlow.GameFlowStateType.Gameplay);
                    _projectEvents.GameFlow.StartGame?.Invoke();

                    break;
                case MainMenuStateType.Options:
                    _mainMenuGameObject.SetActive(false);
                    break;
                case MainMenuStateType.Credits:
                    _mainMenuGameObject.SetActive(false);
                    break;
                case MainMenuStateType.Quit:
                    _sceneLoader.CloseGame();
                    break;
                default:
                    break;
            }

            _projectEvents.MainMenu.StateChanged?.Invoke(buttons);
        }

        private void MainMenuStateChanged(MainMenuStateType type)
        {
            switch (type)
            {
                case MainMenuStateType.MainMenu:
                    _mainMenuGameObject.SetActive(true);
                    _startButton.Select();
                    break;
                case MainMenuStateType.Start:
                    break;
                case MainMenuStateType.Options:
                case MainMenuStateType.Credits:
                    _mainMenuGameObject.SetActive(false);
                    break;
                case MainMenuStateType.Quit:
                    break;
            }
        }
    }

}

