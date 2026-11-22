using UnityEngine;
using System.Collections.Generic;
using Zenject;
using EditorAttributes;


public class EnemySpawner : MonoBehaviour
{
    //TODO: MAKE ENEMYSPAWNER LISTEN TO THE TELEPORT ACTION AND THEN SPAWN THE ENTITIES

    
    [Inject]
    private PrototypeSettingsList settings;
    [Inject] private SceneEvents sceneEvents;

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    
    public bool useRoomsInsteadWaves;
    public int currentRoom;
    public List<EnemyRoom> enemyRooms = new List<EnemyRoom>();

    [SerializeField, Tooltip("If enabled, takes position the gameobject is placed at, instead of a random spawn point.")]
    private bool _dontPickRandomSpawn = false;
    public bool useSpawnWaves;

    public List<EnemyWave> spawnWaves = new List<EnemyWave>();

    //keep this in for no waves
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    private int enemySpawnCount = 0;
    private float currentTimer = 0;
    private int currentWave = 0;
    private void Awake()
    {
        if (useSpawnWaves)
        {
            foreach (EnemyWave w in spawnWaves)
            {
                foreach (GameObject e in w.enemies)
                {
                    if (!_dontPickRandomSpawn)
                        SetRandomPos(e);
                    e.SetActive(false);
                }
            }
        }
        else if (useRoomsInsteadWaves)
        {
            EntityTracker.maxWaves = enemyRooms.Count;
            EntityTracker.useRooms = true;
        }
        else
        {
            foreach (GameObject e in enemies)
            {
                if (!_dontPickRandomSpawn)
                    SetRandomPos(e);
                e.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        EntityTracker.OnNextRoom += NextRoom;
    }

    private void OnDisable()
    {
        EntityTracker.OnNextRoom -= NextRoom;
    }



    private void SetRandomPos(GameObject e)
    {
        //get random position
        Vector3 spawnPos = Vector3.zero;
        if (spawnPoints.Count > 0)
        {
            int rand = Random.Range(0, spawnPoints.Count - 1);
            spawnPos = spawnPoints[rand].position;
        }
        e.transform.position = spawnPos;
    }

    private void Update()
    {
        if (EntityTracker._finishedSpawns || !EntityTracker._gameStarted)
            return;

        currentTimer += Time.deltaTime;
        if (useSpawnWaves)
        {
            if (currentTimer >= spawnWaves[currentWave].timeTillSpawn)
                WaveSpawn();
        }
        else if (useRoomsInsteadWaves)
        {
            if (currentTimer >= enemyRooms[currentRoom].waves[currentWave].timeTillSpawn)
                WaveSpawn(enemyRooms[currentRoom]);
        }
        else
        {
            if (currentTimer >= settings.setting.enemySpawnSpeed)
                Spawn();
        }
        
    }
    private void Spawn()
    {
        //set enemy from pool to active
        enemies[enemySpawnCount].SetActive(true);
        //set values for potential next spawn
        currentTimer = 0;
        enemySpawnCount++;
        //if all enemies have been spawned, finish spawn
        if (enemySpawnCount > enemies.Count - 1)
            EntityTracker._finishedSpawns = true;
    }
    private void WaveSpawn()
    {
        //set all enemies in wave to active
        foreach(GameObject e in spawnWaves[currentWave].enemies)
        {
            e.SetActive(true);
        }
        //set values for potential next spawn
        currentTimer = 0;
        currentWave++;
        //if all waves have been spawned, finish spawn
        if (currentWave == spawnWaves.Count)
            EntityTracker._finishedSpawns = true;
    }

    private void WaveSpawn(EnemyRoom room)
    {
        foreach (GameObject e in room.waves[currentWave].enemies)
        {
            e.SetActive(true);
        }
        //set values for potential next spawn
        currentTimer = 0;
        currentWave++;
        //if all waves have been spawned, finish spawn
        if (currentWave == room.waves.Count)
            EntityTracker._finishedSpawns = true;
    }

    private void NextRoom(int room)
    {
        currentRoom = room;
        currentWave = 0;
        EntityTracker._finishedSpawns = false;
    }

    /*private void SpawnRoom(int room)
    {
        if (room >= enemyRooms.Count)
            return;

        EntityTracker._finishedSpawns = false;

        EntityTracker.entities_enemies = new List<CharacterBase>();

        EnemyRoom currentRoom = enemyRooms[room];

        for (int i = 0; i < currentRoom.enemies.Count; i++)
        {
            foreach (GameObject e in enemyRooms[room].enemies)
            {
                currentRoom.enemies[i].SetActive(true);
                //EntityTracker.OnAddEnemy(currentRoom.enemies[i].GetComponent<CharacterBase>());
            }
        }

        EntityTracker._finishedSpawns = true;
    }*/
}
