using Game.Entity.Events;
using UnityEngine;
using Zenject;

namespace Game.Entity
{
    public abstract class EntityInput_Base
    {
        [Inject] protected EntityEvents _events;
        [Inject] protected EntityManagers _entityManagers;
        public void Initialize(EntityInputData data)
        {
            ProcessMoveInput(data.moveInput);
            ProcessAttackInput(data.attackInput);
            ProcessAttackDirectionInput(data.attackDirection);
            ProcessShootSporeInput(data.shootSporeInput);
            ProcessShootSporeDirectionInput(data.shootSporeDirection);
            ProcessCallAlliesInput(data.callAlliesInput);
            ProcessConsumeAlliesInput(data.consumeAlliesInput);
        }

        /// <summary>
        /// Method for Processing the Move Input, Main Method for Moving Around Entities
        /// </summary>
        /// <param name="input">Normalized Input for Movement as Vector2</param>
        public virtual void ProcessMoveInput(Vector2 input)
        {
            _events.InputActions.Moving?.Invoke(input);
        }

        /// <summary>
        /// Method for Processing the Attack Input
        /// </summary>
        /// <param name="input">Bool: true if the Attack Button is pressed down</param>
        public virtual void ProcessAttackInput(bool input)
        {
            _events.InputActions.Attacking?.Invoke(input);
        }

        /// <summary>
        /// Method for Processing an Attack based on Mouse Position/Stick Rotation
        /// </summary>
        /// <param name="input">Raw Input for Mouse Position</param>
        public virtual void ProcessAttackDirectionInput(Vector2 input)
        {
            _events.InputActions.AttackDirection?.Invoke(input);
        }

        /// <summary>
        /// Method for Processing the Shoot Spore Input
        /// </summary>
        /// <param name="input">Bool: true if the Shoot Spore Button is pressed down</param>
        public virtual void ProcessShootSporeInput(bool input)
        {
            if (CheckPlayer(PlayerActions.SporeShooting))
                _events.InputActions.ShootingSpore?.Invoke(input);
        }

        /// <summary>
        /// Method for Processing Shooting a Spore based on Mouse Position/Stick Rotation
        /// </summary>
        /// <param name="input">Raw Input for Mouse Position</param>
        public virtual void ProcessShootSporeDirectionInput(Vector2 input)
        {
            if(CheckPlayer(PlayerActions.SporeShooting))
                _events.InputActions.ShootSporeDirection?.Invoke(input);
        }

        /// <summary>
        /// Method for Processing the Calling over Allies
        /// </summary>
        /// <param name="input">Bool: true if the Call Allies Button is pressed down</param>
        public virtual void ProcessCallAlliesInput(bool input)
        {
            if (CheckPlayer(PlayerActions.Formation))
                _events.InputActions.CallingAllies?.Invoke(input);
        }
        
        /// <summary>
        /// Method for Processing the Consuming
        /// </summary>
        /// <param name="input">Bool: true if the Consume Allies Button is pressed down</param>
        public virtual void ProcessConsumeAlliesInput(bool input)
        {
            if (CheckPlayer(PlayerActions.SpecialAttack))
                _events.InputActions.ConsumeAllies?.Invoke(input);
        }

        protected bool CheckPlayer(PlayerActions action)
        {
            if (_entityManagers == null)
                return false;
            if (_entityManagers.allyManager == null)
                return false;
            if (_entityManagers.allyManager.Player == null)
                return false;
            if (!_entityManagers.allyManager.Player.StatsConfiguration.Player.PlayerValueContainer.playerActions.HasFlag(PlayerActions.SpecialAttack))
                return false;
            return true;
        }
    }
}

