using System;

namespace Game.Entity
{
    public class EnemyEvents
    {
        //To-Do: Call this in EntityBase when Context is Enemy and they are dead!
        public Action<EntityBase> OnEnemyDead;
        public Action<EnemyWave> OnWaveCleared;
    }
}

