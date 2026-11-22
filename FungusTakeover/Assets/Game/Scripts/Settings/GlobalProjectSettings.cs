using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Game.Entity;

[CreateAssetMenu(fileName = "GlobalProjectSettings", menuName = "FungusTakeOver/GlobalProjectSettings")]
public class GlobalProjectSettings : ScriptableObject
{
    [Inject] EntityManagers _entityManagers;
    [field: SerializeField, PropertyRange(0.1, "MaxKnockbackTime")] public float KnockbackGlobalTime { get; private set; }
    public int MaxKnockbackTime = 5;

    [field: SerializeField, Tooltip("y = 0.99 is fast, 0.01 is slow, 0.0 is instant"), BoundedCurve(0, 0, 1, 0.99f)] 
    public AnimationCurve knockbackSpeed { get; private set; }

    [field: SerializeField, MinValue(0f)] public float poisonCooldown { get; private set; }
    [field: SerializeField, MinValue(0f)] public float callAlliesCooldown { get; private set; }

    [MinValue(0),InlineButton("SetSpores", "Set MaxSpores (During Runtime)")]
    public int StartSpores = 10;
    public void SetSpores()
    {
        if (_entityManagers == null)
            return;
         _entityManagers.allyManager.UpdateSporesFromSettings();
    }
}
