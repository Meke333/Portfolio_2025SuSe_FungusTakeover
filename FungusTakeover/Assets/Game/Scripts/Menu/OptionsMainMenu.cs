using UnityEngine;

namespace Game.Menu
{
    public class OptionsMainMenu : OptionsMenu
    {


        #region UnityMethods

        protected override void Awake()
        {
            _projectEvents.MainMenu.StateChanged += StateChanged;
            base.Awake();
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            _projectEvents.MainMenu.StateChanged -= StateChanged;
        }

        #endregion

        private void StateChanged(MainMenuStateType state)
        {
            _optionsMenuGameObject.SetActive(state == MainMenuStateType.Options);
            if (state == MainMenuStateType.Options)
                _mainBackButton.Select();
            
        
        }

        protected override void BackButtonPressed()
        {
            _projectEvents.MainMenu.StateChanged?.Invoke(MainMenuStateType.MainMenu);
            base.BackButtonPressed();
        }

    }

}

