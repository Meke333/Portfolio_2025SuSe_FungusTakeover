using UnityEngine;
using Zenject;

public class EnvironmentController : MonoBehaviour
{
    [Inject]
    private PrototypeSettingsList settings;

    [SerializeField] private GameObject arena;
    [SerializeField] private GameObject square;

    private void Start()
    {
        arena.SetActive(false);
        square.SetActive(false);

        switch (settings.setting.cameraView)
        {
            case "Arena":
                arena.SetActive(true);
                break;
            default:
                square.SetActive(true);
                break;
        }
    }
}
