using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine.UI;
using TMPro;
public class OnlineUsertestingUI : MonoBehaviour
{
    //Inspector assigned refs
    [SerializeField]
    private PrototypeSettings_Scriptable _settings;
    [SerializeField]
    private CinemachineHardLockToTarget _cinemachineHardLockTarget;
    [SerializeField]
    private CinemachineTargetGroup _cinemachineTargetGroup;

    //Inspector assigned refs: sliders for initial value setting
    [FoldoutGroup("UI Internal References", true, nameof(_UIPanel),
        nameof(cameraDamping), nameof(speedPlayer), nameof(speedEntity),
        nameof(knockbackPlayer), nameof(knockbackEntity), nameof(noticeRange),
        nameof(rotationSpeed), nameof(formationCallSpeed), nameof(formationCallTime),
        nameof(cameraDamping_Text),
        nameof(speedPlayer_Text),
        nameof(speedEntity_Text),
        nameof(knockbackPlayer_Text),
        nameof(knockbackEntity_Text),
        nameof(noticeRange_Text),
        nameof(rotationSpeed_Text),
        nameof(formationCallSpeed_Text),
        nameof(formationCallTime_Text)
        )]
    [SerializeField] private Void sliderGroupHolder;

    [SerializeField, HideInInspector] private GameObject[] _UIPanel;
    [SerializeField, HideInInspector] private Slider cameraDamping;
    [SerializeField, HideInInspector] private Slider speedPlayer;
    [SerializeField, HideInInspector] private Slider speedEntity;
    [SerializeField, HideInInspector] private Slider knockbackPlayer;
    [SerializeField, HideInInspector] private Slider knockbackEntity;
    [SerializeField, HideInInspector] private Slider noticeRange;
    [SerializeField, HideInInspector] private Slider rotationSpeed;
    [SerializeField, HideInInspector] private Slider formationCallSpeed;
    [SerializeField, HideInInspector] private Slider formationCallTime;
    [SerializeField, HideInInspector] private TextMeshProUGUI cameraDamping_Text;
    [SerializeField, HideInInspector] private TextMeshProUGUI speedPlayer_Text;
    [SerializeField, HideInInspector] private TextMeshProUGUI speedEntity_Text;
    [SerializeField, HideInInspector] private TextMeshProUGUI knockbackPlayer_Text;
    [SerializeField, HideInInspector] private TextMeshProUGUI knockbackEntity_Text;
    [SerializeField, HideInInspector] private TextMeshProUGUI noticeRange_Text;
    [SerializeField, HideInInspector] private TextMeshProUGUI rotationSpeed_Text;
    [SerializeField, HideInInspector] private TextMeshProUGUI formationCallSpeed_Text;
    [SerializeField, HideInInspector] private TextMeshProUGUI formationCallTime_Text;

    //Internal refs
    private bool _isTrackingAllAllies;

