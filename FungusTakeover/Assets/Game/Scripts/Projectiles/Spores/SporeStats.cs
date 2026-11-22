using Sirenix.OdinInspector;


namespace Game.Projectiles
{
    /// <summary>
    /// Since Upgrades could be a thing, we will put this as a separate class
    /// </summary>
    [System.Serializable]
    public class SporeStats
    {
        public float lifeTimeDuration;

        public bool canHurt;
        [ShowIf(nameof(canHurt), true)]
        [BoxGroup("CanHurt")] 
        public float damage;
    }
}

