using Game.Dungeon;
using Game.Entity;
using Game.GameFlow;
using Game.InputHandler;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class ProjectEvents
    {
        public InputEvents Input { get; private set; } = new InputEvents();
        public PoolingEvents Pooling { get; private set; } = new PoolingEvents();
        public EntityEvents Entity { get; private set; } = new EntityEvents();
        public DungeonEvents Dungeon { get; private set; } = new DungeonEvents();
        public GameFlowEvents GameFlow { get; private set; } = new GameFlowEvents();
        public TimeEvents Time { get; private set; } = new TimeEvents();
        public MainMenuEvents MainMenu { get; private set; } = new MainMenuEvents();
    }

    public class EntityEvents
    {
        public Action<List<EntityBase>> OnAllyListChanged;
        public Action CurrentRoomCleared;
        public Action<EntityBase> OnPlayerChanged;

        public Action<int> SporeAmountChanged;
        public Action<int> SporeMaximumChanged;
        public Action<float> PlayerHealthChanged;

        public Action<EntityBase> AddNewAllyToManager;
        public Action<EntityBase> RemoveNewAllyInManager;
        public Action<EntityBase> PlayerDied;

        public Action<Transform> TeleportToPoint;

        public Action<int> UseSpores;
        public Action<int> GetSpores;
    }

    public class DungeonEvents
    {
        public Action OpenUpDoors;
        public Action<DungeonRoom> MovedIntoNextRoom;
        public Action<int> CurrentRoom;
        
        public Action StartRoomTransition;
        public Action FinishedRoomTransition;
        public Action<float> WavePercentageChanged; 
    }

    public class PoolingEvents
    {
        public Action DeactivateAllInteracttions;
    }

    public class InputEvents
    {
        public InputEvents_Gameplay Gameplay { get; private set; } = new InputEvents_Gameplay();
        public InputEvents_Menu Menu { get; private set; } = new InputEvents_Menu();


        public Action<bool> ToggleInputAction;
        public Action<InputMapType> SwitchActionMapAction;
    }

    public class InputEvents_Gameplay
    {
        public Action<Vector2> MoveAction;
        public Action<bool> AttackAction;
        public Action<Vector2> AttackDirectionAction; //triggers when mousebutton is clicked, it tracks position of the mouse cursor
        public Action<bool> ShootSporeAction;
        public Action<Vector2> ShootSporeDirectionAction; //triggers when mousebutton is clicked, it tracks position of the mouse cursor
        public Action<bool> CallAlliesAction;
        public Action<bool> ConsumeAlliesAction;
        public Action<bool> MenuOpenAction;
    }

    public class InputEvents_Menu
    {
        public Action<Vector2> MoveAction;
        public Action<bool> SelectAction;
        public Action<bool> BackAction;
        public Action<bool> MenuExitAction;
    }

    public class GameFlowEvents
    {
        public Action<GameFlowStateType> GameFlowChange;
        public Action OpenMenu;
        public Action CloseMenu; //PauseMenu, GameOverScreen, WinScreen (until now), MainMenu (Exit)
        public Action RoomFinished;
        public Action GameWin;
        public Action GameOver;
        public Action StartGame;
        public Action ReturnToMainMenu;
    }

    public class MainMenuEvents
    {
        public Action<MainMenuStateType> StateChanged;
    }

    public class TimeEvents
    {

    }




}
