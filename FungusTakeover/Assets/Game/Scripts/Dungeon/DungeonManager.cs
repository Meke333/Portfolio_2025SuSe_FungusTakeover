using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Events;
using Game.Entity;
using UnityEngine;
using Zenject;


namespace Game.Dungeon
{
    //This depends on how we will do it, but for MVP we only need one room
    public class DungeonManager : MonoBehaviour
    {
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private EntityManagers _entityManagers;


        [SerializeField] private DungeonRoom[] _dungeonRooms;
        [SerializeField, Tooltip("Place in the first Dungeon")] private DungeonRoom _currentRoom;

        private int _currentRoomIndex;
        
        #region UnityMethods

        async void Start()
        {
            await Task.Delay(300);
            
            _projectEvents.Dungeon.FinishedRoomTransition?.Invoke();
        }

        void OnEnable()
        {
            ToggleEvents(true);
        }

        void OnDisable()
        {
            ToggleEvents(false);
        }

        #endregion

        void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _projectEvents.Dungeon.MovedIntoNextRoom += SwitchDungeonRoom;
                    _projectEvents.Dungeon.OpenUpDoors += OpenDoors;
                    break;
                case false:
                    _projectEvents.Dungeon.MovedIntoNextRoom -= SwitchDungeonRoom;
                    _projectEvents.Dungeon.OpenUpDoors -= OpenDoors;
                    break;
            }
        }

        private void OpenDoors()
        {
            if (_currentRoomIndex + 1 >= _dungeonRooms.Length)
            {
                _projectEvents.GameFlow.GameWin?.Invoke();
                return;
            }
            
            _currentRoom.OpenDoor();
        }

        async void SwitchDungeonRoom(DungeonRoom newActiveRoom)
        {
            _currentRoomIndex++;

            _projectEvents.Dungeon.CurrentRoom?.Invoke(_currentRoomIndex);
            
            if (_currentRoomIndex > _dungeonRooms.Length)
            {
                _projectEvents.GameFlow.GameWin?.Invoke();
                return;
            }
            
            newActiveRoom.gameObject.SetActive(true);

            await TeleportPlayers(newActiveRoom);

            if (_currentRoom == newActiveRoom)
                return;
            
            _currentRoom.gameObject.SetActive(false);
            _currentRoom = newActiveRoom;

            await Task.Delay(300);
            _projectEvents.Dungeon.FinishedRoomTransition?.Invoke();
        }

        async Task TeleportPlayers(DungeonRoom newActiveRoom)
        {
            Debug.Log("Hello");
            _projectEvents.Dungeon.StartRoomTransition?.Invoke();

            await Task.Delay(325);
            
            Debug.Log("Bye");
            _entityManagers.allyManager.TeleportAllies(newActiveRoom.StartPoint);
        }

    }
}

