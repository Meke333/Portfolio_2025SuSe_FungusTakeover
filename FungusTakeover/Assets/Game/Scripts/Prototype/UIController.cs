using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject _canvas;
    [SerializeField] GameObject _start;
    [SerializeField] GameObject _reload;
    [SerializeField] GameObject _txtLose;
    [SerializeField] GameObject _txtWin;

    private InputSystem_Actions input;

    private void OnEnable()
    {
        EntityTracker.OnPlayerDeath += LostGame;
        EntityTracker.OnAllEnemiesDeadWinning += WonGame;
        SceneManager.sceneLoaded += OnSceneLoaded;
        input.Enable();
        input.Player.DEBUG_UIPress.performed += context => OnKeyboardPress();
    }
    private void OnDisable()
    {
        EntityTracker.OnPlayerDeath -= LostGame;
        EntityTracker.OnAllEnemiesDeadWinning -= WonGame;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        input.Disable();
        input.Player.DEBUG_UIPress.performed -= context => OnKeyboardPress();
    }

    private void Awake()
    {
        input = new InputSystem_Actions();
        input.Enable();
    }

    private void Start()
    {
        ToggleElements(true, true,false,false,false);
    }
    private void OnKeyboardPress()
    {
        if (_start.activeInHierarchy)
            StartButton();
        else if (_reload.activeInHierarchy)
            RestartButton();
    }
    public void StartButton()
    {
        input.Disable();
        ToggleElements(false, true, false, false, false);
        EntityTracker._gameStarted = true;
    }
    public void RestartButton()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    private void LostGame()
    {
        input.Enable();
        ToggleElements(true, false,true,true,false);
    }
    private void WonGame()
    {
        input.Enable();
        ToggleElements(true, false,true,false,true);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EntityTracker.OnLoadScene(); //initialize entity tracker
    }

    private void ToggleElements(bool visible, bool start, bool reload, bool lost, bool win)
    {
        _canvas.SetActive(visible);
        _start.SetActive(start);
        _reload.SetActive(reload);
        _txtLose.SetActive(lost);
        _txtWin.SetActive(win);
    }
}
