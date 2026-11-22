using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "PrototypeSettings_Scriptable", menuName = "ScriptableObjects/PrototypeSettings")]
public class PrototypeSettings_Scriptable : ScriptableObject
{
    [Header("Hover over variables to get an explanation!\nDropdowns can't have one sry.")]
    [HorizontalLine(color: EColor.Gray)]

    //Different Camera views
    [Dropdown("CameraView")]
    public string cameraView;
    private List<string> CameraView { get { return new List<string>() { "3rd Person", "Top-Down", "Static" }; } }

    //Different Stage types
    [Dropdown("ArenaType")]
    public string arenaType;
    private List<string> ArenaType { get { return new List<string>() { "Square", "Arena"}; } }

    //Calc formular for stats
    [Dropdown("ScaleType")]
    public string scaleType;
    private List<string> ScaleType { get { return new List<string>() { "%" }; } }

    [HorizontalLine(color: EColor.Gray)]
    [Header("Enemy Base Stats")]
    [Label("Enable Notice Range for targeting Allies")]
    [AllowNesting]
    public bool enableNoticeEnemy;
    [Label("Notice Range")]
    [ShowIf("enableNoticeEnemy")]
    [Tooltip("Distance at which enemies notice ally targets.")]
    [AllowNesting]
    public float noticeRadiusEnemy;
    [Label("Attack Range")]
    [Tooltip("Distance at which enemies start attacking targets (they don't waste their attack if they're too far away.)")]
    [AllowNesting]
    public float attackRadiusEnemy;
    [Label("Attack Degree")]
    [Tooltip("The degree of view")]
    [AllowNesting, Range(10f, 360.0f)]
    public float attackAngleEnemy;
    [Label("HP")]
    [AllowNesting]
    public int hpEnemy;
    [AllowNesting]
    [Label("ATK")]
    public int atkEnemy;
    [AllowNesting]
    [Label("Speed")]
    public float speedEnemy;
    [AllowNesting]
    [Label("Cooldown")]
    [Tooltip("Attack cooldown, this runs even if they're outside of attack range. So if the target runs away, the cooldown keeps running so they can attack when they're close enough again.")]
    public float cooldownEnemy;
    [AllowNesting]
    [Tooltip("How long the stun lasts."), Label("Stun Duration")]
    public float stunEnemy;
    [AllowNesting]
    [Label("Knockback force")]
    public float knockbackEnemy;

    [HorizontalLine(color: EColor.Gray)]

    [Header("Player Base Stats")]
    [Label("Spore amount")]
    public int sporeAmountPlayer;
    [Label("Spore range")]
    public int sporeRangePlayer;
    [Label("Spore speed")]
    public float sporeSpeedPlayer;
    [Label("Attack Range")]
    [AllowNesting]
    public float attackRadiusPlayer;
    [Label("Attack Degree")]
    [Tooltip("The degree of view")]
    [AllowNesting, Range(10f, 360.0f)]
    public float attackAnglePlayer;
    [Label("HP")]
    [AllowNesting]
    public int hpPlayer;
    [AllowNesting]
    [Label("ATK")]
    public int atkPlayer;
    [AllowNesting]
    [Label("Speed")]
    public float speedPlayer;
    [AllowNesting]
    [Label("Cooldown")]
    public float cooldownPlayer;
    [AllowNesting]
    [Tooltip("How long the stun lasts."), Label("Stun Duration")]
    public float stunPlayer;
    [EnumFlags]
    public CharActions stunFlagsPlayer;
    [AllowNesting]
    [Label("Knockback force")]
    public float knockbackPlayer;
    [AllowNesting]
    public float mycelCooldown;

    [HorizontalLine(color: EColor.Gray)]
    [AllowNesting]
    [Header("Misc")]
    public float rotationSpeed;
    [Tooltip("Doesn't work with enemy waves enabled, but works for the old setup.")]
    public float enemySpawnSpeed;
    [Tooltip("How long the formation call takes.")]
    public float callTime;
    [Tooltip("Speed multiplier during the call formation.")]
    public float callSpeedMultiplier;
    [Tooltip("Knockback speed.")]
    public float knockbackSpeed;
    [Tooltip("Is the player able to take over dead enemies by touching them?"), Label("Player: Touching Takeover")]
    public bool touchTakeoverPlayer;
    [Tooltip("Are allies able to take over dead enemies by touching them?"), Label("Allies: Touching Takeover")]
    public bool touchTakeoverAlly;
    [Tooltip("Can the player 'live' on after it dies by turning an ally into the new player?"), Label("Ally turns into Player when Player dies")]
    public bool noPlayerDeath;
    [Tooltip("Should the new ally NOT be able to shoot spores/call formation?"), Label("Ally can't use Spores/Formation")]
    public bool noPlayerFunctions;
    [Tooltip("Should a random ally get picked? (If not turned on, it instead picks the first ally.)")]
    public bool pickRandomAllyAsNewPlayer;
    public Material materialPlayer;
    public Material materialEnemyWasp;
    public Material materialAllyWasp;
    public Material materialEnemyAnt;
    public Material materialAllyAnt;
    public Material materialHurt;
    public Material materialAttack;
    public Material materialDead;
}
