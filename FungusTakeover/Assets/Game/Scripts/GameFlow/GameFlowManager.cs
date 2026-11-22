using Base;
using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Game.GameFlow.States;
using Helper;
using System;
using Interface;
using SceneLoader;

namespace Game.GameFlow
{
    public sealed class GameFlowManager : StatemachineBase<GameFlowStateBase, GameFlowStateData>
    {
        //Manages Main Menu, Gameplay, Pause Menu?, Losing, Winning
        //Timescale Manipulation

        [Inject] private SubClassHelper _helper;
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private ISceneLoader _sceneLoader;

#if UNITY_EDITOR
        [SerializeField, DisplayAsString(false), ReadOnly] private GameFlowStateType _stateType;

        [SerializeField, DisplayAsString(false), ReadOnly] private string _stateName;
#endif

        [Inject]
        public void Inject(GameFlowStateData data)
        {
            stateData = data;
        }

        #region UnityMethod
        private void Awake()
        {
            ToggleActivity(true);
            ToggleEvents(true);
        }

        private void Start()
        {
            GetInitialState();
        }

        protected override void Update()
        {
            base.Update();

        #if UNITY_EDITOR

            if (_stateType == stateData.currentType)
                return;
                
            _stateType = stateData.currentType;
            _stateName = currentState.ToString();

        #endif

        }

        private void OnDestroy()
        {
            ToggleActivity(false);
            ToggleEvents(false);
        }

#endregion

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe) 
            {
                case true:
                    _projectEvents.GameFlow.GameFlowChange += GameFlowChange;
                    _projectEvents.GameFlow.StartGame += () => { SceneSwitching(GameFlowStateType.Gameplay); };
                    _projectEvents.GameFlow.ReturnToMainMenu += () => { SceneSwitching(GameFlowStateType.MainMenu); };
                    break;
                case false:
                    _projectEvents.GameFlow.GameFlowChange -= GameFlowChange;
                    _projectEvents.GameFlow.StartGame -= () => { SceneSwitching(GameFlowStateType.Gameplay); };
                    _projectEvents.GameFlow.ReturnToMainMenu -= () => { SceneSwitching(GameFlowStateType.MainMenu); };
                    break;
            }
        }

        private void GameFlowChange(GameFlowStateType type)
        {

            switch (type)
            {
                case GameFlowStateType.MainMenu:
                    break;
                case GameFlowStateType.Gameplay:
                    break;
                case GameFlowStateType.PauseMenu:
                    break;
                case GameFlowStateType.GameOver:
                    break;
                case GameFlowStateType.GameWon:
                    break;
                default:
                    break;
            }
        }

        private void SceneSwitching(GameFlowStateType type)
        {
            SceneSetups newSetup = default; 
            
            switch (type)
            {
                case GameFlowStateType.MainMenu:
                    newSetup = SceneSetups.MainMenu;
                    break;
                case GameFlowStateType.Gameplay:
                    newSetup = SceneSetups.Gameplay;
                    break;
                default:
                    return;
            }

            _sceneLoader.LoadSceneSetUp(newSetup);
        }

        protected void GetInitialState()
        {
            //To-Do: Change out GameFlowState to Beginning of the Game (probably MainMenu)
            currentState = _helper.GetNewInputBrain<GameFlowState_MainMenu>();
            
        }
    }

    

}


