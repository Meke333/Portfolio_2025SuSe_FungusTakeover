using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class EntityTracker
{
    //TODO: SET MAXWAVES EDIT ONGAMEWON() CALL IF CWAVE == MAXWAVE, EDIT IncreaseRoomCount()

    public static bool _gameStarted = false;
    public static bool _finishedSpawns = false;


    public static List<CharacterBase> entities_enemies = new List<CharacterBase>(); //enemies currently on field
    public static List<CharacterBase> entities_allies = new List<CharacterBase>(); //includes player
    public static CharacterBase entity_player; //just the player (for spores and stuff)

    //Events for win/lose/start(?)
    public static event Action OnAllyListChanged = delegate { };
    public static event Action OnAllEnemiesDeadRoom = delegate { };
    public static event Action OnAllEnemiesDeadWinning = delegate { };
    public static event Action OnPlayerDeath = delegate { };
    public static event Action OnDesignateNewPlayer = delegate { }; 
    public static event Action<int> OnNextRoom;
    //public static event Action<int> OnWterPhoneFinished = (musicNoteID) => { };

    private static int _currentWave = 0;
    public static int maxWaves;
    public static bool useRooms;

    public static void OnRemoveEnemy(CharacterBase e)
    {
        entities_enemies.Remove(e);
        if (!_finishedSpawns || entities_enemies.Count != 0)
            return;

        //Debug.Log(_currentWave + ";" + maxWaves);
        if (useRooms && _currentWave < maxWaves - 1)
        {
            OnAllEnemiesDeadRoom();
            return;
        }

        OnGameWon();
    }
    public static void OnAddEnemy(CharacterBase e)
    {
        entities_enemies.Add(e);
    }
    public static void OnRemoveAlly(CharacterBase e)
    {
        entities_allies.Remove(e);
        OnAllyListChanged();
    }
    public static void OnAddAlly(CharacterBase e)
    {
        entities_allies.Add(e);
        if (e._isPlayer)
            entity_player = e;
        OnAllyListChanged();
    }

    public static void OnTryGetNewNewPlayer(int _MaxSpores, int _CurrentSpores)
    {
        //check if there's still an ally in the list after the player removed itself
        if (entities_allies.Count > 0)
        {
            int allyNB = 0; //select the first ally by default
            if (entities_allies[allyNB].settings.setting.pickRandomAllyAsNewPlayer) //it it instead should pick a random one
                allyNB = UnityEngine.Random.Range(0, entities_allies.Count);
            //make first from list new ally
            entity_player = entities_allies[allyNB];
            entities_allies[allyNB].maxSpores = _MaxSpores;
            entities_allies[allyNB].spores = _CurrentSpores;
            entities_allies[allyNB]._isAllyPlayer = true;
            entities_allies[allyNB]._isPlayer = true;
            OnDesignateNewPlayer(); //fire event to update camera
        }
        else
            OnGameOver();
    }
    public static void OnGameOver()
    {
        _gameStarted = false;
        while (entities_allies.Count > 0)
        {
            CharacterBase tempEntity = entities_allies[0];
            entities_allies.RemoveAt(0);
            tempEntity.DestroyYourself();
        }
        OnPlayerDeath(); //fire death event so ui and enemies can grab it
    }
    public static void OnGameWon()
    {
        _gameStarted = false;
        OnAllEnemiesDeadWinning();
    }

    public static void OnLoadScene()
    {
        //Clear everything
        _gameStarted = false;
        _finishedSpawns = false;
        entities_enemies.Clear();
        entities_allies.Clear();
        entity_player = null;
    }

    public static void ChangeSporeCount(int amount)
    {
        if (entity_player == null)
        {
            //Debug.Log("No player detected, can't change spore amount");
            return;
        }
        entity_player.ChangeSporeCount(amount);
    }

    public static CharacterBase GetClosestAlly(Vector3 posEnemy, float notice)
    {
        float distance = 0;
        CharacterBase returnAlly = null;
        CharacterBase player = null;
        if (entities_allies.Count <= 0)
            return null;
        for (int x = 0; x < entities_allies.Count; x++)
        {
            CharacterBase e = entities_allies[x];
            if (e._isPlayer)
                player = e;
            float tempDistance;
            tempDistance = Vector3.Distance(posEnemy, e.transform.position);
            if (x == 0) //initialize if first loop 
            {
                distance = tempDistance;
                returnAlly = e;
            }
            else if (tempDistance < distance) //Check if allyX is closer than the one before
            {
                distance = tempDistance;
                returnAlly = e;
            }
        }
        //If the closest ally enemy isn't close enough to notice range or notice range is disabled, target player
        if (notice < distance || !returnAlly.settings.setting.enableNoticeEnemy)
            returnAlly = player;
        return returnAlly;
    }

    public static void IncreaseRoomCount()
    {
        _currentWave++;
        OnNextRoom?.Invoke(_currentWave);
    }
}
