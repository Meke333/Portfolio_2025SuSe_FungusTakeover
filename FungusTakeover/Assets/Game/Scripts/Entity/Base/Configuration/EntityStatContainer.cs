using Sirenix.OdinInspector;
using UnityEngine;
using Game.Environment;

namespace Game.Entity
{
    [System.Serializable]
    public class EntityStatContainer
    {
        [MinValue(0f)] public float healthPoints;
        [MinValue(0f)] public float attackStat;
        [MinValue(0f)] public float attackCooldown;
        [MinValue(0f)] public float attackWindUp;
        [MinValue(0f), HideInInspector] public float attackAngle;
        [MinValue(0f)] public float attackRange;
        [Tooltip("The range the attack collider gets thrown if using an animated attack type")] public float attackThrustRange;
        [MinValue(0f), Tooltip("If this is smaller than the cooldown, attack will get interrupted and not shoot the full range!")] public float attackThrustSeconds;
        [HideInInspector] public float defenseStat; //unused
        [MinValue(0f)] public float stunDuration;
        [EnumToggleButtons] public CharActions stunPossibleActions;
        public float knockbackForce;
        public bool useEntityKnockbackSpeed;
        [Tooltip("y = 0.99 is fast, 0.01 is slow, 0.0 is instant"), BoundedCurve(0,0,1,0.99f), ShowIf("useEntityKnockbackSpeed")]
        public AnimationCurve knockbackSpeed; //xmin,ymin,xmax,ymax
        [MinValue(0f)] public float speedStat;
        [MinValue(0f)] public float rotationSpeed;
        public int sporeCost = 1;

        [Title("Behaviour")]
        [EnumPaging, Tooltip("This will be expanded once code is implemented")] public EntityType entityType;
        [EnumPaging] public EntityMovementType movementBehaviourType;
        [Range(0.1f,100f), ShowIf("movementBehaviourType", EntityMovementType.EnemyWasp), Tooltip("The % of hp left on the player when wasp targets it")] public float playerHPLeftPercent;
        [EnumPaging] public EntityAttackType attackBehaviourType;
        [EnumToggleButtons] public EnvironmentEffectType affectedByEnvironment;
    }
}

