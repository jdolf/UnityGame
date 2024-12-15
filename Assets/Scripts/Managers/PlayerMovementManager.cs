using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour, StatsChangedListener
{
    const float DefaultWalkSpeed = 3f;
    const float DefaultRunSpeed = 6f;
    public Rigidbody2D body;
    public Animator animator;
    PlayerState state = PlayerState.Idle;
    float walkSpeed = DefaultWalkSpeed;
    float runSpeed = DefaultRunSpeed;
    string attackStyle;
    Vector2 movement;
    [HideInInspector]
    public Facing facing;
    bool running;
    public static Dictionary<Facing, Vector2> DirectionDictionary;
    public static Dictionary<Vector2, Facing> FacingDictionary;
    public PlayerStatsManager PlayerStats;
    private Transform weaponSlot;
    private Transform helmetSlot;
    private Transform chestSlot;
    private Transform legSlot;
    private Transform shieldSlot;
    List<Transform> equipmentSlots = new List<Transform>();

    void Awake()
    {
        PlayerStats.AddListener(this);
        FacingDictionary = new Dictionary<Vector2, Facing>()
        {
            {new Vector2(-1, 0), Facing.Left},
            {new Vector2(0, -1), Facing.Down},
            {new Vector2(1, 0), Facing.Right},
            {new Vector2(0, 1), Facing.Up},
            {new Vector2(-1, 1), Facing.UpLeft},
            {new Vector2(1, 1), Facing.UpRight},
            {new Vector2(-1, -1), Facing.DownLeft},
            {new Vector2(1, -1), Facing.DownRight}
        };

        DirectionDictionary = new Dictionary<Facing, Vector2>()
        {
            {Facing.Left, new Vector2(-1, 0)},
            {Facing.Down, new Vector2(0, -1)},
            {Facing.Right, new Vector2(1, 0)},
            {Facing.Up, new Vector2(0, 1)},
            {Facing.UpLeft, new Vector2(-1, 1)},
            {Facing.UpRight, new Vector2(1, 1)},
            {Facing.DownLeft, new Vector2(-1, -1)},
            {Facing.DownRight, new Vector2(1, -1)}
        };

        Vector2 defaultFacing = DirectionDictionary[Facing.Down];
        this.updateFacing(this.animator, defaultFacing.x, defaultFacing.y);
        facing = FacingDictionary[defaultFacing];

        this.weaponSlot = this.body.transform.Find("Sword_Slot");
        this.helmetSlot = this.body.transform.Find("Helmet_Slot");
        this.chestSlot = this.body.transform.Find("Chest_Armor_Slot");
        this.legSlot = this.body.transform.Find("Leg_Armor_Slot");
        this.shieldSlot = this.body.transform.Find("Shield_Slot");
        this.equipmentSlots.Add(this.weaponSlot);
        this.equipmentSlots.Add(this.helmetSlot);
        this.equipmentSlots.Add(this.chestSlot);
        this.equipmentSlots.Add(this.legSlot);
        this.equipmentSlots.Add(this.shieldSlot);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        float speed = 0;

        if (Input.GetKey(KeyCode.Mouse0) && !UIManagementScreen.IsActive()) {
            if (this.state != PlayerState.Attacking) {
                this.state = PlayerState.Attacking;
                if (this.weaponSlot.childCount > 0) {
                    GameObject weaponObject = this.weaponSlot.GetChild(0).gameObject;
                    float attackStartupSpeed = 0f;
                    float attackCooldownSpeed = 0f;
                    float attackSpeed = 0f;
                    
                    // Weapon
                    Animator weaponAnimator =  weaponObject.GetComponent<Animator>();
                    Weapon weapon = weaponObject.GetComponent<Weapon>();

                    if (weaponAnimator.runtimeAnimatorController != null) {
                        foreach (AnimationClip clip in weaponAnimator.runtimeAnimatorController.animationClips) {
                            if (clip.name.Contains("Startup")) {
                                attackStartupSpeed = 1 / (float)PlayerStats.Stats.Startup * clip.length * 60;
                            } else if (clip.name.Contains("Cooldown")) {
                                attackCooldownSpeed = 1 / (float)PlayerStats.Stats.Cooldown * clip.length * 60;
                            } else if (clip.name.Contains("Attack_1")) {
                                attackSpeed = 1 / (float)PlayerStats.Stats.Duration * clip.length * 60;
                            }
                        }

                        // Weapon
                        weaponAnimator.SetFloat("Attack_Startup_Speed", attackStartupSpeed);
                        weaponAnimator.SetFloat("Attack_Cooldown_Speed", attackCooldownSpeed);
                        weaponAnimator.SetFloat("Attack_Speed", attackSpeed);
                        weaponAnimator.SetBool("Attack_1", true);
                        
                    } else {
                        attackStartupSpeed = 1 / (float)PlayerStats.Stats.Startup * 2;
                        attackCooldownSpeed = 1 / (float)PlayerStats.Stats.Cooldown;
                        attackSpeed = 1 / (float)PlayerStats.Stats.Duration * 3;
                    }

                    attackStyle = weapon.Item.AttackOne.ToString();
                    this.animator.SetBool(attackStyle, true);

                    // Player
                    this.animator.SetFloat("Attack_Startup_Speed", attackStartupSpeed);
                    this.animator.SetFloat("Attack_Cooldown_Speed", attackCooldownSpeed);
                    this.animator.SetFloat("Attack_Speed", attackSpeed);
                }
            }
        }

        if ((x != 0 || y != 0) && this.state != PlayerState.Attacking && !UIManagementScreen.IsActive()) {
            movement.x = x;
            movement.y = y;
            Facing previousFacing = facing;
            //Debug.Log(facing);
            facing = FacingDictionary[movement];
            //Debug.Log(facing + "    " + previousFacing);
            if (previousFacing != facing) {
                Vector2 facingDirection = DirectionDictionary[this.facing];
                this.updateFacing(this.animator, facingDirection.x, facingDirection.y);
                UpdateEquipmentFacing();
            }

            this.state = PlayerState.Moving;
            speed = movement.sqrMagnitude;
        }

        if (x == 0 && y == 0 && this.state != PlayerState.Attacking) {
            this.state = PlayerState.Idle;
        }
        

        this.animator.SetFloat("Horizontal", movement.x);
        this.animator.SetFloat("Vertical", movement.y);
        this.animator.SetFloat("Speed", speed);
        

        running = Input.GetKey(KeyCode.LeftShift);
    }

// Runs at 50FPS
    void FixedUpdate()
    {
        if (this.state == PlayerState.Moving) {
            if (!running)
            {
                body.MovePosition(body.position + movement * walkSpeed * Time.fixedDeltaTime);
            }
            else
            {
                body.MovePosition(body.position + movement * runSpeed * Time.fixedDeltaTime);
            }
        }
    }

    protected void updateFacing(Animator anim, float x, float y)
    {
        if (anim.runtimeAnimatorController != null) {
            anim.SetFloat("Facing_Horizontal", x);
            anim.SetFloat("Facing_Vertical", y);
        }
    }

    protected void PutWeaponBehindPlayer() {
        if (this.weaponSlot.childCount > 0) {
            this.weaponSlot.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
    }

    protected void PutWeaponInFrontOfPlayer() {
        if (this.weaponSlot.childCount > 0) {
            this.weaponSlot.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }

    protected void AfterStartAttacking() {
        this.animator.SetBool(attackStyle, false);
        if (this.weaponSlot.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController != null) {
            this.weaponSlot.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Attack_1", false);
            this.weaponSlot.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Attack_2", false);
        }

        if (attackStyle == "Crush") {
            if (facing == Facing.Left || facing == Facing.DownLeft || facing == Facing.UpLeft) {
                this.weaponSlot.GetChild(0).gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, -90);
            }

            if (facing == Facing.Right || facing == Facing.DownRight || facing == Facing.UpRight) {
                this.weaponSlot.GetChild(0).gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }

    protected void FinishAttacking() {
        this.state = PlayerState.Idle;

        if (attackStyle == "Crush") {
            if (facing != Facing.Up || facing != Facing.Down) {
                this.weaponSlot.GetChild(0).gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    protected void EnableAttackHitbox() {
        this.weaponSlot.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    protected void DisableAttackHitbox() {
        this.weaponSlot.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void UpdateEquipmentFacing() {
        foreach (Transform equipmentSlot in this.equipmentSlots) {
            Vector2 facingDirection = DirectionDictionary[this.facing];
            if (equipmentSlot.childCount > 0) {
                this.updateFacing(equipmentSlot.GetChild(0).gameObject.GetComponent<Animator>(), facingDirection.x, facingDirection.y);
            } 
        }

        // Update Weapon layer
        if (new List<Facing>{Facing.Up, Facing.Left, Facing.UpLeft, Facing.DownLeft}.Contains(this.facing)) {
            this.PutWeaponBehindPlayer();
        } else {
            this.PutWeaponInFrontOfPlayer();
        }
    }

    public void StatsChanged()
    {
        walkSpeed = DefaultWalkSpeed * ((float) PlayerStats.Stats.WalkSpeed / 100);
        runSpeed = DefaultRunSpeed * ((float) PlayerStats.Stats.WalkSpeed / 100);
    }

    public enum Facing {
        Left,
        Down,
        Right,
        Up,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    enum PlayerState {
        Idle,
        Moving,
        Attacking,
        Stuck
    }

}
