using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.InputHandler
{
    public class InputHandler : MonoBehaviour
    {
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private GlobalProjectSettings _projectSettings;
        [Inject] private Game.Entity.EntityManagers _entityManagers;

        private InputActionMap _inputActionMap_Gameplay;
        private InputActionMap _inputActionMap_Menu;

        private bool _isSubscribed;
        [SerializeField] private InputMapType _currentInputMap = InputMapType.Menu;

        //cooldown ally calling
        private float _allyCallingCooldown;

#if DEBUG || UNITY_EDITOR

        //Gameplay
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Gameplay")] private Vector2 gameplay_moveInput;
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Gameplay")] private bool gameplay_attackInput;
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Gameplay")] private Vector2 gameplay_attackDirectionInput;
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Gameplay")] private bool gameplay_shootSporeInput;
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Gameplay")] private Vector2 gameplay_shootSporeDirectionInput;
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Gameplay")] private bool gameplay_callAlliesInput;
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Gameplay")] private bool gameplay_consumeAlliesInput;
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Gameplay")] private bool gameplay_menuInput;

        //Menu
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Menu")] private Vector2 menu_moveInput;
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Menu")] private bool menu_selectInput;
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Menu")] private bool menu_backInput;
        [SerializeField, ReadOnly, BoxGroup("Debug: Input-Values: Menu")] private bool menu_menuInput;

#endif

        #region UnityMethods

        public void Awake()
        {
            FindActionMaps();

            SwitchActionMap(_currentInputMap);

            _projectEvents.Input.ToggleInputAction += TogglePlayerInput;
            _projectEvents.Input.SwitchActionMapAction += SwitchActionMap;
        }

        private void Update()
        {
            if (_allyCallingCooldown > 0)
            {
                _allyCallingCooldown -= Time.deltaTime;
            }
        }


        private void FindActionMaps()
        {
            _inputActionMap_Gameplay = InputSystem.actions.FindActionMap(Game.Constants.Input.ActionMap_Gameplay);
            _inputActionMap_Menu = InputSystem.actions.FindActionMap(Game.Constants.Input.ActionMap_Menu);
        }

        public void OnEnable()
        {
            ToggleActionSubscription(true);
        }

        public void OnDisable()
        {
            ToggleActionSubscription(false);
        }

        #endregion

        #region EventsMethods

        private void Gameplay_Move(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.MoveAction?.Invoke(context.ReadValue<Vector2>());

#if DEBUG || UNITY_EDITOR
            gameplay_moveInput = context.ReadValue<Vector2>();
#endif
        }

        private void Gameplay_Attack_started(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.AttackAction?.Invoke(true);

#if DEBUG || UNITY_EDITOR
            gameplay_attackInput = true;
#endif
        }
        private void Gameplay_Attack_cancelled(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.AttackAction?.Invoke(false);

#if DEBUG || UNITY_EDITOR
            gameplay_attackInput = false;
#endif
        }

        private void Gameplay_AttackDirection(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.AttackDirectionAction?.Invoke(context.ReadValue<Vector2>());

#if DEBUG || UNITY_EDITOR
            gameplay_attackDirectionInput = context.ReadValue<Vector2>();
#endif
        }

        private void Gameplay_ShootSpore_started(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.ShootSporeAction?.Invoke(true);

#if DEBUG || UNITY_EDITOR
            gameplay_shootSporeInput = true;
#endif
        }
        private void Gameplay_ShootSpore_cancelled(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.ShootSporeAction?.Invoke(false);

#if DEBUG || UNITY_EDITOR
            gameplay_shootSporeInput = false;
#endif
        }

        private void Gameplay_ShootSporeDirection(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.ShootSporeDirectionAction?.Invoke(context.ReadValue<Vector2>());

#if DEBUG || UNITY_EDITOR
            gameplay_shootSporeDirectionInput = context.ReadValue<Vector2>();
#endif
        }

        private void Gameplay_CallAllies_started(InputAction.CallbackContext context)
        {
            //limit event firing by cooldown and ally size
            if (_allyCallingCooldown <= 0 && _entityManagers.allyManager.AlliesList.Count > 1)
            {
                _projectEvents.Input.Gameplay.CallAlliesAction?.Invoke(true);
                _allyCallingCooldown = _projectSettings.callAlliesCooldown;
            }

#if DEBUG || UNITY_EDITOR
            gameplay_callAlliesInput = true;
#endif
        }
        private void Gameplay_CallAllies_cancelled(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.CallAlliesAction?.Invoke(false);

#if DEBUG || UNITY_EDITOR
            gameplay_callAlliesInput = false;
#endif
        }

        private void Gameplay_ConsumeAllies_started(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.ConsumeAlliesAction?.Invoke(true);

#if DEBUG || UNITY_EDITOR
            gameplay_consumeAlliesInput = true;
#endif
        }
        private void Gameplay_ConsumeAllies_cancelled(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.ConsumeAlliesAction?.Invoke(false);

#if DEBUG || UNITY_EDITOR
            gameplay_consumeAlliesInput = false;
#endif
        }

        private void Gameplay_MenuOpen_started(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.MenuOpenAction?.Invoke(true);

#if DEBUG || UNITY_EDITOR
            gameplay_menuInput = true;
#endif
        }
        private void Gameplay_MenuOpen_cancelled(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Gameplay.MenuOpenAction?.Invoke(false);

#if DEBUG || UNITY_EDITOR
            gameplay_menuInput = false;
#endif
        }

        private void Menu_Move(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Menu.MoveAction?.Invoke(context.ReadValue<Vector2>());

#if DEBUG || UNITY_EDITOR
            menu_moveInput = context.ReadValue<Vector2>();
#endif
        }

        private void Menu_Select_started(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Menu.SelectAction?.Invoke(true);

#if DEBUG || UNITY_EDITOR
            menu_selectInput = true;
#endif
        }
        private void Menu_Select_cancelled(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Menu.SelectAction?.Invoke(false);

#if DEBUG || UNITY_EDITOR
            menu_selectInput = false;
#endif
        }

        private void Menu_Back_started(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Menu.BackAction?.Invoke(true);

#if DEBUG || UNITY_EDITOR
            menu_backInput = true;
#endif
        }
        private void Menu_Back_cancelled(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Menu.BackAction?.Invoke(false);

#if DEBUG || UNITY_EDITOR
            menu_backInput = false;
#endif
        }

        private void Menu_Menu_started(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Menu.MenuExitAction?.Invoke(true);

#if DEBUG || UNITY_EDITOR
            menu_menuInput = true;
#endif
        }
        private void Menu_Menu_cancelled(InputAction.CallbackContext context)
        {
            _projectEvents.Input.Menu.MenuExitAction?.Invoke(false);

#if DEBUG || UNITY_EDITOR
            menu_menuInput = false;
#endif
        }


        #endregion


        #region Methods
        private void TogglePlayerInput(bool isPlayerInputActive)
        {
            ToggleActionSubscription(isPlayerInputActive);
        }
        private void SwitchActionMap(InputMapType type)
        {
            if (_inputActionMap_Gameplay == null || _inputActionMap_Menu == null)
            {
                Debug.LogWarning("InputHandler: This project still uses the old InputAsset: When in Production use the new one! (PlayerInputSystem)");
                return;
            }

            switch (type)
            {
                case InputMapType.Gameplay:
                    _inputActionMap_Gameplay.Enable();
                    _inputActionMap_Menu.Disable();
                    break;
                case InputMapType.Menu:
                    _inputActionMap_Gameplay.Disable();
                    _inputActionMap_Menu.Enable();
                    break;
            }
        }

        private void ToggleActionSubscription(bool shouldSubscribe)
        {
            if (_inputActionMap_Gameplay == null || _inputActionMap_Menu == null)
            {
                Debug.LogWarning("InputHandler: This project still uses the old InputAsset: When in Production use the new one! (PlayerInputSystem)");
                return;
            }

            if (shouldSubscribe == _isSubscribed)
                return;

            _isSubscribed = shouldSubscribe;

            switch (shouldSubscribe)
            {
                case true:

                    InputSystem.actions[Game.Constants.Input.Gameplay_Move].performed += Gameplay_Move;
                    InputSystem.actions[Game.Constants.Input.Gameplay_Move].canceled += Gameplay_Move;
                    InputSystem.actions[Game.Constants.Input.Gameplay_Attack].started += Gameplay_Attack_started;
                    InputSystem.actions[Game.Constants.Input.Gameplay_Attack].canceled += Gameplay_Attack_cancelled;
                    InputSystem.actions[Game.Constants.Input.Gameplay_AttackDirection].performed += Gameplay_AttackDirection;
                    InputSystem.actions[Game.Constants.Input.Gameplay_AttackDirection].canceled += Gameplay_AttackDirection;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ShootSpore].started += Gameplay_ShootSpore_started;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ShootSpore].canceled += Gameplay_ShootSpore_cancelled;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ShootSporeDirection].performed += Gameplay_ShootSporeDirection;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ShootSporeDirection].canceled += Gameplay_ShootSporeDirection;
                    InputSystem.actions[Game.Constants.Input.Gameplay_CallAllies].started += Gameplay_CallAllies_started;
                    InputSystem.actions[Game.Constants.Input.Gameplay_CallAllies].canceled += Gameplay_CallAllies_cancelled;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ConsumeAllies].started += Gameplay_ConsumeAllies_started;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ConsumeAllies].canceled += Gameplay_ConsumeAllies_cancelled;
                    InputSystem.actions[Game.Constants.Input.Gameplay_MenuOpen].performed += Gameplay_MenuOpen_started;
                    InputSystem.actions[Game.Constants.Input.Gameplay_MenuOpen].canceled += Gameplay_MenuOpen_cancelled;

                    InputSystem.actions[Game.Constants.Input.Menu_Move].performed += Menu_Move;
                    InputSystem.actions[Game.Constants.Input.Menu_Move].canceled += Menu_Move;
                    InputSystem.actions[Game.Constants.Input.Menu_Select].started += Menu_Select_started;
                    InputSystem.actions[Game.Constants.Input.Menu_Select].canceled += Menu_Select_cancelled;
                    InputSystem.actions[Game.Constants.Input.Menu_Back].started += Menu_Back_started;
                    InputSystem.actions[Game.Constants.Input.Menu_Back].canceled += Menu_Back_cancelled;
                    InputSystem.actions[Game.Constants.Input.Menu_Menu].started += Menu_Menu_started;
                    InputSystem.actions[Game.Constants.Input.Menu_Menu].canceled += Menu_Menu_cancelled;

                    break;
                case false:

                    InputSystem.actions[Game.Constants.Input.Gameplay_Move].performed -= Gameplay_Move;
                    InputSystem.actions[Game.Constants.Input.Gameplay_Move].canceled -= Gameplay_Move;
                    InputSystem.actions[Game.Constants.Input.Gameplay_Attack].started -= Gameplay_Attack_started;
                    InputSystem.actions[Game.Constants.Input.Gameplay_Attack].canceled -= Gameplay_Attack_cancelled;
                    InputSystem.actions[Game.Constants.Input.Gameplay_AttackDirection].performed -= Gameplay_AttackDirection;
                    InputSystem.actions[Game.Constants.Input.Gameplay_AttackDirection].canceled -= Gameplay_AttackDirection;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ShootSpore].started -= Gameplay_ShootSpore_started;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ShootSpore].canceled -= Gameplay_ShootSpore_cancelled;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ShootSporeDirection].performed -= Gameplay_ShootSporeDirection;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ShootSporeDirection].canceled -= Gameplay_ShootSporeDirection;
                    InputSystem.actions[Game.Constants.Input.Gameplay_CallAllies].started -= Gameplay_CallAllies_started;
                    InputSystem.actions[Game.Constants.Input.Gameplay_CallAllies].canceled -= Gameplay_CallAllies_cancelled;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ConsumeAllies].started -= Gameplay_ConsumeAllies_started;
                    InputSystem.actions[Game.Constants.Input.Gameplay_ConsumeAllies].canceled -= Gameplay_ConsumeAllies_cancelled;
                    InputSystem.actions[Game.Constants.Input.Gameplay_MenuOpen].performed -= Gameplay_MenuOpen_started;
                    InputSystem.actions[Game.Constants.Input.Gameplay_MenuOpen].canceled -= Gameplay_MenuOpen_cancelled;

                    InputSystem.actions[Game.Constants.Input.Menu_Move].performed -= Menu_Move;
                    InputSystem.actions[Game.Constants.Input.Menu_Move].canceled -= Menu_Move;
                    InputSystem.actions[Game.Constants.Input.Menu_Select].started -= Menu_Select_started;
                    InputSystem.actions[Game.Constants.Input.Menu_Select].canceled -= Menu_Select_cancelled;
                    InputSystem.actions[Game.Constants.Input.Menu_Back].started -= Menu_Back_started;
                    InputSystem.actions[Game.Constants.Input.Menu_Back].canceled -= Menu_Back_cancelled;
                    InputSystem.actions[Game.Constants.Input.Menu_Menu].started -= Menu_Menu_started;
                    InputSystem.actions[Game.Constants.Input.Menu_Menu].canceled -= Menu_Menu_cancelled;

                    break;
            }
        }



        #endregion

    }
}

