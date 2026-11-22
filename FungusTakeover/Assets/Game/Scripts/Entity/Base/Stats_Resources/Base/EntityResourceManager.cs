using Game.ResourceElement;

namespace Game.Entity
{
    /// <summary>
    /// This class stores resources like 
    /// HP, Spores, State etc.
    /// </summary>
    [System.Serializable]
    public class EntityResourceManager
    {
        public EntityResourceHealth Health;
        public EntityResourceKnockback Knockback;
        public EntityResourceTarget Target;

        //Stats
        public EntityStatContext EntityContext;
        public EntityStatStates StateType;
        public EntityStatStunAction StunAction;
        public EntityStatStunTimer StunTimer;
        public EntityStatKnockbackTimer KnockbackTimer;

        public EntityStatSpeed Speed;
        public EntityStatAttack Attack;
        public EntityStatDefense Defense;

    }


}

