using UnityEngine;
using Zenject;
using NaughtyAttributes;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine.AI;
using System.Collections.Generic;


[System.Flags] public enum CharActions
{
    None = 0,
    Attack = 1 << 0,
    Rotate = 1 << 1,
    Spores = 1 << 2,
    Formation = 1 << 3,
    SpecialAttack = 1 << 4,
    Mycellium = 1 << 5
}
public class CharacterBase : MonoBehaviour
{
    [Inject]
    [HideInInspector] public PrototypeSettingsList settings;

    [Inject] private SceneEvents sceneEvents;

    private InputSystem_Actions input;

    [SerializeField, Tooltip("If enabled uses the old atk way, so just a line, cuz you need a min of 10 degrees or sth for it to work at all")]
    private bool _useOldAtk;

    [SerializeField, HideIf("_isPlayer"), Tooltip("If enabled uses the scriptable object preset instead of this gameObjects scripts stats."), AllowNesting]
    private bool _useStatPreset = false;

    [Expandable, HideIf("_isPlayer")]
    public EntityStats statPreset;

    [Header("Enemy -> 1 = 100%"), HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting, SerializeField]
    [Tooltip("Multiplier for the base HP set in Settings."),Label("HP Modifier")]
    private float hpModifierEnemy = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base ATK set in Settings."), Label("ATK Modifier")]
    private float atkModifierEnemy = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base distance attacks can have set in Settings."), Label("ATK Radius Modifier")]
    private float atkRadiusModifierEnemy = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), Range(1f,36f)]
    [Tooltip("Multiplier for the base degree attacks can have set in Settings.\nBase Angle * This value (make sure it's not above 360!)\n(sry can't hide this if not needed)"), Label("ATK Degree Modifier")]
    private float atkAngleModifierEnemy = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base Speed set in Settings."), Label("Speed Modifier")]
    private float speedModifierEnemy = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base ATK Cooldown set in Settings."), Label("Cooldown Modifier")]
    private float cooldownModifierEnemy = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base Notice Range set in Settings."), Label("Notice Range Modifier")]
    private float noticeModifierEnemy = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base Stun Duration set in Settings."), Label("Stun Duration Modifier")]
    private float stunModifierEnemy;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiple choice select for everything the stun blocks the hit ally/player from doing."), Label("Stun Blocked Actions")]
    private CharActions stunFlagsEnemy;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base Knockback Force set in Settings."), Label("Knockback Force Modifier")]
    private float knockbackModifierEnemy;

    [Header("Ally -> 1 = 100%")]
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base HP set in Settings."), Label("HP Modifier")]
    private float hpModifierAlly = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base ATK set in Settings."), Label("ATK Modifier")]
    private float atkModifierAlly = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base distance attacks can have set in Settings."), Label("ATK Radius Modifier")]
    private float atkRadiusModifierAlly = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), Range(1f, 36f)]
    [Tooltip("Multiplier for the base degree attacks can have set in Settings.\nBase Angle * This value (make sure it's not above 360!)\n(sry can't hide this if not needed)"), Label("ATK Degree Modifier")]
    private float atkAngleModifierAlly = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base Speed set in Settings."), Label("Speed Modifier")]
    private float speedModifierAlly = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base ATK Cooldown set in Settings."), Label("Cooldown Modifier")]
    private float cooldownModifierAlly = 1;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base Stun Duration set in Settings."), Label("Stun Duration Modifier")]
    private float stunModifierAlly;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiple choice select for everything the stun blocks the hit enemy from doing."), Label("Stun Blocked Actions")]
    private CharActions stunFlagsAlly;
    [SerializeField, HideIf(EConditionOperator.Or, "_useStatPreset", "_isPlayer"), AllowNesting]
    [Tooltip("Multiplier for the base Knockback Force set in Settings."), Label("Knockback Force Modifier")]
    private float knockbackModifierAlly;

    [Header("Live values to view during runtime")]
    [SerializeField, Tooltip("Current HP")] private float hp;
    [SerializeField, ReadOnly, Tooltip("Current Spores"), AllowNesting] public int spores;

    [Header("Toggle before playstart if at all, as this affects game logic behavior and stats!")]
    [SerializeField, Tooltip("If enabled, is part of player army.")] public bool _isCaptured;
    [SerializeField, Tooltip("If enabled, is player.")] public bool _isPlayer;
    [SerializeField, Tooltip("If enabled, is ally after it has been turned into player.")] public bool _isAllyPlayer;
    [Tooltip("If enemy is considered dead and thus capturable.")] public bool _isCapturable;

    //Other values to keep track of that do not need to be displayed
    private float maxHP;
    private float speed;
    [HideInInspector] public int maxSpores;
    private float atk;
    private float attackRange;
    private float atkAngle;
    private float cooldown;
    private float stun;
    private CharActions stunFlags;
    private float knockback;
    [HideInInspector] public float notice;
    private float timer = 0;
    private float formationTimer = 0;
    private float stunTimer = 0;
    private bool isPerformingFormation = false;
    private Vector3 knockbackDirection;
    private float knockbackTimer = 0;
    private float mycelTimer = 0;

    //Environment modifers
    private float envSpeedModifer = 1;
    private Vector3 envDirectionModifer = Vector3.zero;

    //Script references
    private CharacterController controller;
    private SporeBullet sporeBullet;
    [SerializeField] private SkinnedMeshRenderer m;
    [SerializeField] private float dissolveRate = 0.0125f;
    [SerializeField] private float refreshRate = 0.025f;
    [SerializeField] private bool _isWasp;
    private CharacterBase target;
    private LineRenderer lineRenderer;
    private NavMeshAgent agent;
    private CharActions stunFlagsENEMY;
    private Collider charCollider;
    private GimmickMycel mycel;

    //UI References
    [Header("Spore Count UI")]
    [SerializeField] private TextMeshProUGUI SporeCount;

    //Animator References
    [Header ("Animator")]
    [SerializeField] private Animator animator;


    //Feedbacks
    [Header("MMF Feedbacks")]
    [SerializeField] private MMProgressBar PlayerProgressBar;
    [SerializeField] private GameObject EnemyProgressBar;
    [SerializeField] private GameObject AllyProgressBar;

    [SerializeField] private MMFeedbacks attackFeedback;
    [SerializeField] private MMFeedbacks sporeFeedback;
    [SerializeField] private MMFeedbacks sporeDustFeedback;
    [SerializeField] private MMFeedbacks attackImpactFeedback;
    [SerializeField] private MMFeedbacks hitFeedback;
    [SerializeField] private MMFeedbacks overtakingFeedback;
    [SerializeField] private MMFeedbacks eatingAlliesFeedback;
    [SerializeField] private MMFeedbacks stunFeedback;
    [SerializeField] private MMFeedbacks callingAlliesFeedback;

    [SerializeField] private GameObject attackImpactPosition;

    [SerializeField] private MMHealthBar PlayerHealthBar;
    [SerializeField] private MMHealthBar AllyHealthBar;
    [SerializeField] private MMHealthBar EnemyHealthBar;

    //Mushrooms
    [Header("Ally Mushrooms")]
    [SerializeField] private GameObject mushroom0;
    [SerializeField] private GameObject mushroom1;
    [SerializeField] private GameObject mushroom2;
    [SerializeField] private GameObject mushroom3;
    [SerializeField] private GameObject mushroom4;
    [SerializeField] private GameObject mushroom5;
    [SerializeField] private GameObject mushroom6;
    [SerializeField] private GameObject mushroom7;
    [SerializeField] private GameObject mushroom8;
    [SerializeField] private GameObject mushroom9;
    [SerializeField] private GameObject mushroom10;
    [SerializeField] private GameObject mushroom11;
    [SerializeField] private GameObject mushroom12;
    [SerializeField] private GameObject mushroom13;
    [SerializeField] private GameObject mushroom14;


    public GameObject[] mushrooms;
    [SerializeField] private Material mushroomGrowShader;


    //pls change later this is horrible
    private void Awake()
    {
        input = new InputSystem_Actions();
        //m = this.gameObject.GetComponent<MeshRenderer>();
        sporeBullet = FindFirstObjectByType<SporeBullet>();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        controller = this.gameObject.GetComponent<CharacterController>();
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        if (agent != null)
            controller.enabled = false;
        charCollider = this.GetComponent<Collider>();
        mycel = FindFirstObjectByType<GimmickMycel>();
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.Attack.performed += context => OnAttack();
        input.Player.Spore.performed += context => OnSpore();
        input.Player.Formation.performed += context => OnFormation_AllyPlayerCheck();
        input.Player.SpecialAttack.performed += context => OnSpecialAttack();
        input.Player.Mycelium.performed += context => OnPlaceMycel();
        EntityTracker.OnAllEnemiesDeadWinning += DisableInput;
        sceneEvents.OnPlayerTeleportedToNewRoom += OnFormation;
    }
    private void OnDisable()
    {
        input.Disable();
        input.Player.Attack.performed -= context => OnAttack();
        input.Player.Spore.performed -= context => OnSpore();
        input.Player.Formation.performed -= context => OnFormation_AllyPlayerCheck();
        input.Player.SpecialAttack.performed -= context => OnSpecialAttack();
        input.Player.Mycelium.performed -= context => OnPlaceMycel();
        EntityTracker.OnAllEnemiesDeadWinning -= DisableInput;
        sceneEvents.OnPlayerTeleportedToNewRoom += OnFormation;
    }

    private void DisableInput()
    {
        input.Disable();
    }

    private void Start()
    {
        if (_isPlayer)
            _isCaptured = true;
        else
            _isCaptured = false;

        //put itself into the tracker list according to faction
        if (_isCaptured)
            EntityTracker.OnAddAlly(this);
        else
            EntityTracker.OnAddEnemy(this);

        ValueChange();

        mushrooms = new GameObject[15];

        int i = 0;
        while (i < mushrooms.Length)
        {
            mushrooms[i] = GetMushroomByIndex(i);
            i++;
        }
    }

    public void ValueChange()
    {
        if (_isPlayer) //Player stats
        {
            hp = settings.setting.hpPlayer;
            atk = settings.setting.atkPlayer;
            speed = settings.setting.speedPlayer;
            cooldown = settings.setting.cooldownPlayer;
            attackRange = settings.setting.attackRadiusPlayer;
            spores = settings.setting.sporeAmountPlayer;
            atkAngle = settings.setting.attackAnglePlayer;
            stun = settings.setting.stunPlayer;
            stunFlags = settings.setting.stunFlagsPlayer;
            knockback = settings.setting.knockbackPlayer;
        }
        else if (_isCaptured) //Ally stats
        {
            //check for scriptable object & override script vars
            if (_useStatPreset && statPreset != null)
            {
                hpModifierAlly = statPreset.hpModifierAlly;
                atkModifierAlly = statPreset.atkModifierAlly;
                speedModifierAlly = statPreset.speedModifierAlly;
                cooldownModifierAlly = statPreset.cooldownModifierAlly;
                atkRadiusModifierAlly = statPreset.atkRadiusModifierAlly;
                atkAngleModifierAlly = statPreset.atkAngleModifierAlly;
                stunModifierAlly = statPreset.stunModifierAlly;
                stunFlagsAlly = statPreset.stunFlagsAlly;
                knockbackModifierAlly = statPreset.knockbackModifierAlly;
            }
            hp = settings.setting.hpEnemy * hpModifierAlly;
            atk = settings.setting.atkEnemy * atkModifierAlly;
            speed = settings.setting.speedEnemy * speedModifierAlly;
            cooldown = settings.setting.cooldownEnemy * cooldownModifierAlly;
            attackRange = settings.setting.attackRadiusEnemy * atkRadiusModifierAlly;
            atkAngle = settings.setting.attackAngleEnemy * atkAngleModifierAlly;
            stun = settings.setting.stunEnemy * stunModifierAlly;
            stunFlags = stunFlagsAlly;
            knockback = settings.setting.knockbackEnemy * knockbackModifierAlly;
        }
        else //Enemy stats
        {
            //check for scriptable object & override script vars
            if (_useStatPreset && statPreset != null)
            {
                hpModifierEnemy = statPreset.hpModifierEnemy;
                atkModifierEnemy = statPreset.atkModifierEnemy;
                speedModifierEnemy = statPreset.speedModifierEnemy;
                cooldownModifierEnemy = statPreset.cooldownModifierEnemy;
                atkRadiusModifierEnemy = statPreset.atkRadiusModifierEnemy;
                atkAngleModifierEnemy = statPreset.atkAngleModifierEnemy;
                stunModifierEnemy = statPreset.stunModifierEnemy;
                stunFlagsEnemy = statPreset.stunFlagsEnemy;
                knockbackModifierEnemy = statPreset.knockbackModifierEnemy;
            }
            hp = settings.setting.hpEnemy * hpModifierEnemy;
            atk = settings.setting.atkEnemy * atkModifierEnemy;
            speed = settings.setting.speedEnemy * speedModifierEnemy;
            cooldown = settings.setting.cooldownEnemy * cooldownModifierEnemy;
            attackRange = settings.setting.attackRadiusEnemy * atkRadiusModifierEnemy;
            notice = settings.setting.noticeRadiusEnemy * noticeModifierEnemy;
            atkAngle = settings.setting.attackAngleEnemy * atkAngleModifierEnemy;
            stun = settings.setting.stunEnemy * stunModifierEnemy;
            stunFlags = stunFlagsEnemy;
            knockback = settings.setting.knockbackEnemy * knockbackModifierEnemy;
        }
        maxHP = hp;
        maxSpores = spores;
        //Set health bar
        if (PlayerHealthBar != null || AllyHealthBar != null || EnemyHealthBar != null)
        {
            //HealthBarUI.SetBar(maxHP, .0f, maxHP);

            if(PlayerHealthBar != null)
            {
                PlayerHealthBar.UpdateBar(maxHP, .0f, maxHP, true);
            }

            if(AllyHealthBar != null)
            {
                AllyHealthBar.UpdateBar(maxHP, .0f, maxHP, false);
            }

            if (EnemyHealthBar != null)
            {
                EnemyHealthBar.UpdateBar(maxHP, .0f, maxHP, true);
            }
        }

        if (SporeCount != null)
            SporeCount.text = maxSpores + " / " + maxSpores;

        //sporeCount.TextValueMultiplier(maxSpores);

        //Set line renderer points if one has been added
        if (lineRenderer != null)
        {
            Vector3 localForward = Vector3.forward;
            Vector3 localUp = Vector3.up;
            Vector3 posLeft = Quaternion.AngleAxis(atkAngle / 2.0f, localUp) * localForward;
            Vector3 posRight = Quaternion.AngleAxis(-(atkAngle / 2.0f), localUp) * localForward;
            Vector3 posMiddle = Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward;
            Vector3 posLeftMiddle = Quaternion.AngleAxis(atkAngle / 4.0f, localUp) * localForward;
            Vector3 posRightMiddle = Quaternion.AngleAxis(-(atkAngle / 4.0f), localUp) * localForward;
            lineRenderer.SetPosition(1, posRight * attackRange);
            lineRenderer.SetPosition(2, posRightMiddle * attackRange);
            lineRenderer.SetPosition(3, posMiddle * attackRange);
            lineRenderer.SetPosition(4, posLeftMiddle * attackRange);
            lineRenderer.SetPosition(5, posLeft * attackRange);
        }

        //Set agent values if usiing navmesh movement
        if (agent != null)
        {
            agent.speed = speed;
        }
    }

    private void Update()
    {
        if (controller.enabled) //old
            controller.Move(Vector3.down * 2 * Time.deltaTime);

        //edge case env interaction for already dead enemies
        if (_isCapturable)
        {
            if (controller.enabled && envDirectionModifer != Vector3.zero)
                    controller.Move(envDirectionModifer * Time.deltaTime * speed);
            else if (envDirectionModifer != Vector3.zero)
                this.transform.position += envDirectionModifer * speed * Time.deltaTime;
        }

        //exit early in case its a dead enemy or the game hasn't started yet
        if (_isCapturable || !EntityTracker._gameStarted)
            return;
        timer += Time.deltaTime;

        //Stun mechanic
        if (stunTimer > 0)
            stunTimer -= Time.deltaTime;

        //Knockback mechanic
        if (knockbackTimer > 0)
            knockbackTimer -= Time.deltaTime;

        //Mycel Cooldown
        if (mycelTimer > 0)
            mycelTimer -= Time.deltaTime;

        //if not own unit, enemy ai logic
        if (!_isCaptured)
        {
            EnemyAI_Movement(); //checks internally for stun logic

            if ((stunTimer > 0 && !stunFlagsENEMY.HasFlag(CharActions.Attack)) || stunTimer <= 0)
                EnemyAI_Attack();

            return;
        }

        //formation tracking stufffff
        formationTimer += Time.deltaTime;
        if (formationTimer >= settings.setting.callTime && isPerformingFormation) //if call time is over, re-enable input etc
        {
            isPerformingFormation = false;
            if (agent != null) //remove target + reset speed
            {
                agent.isStopped = true;
                agent.ResetPath();
                agent.speed = speed;
                agent.velocity = Vector3.zero;
            }
        }

        Vector2 rawInput = input.Player.Move.ReadValue<Vector2>();
        float movement = rawInput.magnitude;

        if (animator != null)
        {
            animator.SetFloat("Blend", movement);
        }

        //formation logic
        if (isPerformingFormation)
        {
            if (_isPlayer)
                return;

            if (controller.enabled)
            {
                if ((stunTimer > 0 && !stunFlagsENEMY.HasFlag(CharActions.Rotate)) || stunTimer <= 0)
                    this.transform.LookAt(EntityTracker.entity_player.transform);
                if (stunTimer <= 0)
                    controller.Move(transform.forward * Time.deltaTime * speed * settings.setting.callSpeedMultiplier);
                if (knockbackTimer > 0)
                    controller.Move(knockbackDirection / settings.setting.knockbackSpeed * Time.deltaTime);
            }
            else
            {
                target = EntityTracker.entity_player;
                NavMeshAgentWalk();
            }
            //exit early
            return;
        }

        //Knockback :sob: + environment 
        if (controller.enabled)
        {
            if (knockbackTimer > 0)
                controller.Move(knockbackDirection / settings.setting.knockbackSpeed * Time.deltaTime);
            if (envDirectionModifer != Vector3.zero)
                controller.Move(envDirectionModifer * Time.deltaTime * speed);
        }
        else
        {
            if (knockbackTimer > 0)
                this.transform.position += knockbackDirection / settings.setting.knockbackSpeed * Time.deltaTime;

            if (envDirectionModifer != Vector3.zero)
                this.transform.position += envDirectionModifer * speed * Time.deltaTime;
        }
    
        //Exit early if no input detected + stop agent
        if (rawInput == Vector2.zero)
        {
            if (!controller.enabled)
                agent.isStopped = true;
            return;
        }    

        Vector3 movementInput = new Vector3(rawInput.x, 0, rawInput.y);

        //Else calc movement stuff
        if (controller.enabled) //old
        {
            Quaternion movementRotation = Quaternion.LookRotation(movementInput);

            if (stunTimer <= 0) //movement
                controller.Move(movementInput * Time.deltaTime * speed);

            if ((stunTimer > 0 && !stunFlagsENEMY.HasFlag(CharActions.Rotate)) || stunTimer <= 0) //rotation
                this.transform.rotation = Quaternion.Slerp(transform.rotation, movementRotation, settings.setting.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //check if desired position produces valid movement
            Vector3 newPos = transform.position + movementInput * Time.deltaTime * speed * envSpeedModifer;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newPos, out hit, 3f, NavMesh.AllAreas))
            {
                //if movement would be valid
                if ((transform.position - hit.position).magnitude >= .02f)
                {
                    if (stunTimer <= 0) //movement
                        transform.position = new Vector3(hit.position.x, transform.position.y, hit.position.z);

                    Quaternion movementRotation = Quaternion.LookRotation(movementInput);
                    if ((stunTimer > 0 && !stunFlagsENEMY.HasFlag(CharActions.Rotate)) || stunTimer <= 0)
                        this.transform.rotation = Quaternion.Slerp(transform.rotation, movementRotation, settings.setting.rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void EnemyAI_Movement()
    {
        //grab target every couple update runs
        if (Time.frameCount % 10 == 0)
            target = EntityTracker.GetClosestAlly(this.transform.position, notice);
        //movement
        if (target == null)
            return;

        if (controller.enabled) //old
        {
            if ((stunTimer > 0 && !stunFlagsENEMY.HasFlag(CharActions.Rotate)) || stunTimer <= 0)
                this.transform.LookAt(target.transform);
            if (stunTimer <= 0)
                controller.Move(transform.forward * Time.deltaTime * speed);
            if (knockbackTimer > 0)
                controller.Move(knockbackDirection / settings.setting.knockbackSpeed * Time.deltaTime);
        } //navmeshagent
        else
            NavMeshAgentWalk();
    }
    private void NavMeshAgentWalk()
    {
        if (stunTimer > 0 && stunFlagsENEMY.HasFlag(CharActions.Rotate)) //cant rotate & stunned
            agent.angularSpeed = 0;
        else
            agent.angularSpeed = 999;

        agent.SetDestination(target.transform.position);

        if (stunTimer > 0) //stunned
            agent.velocity = Vector3.zero;

        if (knockbackTimer > 0)
            this.transform.position += knockbackDirection / settings.setting.knockbackSpeed * Time.deltaTime;

        if (envDirectionModifer != Vector3.zero)
            this.transform.position += envDirectionModifer * speed * Time.deltaTime;
    }
    private void EnemyAI_Attack()
    {
        //exit early if stunned and attack is disabled
        if (stunTimer > 0 && !stunFlagsENEMY.HasFlag(CharActions.Attack))
            return;
        //atkTM
        if (timer > cooldown && Vector3.Distance(this.transform.position, target.transform.position) <= attackRange)
        {
            //if (m != null)
                //StartCoroutine(MaterialFlash(true));
            timer = 0;
            AttackCheck();
        }
    }

    private void OnAttack()
    {
        if (!_isCaptured || isPerformingFormation || (stunTimer > 0 && stunFlags.HasFlag(CharActions.Attack)))
            return;

        if (timer > cooldown)
        {
            //if(m != null)
            //StartCoroutine(MaterialFlash(true));
            if (attackFeedback != null)
            {
                attackFeedback.PlayFeedbacks();
                if (animator != null)
                {
                    //animator.SetTrigger("isAttacking");
                    animator.SetTrigger("isAttack");
                }
            }
            timer = 0;
            AttackCheck();
        }
    }
    private void OnSpore()
    {
        if (!_isPlayer || isPerformingFormation || (stunTimer > 0 && stunFlagsENEMY.HasFlag(CharActions.Spores)))
            return;
        //check if playerAlly is not allowed to shoot spores if it's an AllyPlayer
        if (_isAllyPlayer && settings.setting.noPlayerFunctions)
            return;
        if (spores > 0 && !sporeBullet._isFiring)
        {
            sporeBullet.OnStartFire(this.transform, settings.setting.sporeSpeedPlayer, settings.setting.sporeRangePlayer);
            if (animator != null)
            {
                //animator.SetTrigger("isAttack");
                animator.SetTrigger("isSporeShoot");
                sporeFeedback.PlayFeedbacks();
                sporeDustFeedback.PlayFeedbacks();
            }    
        }
    }
    private void OnSpecialAttack()
    {
        if (!_isPlayer || isPerformingFormation || (stunTimer > 0 && stunFlagsENEMY.HasFlag(CharActions.SpecialAttack)))
            return;

        //check if playerAlly is not allowed to do player functions
        if (_isAllyPlayer && settings.setting.noPlayerFunctions)
            return;

        if (attackFeedback != null)
        {
            attackFeedback.PlayFeedbacks();
        }

        if (_useOldAtk) // Old hit check
        {
            if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, attackRange))
            {
                CharacterBase hitChar = hit.collider.GetComponent<CharacterBase>();
                if (hitChar == null)
                    return;
                StealHP(hitChar);
            }
            return;
        }
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            CharacterBase hitChar = hitCollider.GetComponent<CharacterBase>();
            if (hitChar == null) //not a char
                continue;
            //check if in range and angle,, ah math
            Vector3 vec = (hitChar.transform.position - this.transform.position).normalized;
            float angleToTarget = Vector3.Angle(this.transform.forward, vec);

            if (angleToTarget <= atkAngle / 2f) //within hittable range and not the player lol
                StealHP(hitChar);
        }
    }
    private void StealHP(CharacterBase hitChar)
    {
        if (hitChar._isCaptured && !hitChar._isPlayer) //if ally
        {
            //grab da hp and deal lethal damage
            ModifyHp(hitChar.hp);
            hitChar.ModifyHp(-hitChar.hp);
        }
    }
    private void AttackCheck()
    {
        if (_useOldAtk) // Old hit check
        {
            if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, attackRange))
            {
                CharacterBase hitChar = hit.collider.GetComponent<CharacterBase>();
                if (hitChar == null)
                    return;
                hitChar.OnHit(_isCaptured, atk, stun, knockback, stunFlags, this.transform.position);

                if (attackFeedback != null)
                {
                    attackFeedback.PlayFeedbacks();
                }
            }
            return;
        }
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, attackRange);
        bool hasHitted = false;
        foreach (var hitCollider in hitColliders)
        {
            CharacterBase hitChar = hitCollider.GetComponent<CharacterBase>();
            if (hitChar == null) //not a char
                continue;
            //check if in range and angle,, ah math
            Vector3 vec = (hitChar.transform.position - this.transform.position).normalized;
            float angleToTarget = Vector3.Angle(this.transform.forward, vec);

            if (angleToTarget <= atkAngle / 2f) //within hittable range
            {
                hitChar.OnHit(_isCaptured, atk, stun, knockback, stunFlags, this.transform.position); //hit char lol
                hasHitted = true;
            }
        }
        if (hasHitted && attackFeedback != null)
        {
            attackFeedback.PlayFeedbacks();
            if (animator != null)
            {
                //animator.SetTrigger("isAttacking");
                animator.SetTrigger("isAttack");
            }
        }
    }
    public void OnHit(bool player, float dmg, float stun, float knockback, CharActions stunFlagsPassover, Vector3 atkpos)
    {
        //calculate knockback direction
        if (knockback > 0)
        {
            knockbackDirection = (this.transform.position - atkpos).normalized * knockback;
        }

        if (player && !_isCaptured) //enemy hit by player army
        {
            //if(m != null)
            //StartCoroutine(MaterialFlash(false));
            hp -= dmg;

            if(EnemyHealthBar != null)
            {
                if (hp >= 0)
                {
                    EnemyHealthBar.UpdateBar(hp, .0f, maxHP, true);
                    float damageAmount = hp / maxHP;
                    m.material.SetFloat("_DamageAmount", (1 -damageAmount));
                    attackImpactPosition.transform.position = this.transform.position;
                    attackImpactFeedback.PlayFeedbacks();
                }
            }
            if (animator != null)
            {
                animator.SetTrigger("isTakingDamage");
            }

            if (hp <= 0)
            {
                if (agent != null)
                {
                    agent.isStopped = true;
                    agent.ResetPath();
                    agent.velocity = Vector3.zero;
                }
                animator.SetFloat("DeathMultiplier", 1);
                //if(m != null)
                //m.material = settings.setting.materialDead;

                m.material.SetFloat("_DamageAmount", 0f);
                m.material.SetFloat("_Saturation", 0.8f);

                _isCapturable = true;
                if (animator != null)
                {
                    animator.SetTrigger("isDead");
                }
                EntityTracker.OnRemoveEnemy(this);
                return;
            }
            //Start stun
            stunFlagsENEMY = stunFlagsPassover;
            stunTimer = stun;
            if (agent != null && stun > 0)
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                if(stunFeedback != null)
                {
                    stunFeedback.PlayFeedbacks();
                }
            }
            //Start knockback
            knockbackTimer = settings.setting.knockbackSpeed;
        }
        else if (!player && _isCaptured) //player army hit by enemy
        {
            if (hitFeedback != null && _isPlayer)
            {
                hitFeedback.PlayFeedbacks();
                if (attackImpactFeedback != null)
                {
                    attackImpactPosition.transform.position = this.transform.position;
                    attackImpactFeedback.PlayFeedbacks();
                }
            }

            if(animator != null)
            {
                animator.SetTrigger("isTakingDamage");
            }
            
            //if(m!= null)
            //StartCoroutine(MaterialFlash(false));

            hp -= dmg;

            if (PlayerHealthBar != null && _isPlayer)
            {
                if (hp > 0)
                {
                    PlayerHealthBar.UpdateBar(hp, .0f, maxHP, true);
                    attackImpactPosition.transform.position = this.transform.position;
                    float damageAmount = hp / maxHP;
                    m.material.SetFloat("_DamageAmount", (1 - damageAmount));
                }
            }
            else if (AllyHealthBar != null)
            {
                if (hp > 0)
                {
                    AllyHealthBar.UpdateBar(hp, .0f, maxHP, true);
                    float damageAmount = hp / maxHP;
                    m.material.SetFloat("_DamageAmount", (1 - damageAmount));
                }
            }

            if (hp <= 0) //remove itself out of the list + destroy it; if player make a new ally the new player
            {
                EntityTracker.OnRemoveAlly(this);
                if (_isPlayer)
                {
                    if (animator != null)
                    {
                        animator.SetFloat("DeathMultiplier", 1);
                        animator.SetTrigger("isDead");
                        StartCoroutine(DissolveCo());
                    }
                    if (settings.setting.noPlayerDeath) //if turning an ally into a new player is allowed
                        EntityTracker.OnTryGetNewNewPlayer(maxSpores, spores); //pass along spores and max spores to new "player"
                    else //else instant game over
                        EntityTracker.OnGameOver();
                }
                else //give back one spore
                    EntityTracker.ChangeSporeCount(+1);
                if (animator != null)
                {
                    animator.SetFloat("DeathMultiplier", 1);
                    animator.SetTrigger("isDead");
                    StartCoroutine(DissolveCo());
                }
                Destroy(this.gameObject);
            }
            //Start stun
            stunFlagsENEMY = stunFlagsPassover;
            stunTimer = stun;
            if (agent != null && stun > 0)
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                if (stunFeedback != null)
                {
                    stunFeedback.PlayFeedbacks();
                }
            }
            //Start knockback
            knockbackTimer = settings.setting.knockbackSpeed;
        }
    }
    public void InitializeTakeover()
    {
        _isCapturable = false;
        _isCaptured = true;
        ValueChange();
        if(m != null) { 
        if (_isWasp)
        {
            m.material = settings.setting.materialAllyWasp;
        }
        else
        {
            m.material = settings.setting.materialAllyAnt;
        }
        }
        EntityTracker.OnAddAlly(this);
        input.Enable();
        if (animator != null)
        {
            animator.SetTrigger("isTakenOver");
        }
        overtakingFeedback.PlayFeedbacks();
        if (agent != null)
            agent.isStopped = false;

        if (EnemyProgressBar != null && AllyProgressBar != null)
        {
            EnemyProgressBar.SetActive(false);
            AllyHealthBar.UpdateBar(maxHP, .0f, maxHP, true);
            AllyProgressBar.SetActive(true);
        }

        ActivateRandomMushrooms(5);
    }
    IEnumerator MaterialFlash(bool isAttacking)
    {
        if (!isAttacking)
            m.material = settings.setting.materialHurt;
        else
            m.material = settings.setting.materialAttack;
        yield return new WaitForSeconds(0.25f);
        if (_isPlayer)
            m.material = settings.setting.materialPlayer;
        else if (_isCaptured) {
            if (_isWasp) {
                m.material = settings.setting.materialAllyWasp;
            }
            else
            {
                m.material = settings.setting.materialAllyAnt;
            } 
        }
        else if (_isCapturable)
        {
            //m.material = settings.setting.materialDead;
        } //ded
            
        else {
            if (_isWasp)
            {
                m.material = settings.setting.materialEnemyWasp;
            }
            else
            {
                m.material = settings.setting.materialEnemyAnt;
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    public void ChangeSporeCount(int amount)
    {
        spores += amount;
        if (SporeCount != null)
        SporeCount.text = spores + " / " + maxSpores;
    }

    /* this gets called from input on every ally that isnt the player
     * UNLESS THE MAIN PLAYER IS AN ALLY PLAYER!
     * so its not called from the player itself
     */
    private void OnFormation_AllyPlayerCheck()
    {
        //check if allies shouldn't react if it's an AllyPlayer
        if (EntityTracker.entity_player._isAllyPlayer && settings.setting.noPlayerFunctions)
            return;
        OnFormation();
    }
    public void OnFormation()
    {
        if (isPerformingFormation || !_isCaptured || (stunTimer > 0 && stunFlagsENEMY.HasFlag(CharActions.Formation)))
            return;

        isPerformingFormation = true;
        formationTimer = 0;
        if (animator != null)
        {
            animator.SetTrigger("isCallingAllies");
        }
        if (callingAlliesFeedback != null)
        {
            callingAlliesFeedback.PlayFeedbacks();
        }
        if (agent != null)
        {
            agent.isStopped = false;
            agent.speed = speed * settings.setting.callSpeedMultiplier;
        }
    }
    private void OnPlaceMycel()
    {
        if (!_isPlayer || isPerformingFormation || (stunTimer > 0 && stunFlagsENEMY.HasFlag(CharActions.Mycellium)))
            return;

        //check if playerAlly is not allowed to do player functions
        if (_isAllyPlayer && settings.setting.noPlayerFunctions)
            return;

        //check if mycel cooldown timer is over
        if (mycelTimer <= 0)
        {
            mycelTimer = settings.setting.mycelCooldown; //reset cooldown
            mycel.transform.position = this.transform.position; //place at this position
            mycel.transform.rotation = this.transform.rotation; //place with this rotation
            mycel.EnableMycel(); //tell mycel to enable itself in general, just for the the case it's never been placed
        }
    }

    //Environment gimmicks
    public void ModifySpeed(float additionalSpeedModifer)
    {
        if (_isCapturable) //ded enemy
            return;

        envSpeedModifer = additionalSpeedModifer;
        if (agent != null)
            agent.speed = speed * envSpeedModifer;
    }
    public void ModifyDirection(Vector3 direction)
    {
        envDirectionModifer = direction; 
    }
    public void ModifyHp(float hpModify)
    {
        if (_isCapturable) //ded enemy
            return;

        if (hpModify < 0) //like getting hit
        {
            OnHit(!_isCaptured, -hpModify, 0, 0, new CharActions(), this.transform.position);
            return;
        }
        //else heal char
        hp += hpModify;
        hp = Mathf.Clamp(hp,0,maxHP);

        if (animator != null)
        {
            //animator.SetTrigger("isAttacking");
            //animator.SetTrigger("isEatingAllies");
        }

        if (!_isCaptured) //enemy
            EnemyHealthBar.UpdateBar(hp, .0f, maxHP, true);
        else if (_isPlayer)
        { //first player
            PlayerHealthBar.UpdateBar(hp, .0f, maxHP, true);
            if (eatingAlliesFeedback != null)
            {
                eatingAlliesFeedback.PlayFeedbacks();
            }
            if (animator != null)
            {
                //animator.SetTrigger("isAttack");
                animator.SetTrigger("isEatingAllies");
            }
        }
        else if (_isCaptured || _isAllyPlayer) //temp solution, ally or allyPlayer
            AllyHealthBar.UpdateBar(hp, .0f, maxHP, true);
    }

    //kys
    public void DestroyYourself()
    {
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

    //Touch takeover collider functions
    public void TriggerTakeoverCheck(Collider collision)
    {
        CharacterBase touchedChar = collision.gameObject.GetComponent<CharacterBase>();
        if (touchedChar == null) //not a char
            return;
        else if (!touchedChar._isCapturable || !_isCaptured) //if not dead; or if not touched by player/ally
            return;
        else if (EntityTracker.entity_player.spores < 1) //if player doesn't have enough spores lol
            return;

        if (settings.setting.touchTakeoverPlayer && _isPlayer) //player can touch takeover & is player
        {
            EntityTracker.ChangeSporeCount(-1);
            touchedChar.InitializeTakeover();
        }
        if (settings.setting.touchTakeoverAlly && (!_isPlayer || _isAllyPlayer)) //ally can touch takeover & is ally
        {
            EntityTracker.ChangeSporeCount(-1);
            touchedChar.InitializeTakeover();
        }
        if (animator != null)
        {
            //animator.SetBool("takeOver", false);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        TriggerTakeoverCheck(other);
    }
    public void OnTriggerStay(Collider other)
    {
        TriggerTakeoverCheck(other);
    }

    public GameObject GetMushroomByIndex(int index)
    {
        switch (index)
        {
            case 0: return mushroom0;
            case 1: return mushroom1;
            case 2: return mushroom2;
            case 3: return mushroom3;
            case 4: return mushroom4;
            case 5: return mushroom5;
            case 6: return mushroom6;
            case 7: return mushroom7;
            case 8: return mushroom8;
            case 9: return mushroom9;
            case 10: return mushroom10;
            case 11: return mushroom11;
            case 12: return mushroom12;
            case 13: return mushroom13;
            case 14: return mushroom14;
            default: return null;
        }
    }

    void ActivateRandomMushrooms(int count)
    {
        // Sicherstellen, dass genug Pilze vorhanden sind
        if (count > mushrooms.Length)
        {
            //Debug.LogWarning("Nicht genug Pilze im Array!");
            return;
        }

        // Indexliste erzeugen
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < mushrooms.Length; i++)
            availableIndices.Add(i);

        // 5 zufällige, eindeutige Pilze auswählen
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            int selectedIndex = availableIndices[randomIndex];

            mushrooms[selectedIndex].SetActive(true);

            MeshRenderer mushroomMaterial;
            mushroomMaterial = mushrooms[selectedIndex].gameObject.GetComponentInChildren<MeshRenderer>();
        }
    }

    IEnumerator DissolveCo()
    {
        if (m != null)
        {
            float counter = 0;
            while (m.material.GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                m.material.SetFloat("_DissolveAmount", counter);
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
