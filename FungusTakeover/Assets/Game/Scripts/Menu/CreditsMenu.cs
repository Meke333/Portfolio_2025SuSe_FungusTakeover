using Events;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace Game.Menu
{
    public class CreditsMenu : MonoBehaviour
    {

        [Inject] private ProjectEvents _projectEvents;
        [SerializeField] private GameObject _creditsGameObject;

        [Header("UI")]
        [SerializeField] private Button _backButton;
                                        
        [Header("The Sufferers")]       
        [SerializeField] private Button _AleksaButton;     // = 0
        [SerializeField] private string _AlexaText;

        [SerializeField] private Button _AntheaButton;     // = 1 
        [SerializeField] private string _AntheaText; 

        [SerializeField] private Button _ChisakiButton;    // = 2
        [SerializeField] private string _ChisakiText; 

        [SerializeField] private Button _LiamButton;       // = 3
        [SerializeField] private string _LiamText;

        [SerializeField] private Button _MaxButton;        // = 4
        [SerializeField] private string _MaxText;

        [SerializeField] private Button _MikeButton;       // = 5
        [SerializeField] private string _MikeText;

        [SerializeField] private Button _NastjaButton;     // = 6
        [SerializeField] private string _NastjaText;

        [SerializeField] private Button _RinaButton;       // = 7
        [SerializeField] private string _RinaText;

        [SerializeField] private Button _SaskiaButton;     // = 8
        [SerializeField] private string _SaskiaText;

        [SerializeField] private Button _JulesButton;      // = 9
        [SerializeField] private string _JulesText;

        [SerializeField] private Button _LilithButton;     // = 10
        [SerializeField] private string _LilithText;

        [SerializeField] private Button _LizButton;        // = 11
        [SerializeField] private string _LizText;

        #region UnityMethods

        private void Awake()
        {
            _projectEvents.MainMenu.StateChanged += StateChanged;
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
            _projectEvents.MainMenu.StateChanged -= StateChanged;
        }

        #endregion

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _backButton.onClick.AddListener(BackButtonPressed);
                    _AleksaButton.onClick.AddListener(() => ButtonPressed(0));
                    _AntheaButton.onClick.AddListener(() => ButtonPressed(1));
                    _ChisakiButton.onClick.AddListener(() => ButtonPressed(2));
                    _LiamButton.onClick.AddListener(() => ButtonPressed(3));
                    _MaxButton.onClick.AddListener(() => ButtonPressed(4));
                    _MikeButton.onClick.AddListener(() => ButtonPressed(5));
                    _NastjaButton.onClick.AddListener(() => ButtonPressed(6));
                    _RinaButton.onClick.AddListener(() => ButtonPressed(7));
                    _SaskiaButton.onClick.AddListener(() => ButtonPressed(8));
                    _JulesButton.onClick.AddListener(() => ButtonPressed(9));
                    _LilithButton.onClick.AddListener(() => ButtonPressed(10));
                    _LizButton.onClick.AddListener(() => ButtonPressed(11));
                    _projectEvents.MainMenu.StateChanged += StateChanged;
                    break;
                case false:
                    _backButton.onClick.RemoveListener(BackButtonPressed);
                    _AleksaButton.onClick.RemoveListener(() => ButtonPressed(0));
                    _AntheaButton.onClick.RemoveListener(() => ButtonPressed(1));
                    _ChisakiButton.onClick.RemoveListener(() => ButtonPressed(2));
                    _LiamButton.onClick.RemoveListener(() => ButtonPressed(3));
                    _MaxButton.onClick.RemoveListener(() => ButtonPressed(4));
                    _MikeButton.onClick.RemoveListener(() => ButtonPressed(5));
                    _NastjaButton.onClick.RemoveListener(() => ButtonPressed(6));
                    _RinaButton.onClick.RemoveListener(() => ButtonPressed(7));
                    _SaskiaButton.onClick.RemoveListener(() => ButtonPressed(8));
                    _JulesButton.onClick.RemoveListener(() => ButtonPressed(9));
                    _LilithButton.onClick.RemoveListener(() => ButtonPressed(10));
                    _LizButton.onClick.RemoveListener(() => ButtonPressed(11));
                    _projectEvents.MainMenu.StateChanged -= StateChanged;
                    break;
            }
        }



        private void StateChanged(MainMenuStateType state)
        {
            
            _creditsGameObject.SetActive(state == MainMenuStateType.Credits);
            if (state == MainMenuStateType.Credits)
                _backButton.Select();
            
        }

        private void BackButtonPressed()
        {
            _projectEvents.MainMenu.StateChanged?.Invoke(MainMenuStateType.MainMenu);
            _creditsGameObject.SetActive(false);
        }

        private void ButtonPressed(int index)
        {
            string websiteURL = index switch
            {
                0   => _AlexaText,
                1   => _AntheaText,
                2   => _ChisakiText,
                3   => _LiamText,
                4   => _MaxText,
                5   => _MikeText,
                6   => _NastjaText,
                7   => _RinaText,
                8   => _SaskiaText,
                9   => _JulesText,
                10  => _LilithText,
                11  => _LizText
            };

            if (String.IsNullOrEmpty(websiteURL))
                return;

            Application.OpenURL(websiteURL);
        }


    }

}

