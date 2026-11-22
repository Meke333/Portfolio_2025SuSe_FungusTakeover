using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Entity
{
    public class EntityInput : EntitySystem
    {
        [Inject] private IInstantiator _instantiator;
        [Inject] private ProjectEvents _projectEvents;

        [Inject] private EntityBase _base;

        private EntityInput_Base _brain;
        [SerializeField, ReadOnly] private EntityInputData _entityInputData = new EntityInputData();

        #region UnityMethod
        private void Awake()
        {
            _events.ResourceOutput.EntityContext += GetNewInputBrain;
        }

        private void Start()
        {
            GetNewInputBrain(_base.ContextType);
        }

        private void OnEnable()
        {
            ToggleEvents(true);
        }

        private void OnDisable()
        {
            ToggleEvents(false);
        }

        private void OnDestroy()
        {
            _events.ResourceOutput.EntityContext -= GetNewInputBrain;
        }
        #endregion

        #region EventMethods

        private void MoveAction(Vector2 input)
        {
            _entityInputData.moveInput = input;
            _brain.ProcessMoveInput(input);
        }

        private void AttackAction(bool input)
        {
            _entityInputData.attackInput = input;
            _brain.ProcessAttackInput(input);
        }

        private void AttackDirectionAction(Vector2 input)
        {
            _entityInputData.attackDirection = input;
            _brain.ProcessAttackDirectionInput(input);
        }

        private void ShootSporeAction(bool input)
        {
            _entityInputData.shootSporeInput = input;
            _brain.ProcessShootSporeInput(input);
        }

        private void ShootSporeDirectionAction(Vector2 input)
        {
            _entityInputData.shootSporeDirection = input;
            _brain.ProcessShootSporeDirectionInput(input);
        }

        private void CallAlliesAction(bool input)
        {
            _entityInputData.callAlliesInput = input;
            _brain.ProcessCallAlliesInput(input);
        }

        private void ConsumeAlliesAction(bool input)
        {
            _entityInputData.consumeAlliesInput = input;
            _brain.ProcessConsumeAlliesInput(input);
        }

        #endregion

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _projectEvents.Input.Gameplay.MoveAction += MoveAction;
                    _projectEvents.Input.Gameplay.AttackAction += AttackAction;
                    _projectEvents.Input.Gameplay.AttackDirectionAction += AttackDirectionAction;
                    _projectEvents.Input.Gameplay.ShootSporeAction += ShootSporeAction;
                    _projectEvents.Input.Gameplay.ShootSporeDirectionAction += ShootSporeDirectionAction;
                    _projectEvents.Input.Gameplay.CallAlliesAction += CallAlliesAction;
                    _projectEvents.Input.Gameplay.ConsumeAlliesAction += ConsumeAlliesAction;
                    break;
                case false:
                    _projectEvents.Input.Gameplay.MoveAction -= MoveAction;
                    _projectEvents.Input.Gameplay.AttackAction -= AttackAction;
                    _projectEvents.Input.Gameplay.AttackDirectionAction -= AttackDirectionAction;
                    _projectEvents.Input.Gameplay.ShootSporeAction -= ShootSporeAction;
                    _projectEvents.Input.Gameplay.ShootSporeDirectionAction -= ShootSporeDirectionAction;
                    _projectEvents.Input.Gameplay.CallAlliesAction -= CallAlliesAction;
                    _projectEvents.Input.Gameplay.ConsumeAlliesAction -= ConsumeAlliesAction;
                    break;
            }
        }


        private void GetNewInputBrain(EntityContextType type)
        {
            switch (type)
            {
                case EntityContextType.Player:
                    _brain = _instantiator.Instantiate<EntityInput_Player>();
                    break;
                case EntityContextType.Ally:
                    _brain = _instantiator.Instantiate<EntityInput_Ally>();
                    break;
                case EntityContextType.Enemy:
                    _brain = _instantiator.Instantiate<EntityInput_Enemy>();
                    break;
            }

            _brain.Initialize(_entityInputData);
        }
    }
}

