using UnityEngine;
using Zenject;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [Inject]
    private PrototypeSettingsList settings;

    [SerializeField] private CinemachineCamera thirdPerson;
    [SerializeField] private CinemachineCamera topDown;
    [SerializeField] private CinemachineCamera staticView;

    private void OnEnable()
    {
        EntityTracker.OnDesignateNewPlayer += UpdateTarget;
    }
    private void OnDisable()
    {
        EntityTracker.OnDesignateNewPlayer -= UpdateTarget;
    }
    private void UpdateTarget()
    {
        thirdPerson.Follow = EntityTracker.entity_player.transform;
        topDown.Follow = EntityTracker.entity_player.transform;
        staticView.Follow = EntityTracker.entity_player.transform;
    }

    private void Start()
    {
        thirdPerson.gameObject.SetActive(false);
        topDown.gameObject.SetActive(false);
        staticView.gameObject.SetActive(false);

        switch (settings.setting.cameraView)
        {
            case "Top-Down":
                topDown.gameObject.SetActive(true);
                break;
            case "Static":
                staticView.gameObject.SetActive(true);
                break;
            default:
                thirdPerson.gameObject.SetActive(true);
                break;
        }
    }
}
