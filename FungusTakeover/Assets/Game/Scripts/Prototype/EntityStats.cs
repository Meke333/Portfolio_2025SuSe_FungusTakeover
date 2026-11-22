using UnityEngine;
using NaughtyAttributes;


[CreateAssetMenu(fileName = "EntityStats", menuName = "ScriptableObjects/EntityStats")]
public class EntityStats : ScriptableObject
{
    [Header("Enemy -> 1 = 100%")]
    [AllowNesting, Tooltip("Multiplier for the base HP set in Settings."), Label("HP Modifier")]
    public float hpModifierEnemy = 1;
    [AllowNesting, Tooltip("Multiplier for the base ATK set in Settings."), Label("ATK Modifier")]
    public float atkModifierEnemy = 1;
    [AllowNesting, Tooltip("Multiplier for the base distance attacks can have set in Settings."), Label("ATK Radius Modifier")]
    public float atkRadiusModifierEnemy = 1;
    [Range(1f, 36f)]
    [AllowNesting, Tooltip("Multiplier for the base degree attacks can have set in Settings.\nBase Angle * This value (make sure it's not above 360!)\n(sry can't hide this if not needed)"), Label("ATK Degree Modifier")]
    public float atkAngleModifierEnemy = 1;
    [AllowNesting, Tooltip("Multiplier for the base Speed set in Settings."), Label("Speed Modifier")]
    public float speedModifierEnemy = 1;
    [AllowNesting, Tooltip("Multiplier for the base ATK Cooldown set in Settings."), Label("Cooldown Modifier")]
    public float cooldownModifierEnemy = 1;
    [AllowNesting, Tooltip("Multiplier for the base Notice Range set in Settings."), Label("Notice Range Modifier")]
    public float noticeModifierEnemy = 1;
    [AllowNesting, Tooltip("Multiplier for the base Stun Duration set in Settings."), Label("Stun Duration Modifier")]
    public float stunModifierEnemy;
    [AllowNesting, Tooltip("Multiple choice select for everything the stun blocks the hit ally/player from doing."), Label("Stun Blocked Actions")]
    public CharActions stunFlagsEnemy;
    [AllowNesting, Tooltip("Multiplier for the base Knockback Force set in Settings."), Label("Knockback Force Modifier")]
    public float knockbackModifierEnemy;

    [Header("Ally -> 1 = 100%")]
    [AllowNesting, Tooltip("Multiplier for the base HP set in Settings."), Label("HP Modifier")]
    public float hpModifierAlly = 1;
    [AllowNesting, Tooltip("Multiplier for the base ATK set in Settings."), Label("ATK Modifier")]
    public float atkModifierAlly = 1;
    [AllowNesting, Tooltip("Multiplier for the base distance attacks can have set in Settings."), Label("ATK Radius Modifier")]
    public float atkRadiusModifierAlly = 1;
    [Range(1f, 36f)]
    [AllowNesting, Tooltip("Multiplier for the base degree attacks can have set in Settings.\nBase Angle * This value (make sure it's not above 360!)\n(sry can't hide this if not needed)"), Label("ATK Degree Modifier")]
    public float atkAngleModifierAlly = 1;
    [AllowNesting, Tooltip("Multiplier for the base Speed set in Settings."), Label("Speed Modifier")]
    public float speedModifierAlly = 1;
    [AllowNesting, Tooltip("Multiplier for the base ATK Cooldown set in Settings."), Label("Cooldown Modifier")]
    public float cooldownModifierAlly = 1;
    [AllowNesting, Tooltip("Multiplier for the base Stun Duration set in Settings."), Label("Stun Duration Modifier")]
    public float stunModifierAlly;
    [AllowNesting, Tooltip("Multiple choice select for everything the stun blocks the hit enemy from doing."), Label("Stun Blocked Actions")]
    public CharActions stunFlagsAlly;
    [AllowNesting, Tooltip("Multiplier for the base Knockback Force set in Settings."), Label("Knockback Force Modifier")]
    public float knockbackModifierAlly;
}