    private void OnEnable()
    {
        EntityTracker.OnAllyListChanged += SetTargetsForCamera;
    }
    private void OnDisable()
    {
        EntityTracker.OnAllyListChanged -= SetTargetsForCamera;
    }
    private void Start()
    {
        //ToggleUI();
        ResumeGame();
        //plus target tracking not in settings yet so add that ??? else use bool, rn using bool
        cameraDamping.value = _cinemachineHardLockTarget.Damping;
        speedPlayer.value = _settings.speedPlayer;
        speedEntity.value = _settings.speedEnemy;
        knockbackPlayer.value = _settings.knockbackPlayer;
        knockbackEntity.value = _settings.knockbackEnemy;
        noticeRange.value = _settings.noticeRadiusEnemy;
        rotationSpeed.value = _settings.rotationSpeed;
        formationCallSpeed.value = _settings.callSpeedMultiplier;
        formationCallTime.value = _settings.callTime;

        cameraDamping_Text.text = cameraDamping.value.ToString();
        speedPlayer_Text.text = speedPlayer.value.ToString();
        speedEntity_Text.text = speedEntity.value.ToString();
        knockbackPlayer_Text.text = knockbackPlayer.value.ToString();
        knockbackEntity_Text.text = knockbackEntity.value.ToString();
        noticeRange_Text.text = noticeRange.value.ToString();
        rotationSpeed_Text.text = rotationSpeed.value.ToString();
        formationCallSpeed_Text.text = formationCallSpeed.value.ToString();
        formationCallTime_Text.text = formationCallTime.value.ToString();
    }
    public void ToggleUI()
    {
        if (_UIPanel[0].activeInHierarchy)
            ResumeGame();
        else
            PauseGame();
    }
    private void PauseGame()
    {
        foreach(GameObject g in _UIPanel)
        {
            g.SetActive(true);
        }
        Time.timeScale = 0f;
    }
    private void ResumeGame()
    {
        foreach (GameObject g in _UIPanel)
        {
            g.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    public void DampingSpeed(float value)
    {
        _cinemachineHardLockTarget.Damping = value;
        cameraDamping_Text.text = cameraDamping.value.ToString();
    }
    public void CameraTargetGroup(bool value)
    {
        _isTrackingAllAllies = value;
        SetTargetsForCamera();
    }
    private void SetTargetsForCamera()
    {
        List<CinemachineTargetGroup.Target> newTargetGroup = new List<CinemachineTargetGroup.Target>();
        
        if (_isTrackingAllAllies) //track all allies in list
        {
            foreach (CharacterBase charBase in EntityTracker.entities_allies)
            {
                CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target();
                target.Object = charBase.transform;
                target.Radius = 1;
                target.Weight = 1;
                newTargetGroup.Add(target);
            }
        }
        else //only track player
        {
            CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target();
            target.Object = EntityTracker.entity_player.transform;
            target.Radius = 1;
            target.Weight = 1;
            newTargetGroup.Add(target);
        }
        _cinemachineTargetGroup.Targets = newTargetGroup;
    }
    public void PlayerSpeed(float value)
    {
        _settings.speedPlayer = value;
        speedPlayer_Text.text = speedPlayer.value.ToString();
        UpdatePlayer();
    }
    public void EntitySpeed(float value)
    {
        _settings.speedEnemy = value;
        speedEntity_Text.text = speedEntity.value.ToString();
        UpdateAll();
    }
    public void RotationSpeed(float value)
    {
        _settings.rotationSpeed = value;
        rotationSpeed_Text.text = rotationSpeed.value.ToString();
    }
    public void PlayerKnockbackForce(float value)
    {
        _settings.knockbackPlayer = value;
        knockbackPlayer_Text.text = knockbackPlayer.value.ToString();
        UpdatePlayer();
    }
    public void EntityKnockbackForce(float value)
    {
        _settings.knockbackEnemy = value;
        knockbackEntity_Text.text = knockbackEntity.value.ToString();
        UpdateAll();
    }
    public void NoticeRange(float value)
    {
        _settings.noticeRadiusEnemy = value;
        noticeRange_Text.text = noticeRange.value.ToString();
        UpdateEnemies();
    }
    public void CallSpeed(float value)
    {
        _settings.callSpeedMultiplier = value;
        formationCallSpeed_Text.text = formationCallSpeed.value.ToString();
    }
    public void CallTime(float value)
    {
        _settings.callTime = value;
        formationCallTime_Text.text = formationCallTime.value.ToString();
    }

    private void UpdateAll() { UpdateAllies(); UpdateEnemies(); }
    private void UpdatePlayer() { EntityTracker.entity_player.ValueChange(); }
    private void UpdateAllies()
    {
        foreach(CharacterBase charBase in EntityTracker.entities_allies)
        {
            charBase.ValueChange();
        }
    }
    private void UpdateEnemies()
    {
        foreach(CharacterBase charBase in EntityTracker.entities_enemies)
        {
            charBase.ValueChange();
        }
    }
}
