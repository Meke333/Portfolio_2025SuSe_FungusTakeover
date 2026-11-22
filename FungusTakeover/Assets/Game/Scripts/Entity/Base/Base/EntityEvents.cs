using System;
using UnityEngine;


namespace Game.Entity.Events
{
    public class EntityEvents
    {
        public ResourceOutputEvents ResourceOutput { get; private set; } = new ResourceOutputEvents();
        public ResourceInputEvents ResourceInput { get; private set; } = new ResourceInputEvents();
        public EntityUnitEvents EntityUnit { get; private set; } = new EntityUnitEvents();
        public InputActionEvents InputActions { get; private set; } = new InputActionEvents();
        public StateEvents State { get; private set; } = new StateEvents();
        public AnimatorEvents Animator { get; private set; } = new AnimatorEvents();

    }

    /// <summary>
    /// EntityEvents for Resources that actually change
    /// </summary>
    public class ResourceOutputEvents
    {
        public Action<float> HealthPoints;
        public Action<int> SporePoints;
        public Action<Vector3> Knockback;
        public Action<Transform> Target;

        //Stats
        public Action<EntityContextType> EntityContext;
        public Action<EntityStateType> State;
        public Action<CharActions> StunActions;
        public Action<float> StunTimer;
        public Action<float> KnockbackTimer;

        public Action<float> Speed;
        public Action<float> Attack;
        public Action<float> Defense;

    }

    /// <summary>
    /// EntityEvents for Resources that is asked to be changed
    /// </summary>
    public class ResourceInputEvents
    {
        public Action<float> HealthPoints;
        public Action<int> SporePoints;
        public Action<Vector3> Knockback;
        public Action<Transform> Target;

        //Stats
        public Action<EntityContextType> EntityContext;
        public Action<EntityStateType> State;
        public Action<CharActions> StunActions;
        public Action<float> StunTimer;
        public Action<float> KnockbackTimer;

        public Action<float> Speed;
        public Action<float> Attack;
        public Action<float> Defense;
    }

    /// <summary>
    /// EntityEvents for Changes inside the Entity itself
    /// </summary>
    public class EntityUnitEvents
    {
        public Action ResetEntity;
        public Action TriggerAttack;
        public Action TriggerWindup;
        public Action AttackEnded;

        public Action OnTakenEntityDamage;
        public Action OnTakenEnvironmentDamage;

        public Action<Vector3> TransmitAttackData;
        public Action FinishTransmittingAttackData;

        public Action FinishDissolveShader;

        public Action EatAlly;
        public Action ConvertIntoAlly;
    }

    /// <summary>
    /// EntityEvents for Inputs
    /// </summary>
    public class InputActionEvents
    {
        public Action<Vector2> Moving;
        public Action<bool> Attacking;
        public Action<Vector2> AttackDirection;
        public Action<bool> ShootingSpore;
        public Action<Vector2> ShootSporeDirection;
        public Action<bool> CallingAllies;
        public Action<bool> ConsumeAllies;
    }

    /// <summary>
    /// EntityEvents for the StateMachine Changes
    /// </summary>
    public class StateEvents
    {
        public Action<EntityStateType> StateChanged;
    }

    /// <summary>
    /// EntityEvents for AnimatorChanges
    /// </summary>
    public class AnimatorEvents
    {
        public Action<string, float> ChangeFloatParameter;
        public Action<string, int> ChangeIntParameter;
        public Action<string, bool> ChangeBoolParameter;
        public Action<string> TriggerParameter;

        //if you have multiple layer, if not just place 0 there
        public Action<string, int> PlayAnimationImmediately; 
    }
}

