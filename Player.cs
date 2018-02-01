using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent (typeof(Rigidbody2D))]
public class Player : Character {

    private static Player instance;

    private Rigidbody2D myRigidBody;

    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float defensePoints;

    [SerializeField]
    private float maxDefensePoints;

    private bool isJumping;

    private bool isOverheadAttacking;

    private bool isJumpAttacking;

    [SerializeField]
    private Vector3 respawnPoint;

    [SerializeField]
    private EdgeCollider2D swordCollider;

    [SerializeField]
    private EdgeCollider2D swordColliderTwo;

    [SerializeField]
    SpriteGlow spriteGlow;

    [SerializeField]
    private GameObject fadePanel;

    private bool fadeComplete = false;

    private float timePassed = 0f;

    private Health healthComponent;

    [SerializeField]
    private int playerWealth;

    [SerializeField]
    private int meleeDamage;

    private bool repairingDefense = false;

    private bool playerOnTrain;

    [SerializeField]
    private AudioClip jumpSound;

    [SerializeField]
    private AudioClip swordSwing;

    [SerializeField]
    private AudioClip walkSound;

    [SerializeField]
    private AudioClip blockSound;

    [SerializeField]
    private AudioClip jumpAttackSound;

    [SerializeField]
    private AudioClip playerDeathSound;

    [SerializeField]
    private AudioClip hitSound;

    private AudioSource myAudioSource;

    [SerializeField]
    private Button leftButton;

    [SerializeField]
    private Button rightButton;

    [SerializeField]
    private Button jumpButton;

    private bool jumpButtonPressed = false;

    [SerializeField]
    private Button attackButton;

    [SerializeField]
    private Button defendButton;

    [SerializeField]
    private Button interactButton;


    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }

        set
        {
            swordCollider = value;
        }
    }

    public EdgeCollider2D SwordColliderTwo
    {
        get
        {
            return swordColliderTwo;
        }
        set
        {
            swordColliderTwo = value;
        }
    }

    public bool IsJumpAttacking
    {
        get
        {
            return isJumpAttacking;
        }

        set
        {
            isJumpAttacking = value;
        }
    }

    public bool IsJumping
    {
        get
        {
            return isJumping;
        }

        set
        {
            isJumping = value;
        }
    }

    public bool IsOverheadAttacking
    {
        get
        {
            return isOverheadAttacking;
        }

        set
        {
            isOverheadAttacking = value;
        }
    }

    public Rigidbody2D MyRigidBody
    {
        get
        {
            return myRigidBody;
        }

        set
        {
            myRigidBody = value;
        }
    }

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    public int MeleeDamage
    {
        get
        {
            return meleeDamage;
        }

        set
        {
            meleeDamage = value;
        }
    }

    public float MaxDefensePoints
    {
        get
        {
            return maxDefensePoints;
        }

        set
        {
            maxDefensePoints = value;
        }
    }

    public float DefensePoints
    {
        get
        {
            return defensePoints;
        }

        set
        {
            defensePoints = value;
        }
    }
    
    public int PlayerWealth
    {
        get
        {
            return playerWealth;
        }

        set
        {
            playerWealth = value;
        }
    }

    public float RunSpeed
    {
        get
        {
            return runSpeed;
        }

        set
        {
            runSpeed = value;
        }
    }

    public bool PlayerOnTrain
    {
        get
        {
            return playerOnTrain;
        }

        set
        {
            playerOnTrain = value;
        }
    }

    public Vector3 RespawnPoint
    {
        get
        {
            return respawnPoint;
        }

        set
        {
            respawnPoint = value;
        }
    }

    public AudioSource MyAudioSource
    {
        get
        {
            return myAudioSource;
        }

        set
        {
            myAudioSource = value;
        }
    }

    public AudioClip JumpSound
    {
        get
        {
            return jumpSound;
        }

        set
        {
            jumpSound = value;
        }
    }

    public AudioClip SwordSwing
    {
        get
        {
            return swordSwing;
        }

        set
        {
            swordSwing = value;
        }
    }

    public AudioClip WalkSound
    {
        get
        {
            return walkSound;
        }

        set
        {
            walkSound = value;
        }
    }

    public AudioClip BlockSound
    {
        get
        {
            return blockSound;
        }

        set
        {
            blockSound = value;
        }
    }

    public AudioClip JumpAttackSound
    {
        get
        {
            return jumpAttackSound;
        }

        set
        {
            jumpAttackSound = value;
        }
    }

    public AudioClip HitSound
    {
        get
        {
            return hitSound;
        }

        set
        {
            hitSound = value;
        }
    }

    public AudioClip PlayerDeathSound
    {
        get
        {
            return playerDeathSound;
        }

        set
        {
            playerDeathSound = value;
        }
    }

    // Use this for initialization
    public override void Start () {
        base.Start();
        MyRigidBody = GetComponent<Rigidbody2D>();
        healthComponent = GetComponent<Health>();
        MyAudioSource = GetComponent<AudioSource>();
        PlayerOnTrain = false;
        Physics2D.IgnoreLayerCollision(9, 14, true);
    }

    void Update()
    {
        if(myAnimator.GetInteger("currentState") == 7 && !Respawning)
        {
            Respawning = true;
            InitateRespawnFlashCharacter(gameObject);
        }
        HandleKeyInput();
        StopBlocking();

        if(DefenseBar.Instance)
        {
            if (myAnimator.GetInteger("currentState") != 5 && myAnimator.GetInteger("currentState") != 6 && DefenseBar.Instance.DefensePoints < DefenseBar.Instance.MaxDefensePoints && !repairingDefense)
            {
                StartCoroutine(RepairDefense());
            }
        }
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");
        float touchHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");

        if(touchHorizontal == 0)
        {
            MyRigidBody.velocity = new Vector2(0, MyRigidBody.velocity.y);
        }
        Debug.Log("touchHorizontal = " + touchHorizontal);
        if(!IsOverheadAttacking)
        {
            HandleMovement(touchHorizontal);
        }

        if(IsGrounded())
        {
            myAnimator.SetBool("isGrounded", true);
            myAnimator.ResetTrigger("jumpAttack");
            //IsJumpAttacking = false;
            IsJumping = false;
        }
        else
        {
            IsJumping = true;
            myAnimator.SetBool("isGrounded", false);
        }

        if(IsJumping)
        {
            myAnimator.SetBool("isJumping", true);
        }
        else
        {
            myAnimator.SetBool("isJumping", false);
        }

        if (MyRigidBody.velocity.y < -1 && IsGrounded() == false)
        {
            myAnimator.SetBool("land", true);
        }
        else
        {
            myAnimator.SetBool("land", false);
        }

        if(Time.timeSinceLevelLoad > 2f && !fadeComplete)
        {
            FadeGlow(5, .2f);
        }

        PlayerDeath();        

        //Debug.Log("IsGrounded Value = " + IsGrounded());
    }

    private void HandleMovement(float horizontal)
    {
        if(horizontal > 0 && (!IsJumpAttacking || IsGrounded()) && (myAnimator.GetInteger("currentState") != 1) && !myAnimator.GetBool("blocking") && GetComponent<Health>().Alive && myAnimator.GetInteger("currentState") != 7)
        {
            MyRigidBody.velocity = new Vector2(horizontal * RunSpeed * Time.deltaTime, MyRigidBody.velocity.y);
            myAnimator.SetFloat("walkSpeed", horizontal);
            ChangeDirection(horizontal);
        }
        else if(horizontal < 0 && (!IsJumpAttacking || IsGrounded()) && (myAnimator.GetInteger("currentState") != 1) && !myAnimator.GetBool("blocking") && GetComponent<Health>().Alive && myAnimator.GetInteger("currentState") != 7)
        {
            MyRigidBody.velocity = new Vector2(horizontal * RunSpeed * Time.deltaTime, MyRigidBody.velocity.y);
            myAnimator.SetFloat("walkSpeed", Mathf.Abs(horizontal));
            ChangeDirection(horizontal);
        }
        else if(horizontal == 0)
        {
            myAnimator.SetFloat("walkSpeed", 0);
        }
    }

    private void DebugLog()
    {
        Debug.Log("Player IsJumping = " + IsJumping);
        Debug.Log("Player isOverheadAttacking = " + isOverheadAttacking);
        Debug.Log("Player isJumpAttacking = " + IsJumpAttacking);
    }

    private void HandleKeyInput()
    {
        if((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Jump")) && !IsJumping && !IsOverheadAttacking && GetComponent<Health>().Alive && myAnimator.GetInteger("currentState") != 7 && myAnimator.GetBool("blocking") == false)
        {
            IsJumping = true;
            MyRigidBody.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetTrigger("jump");
            MyAudioSource.clip = JumpSound;
            MyAudioSource.Play();
        }
        if((Input.GetKeyDown(KeyCode.LeftShift) || CrossPlatformInputManager.GetButtonDown("Attack")) && !IsJumping && GetComponent<Health>().Alive && myAnimator.GetInteger("currentState") != 7 && myAnimator.GetBool("blocking") == false)
        {
            myAnimator.SetTrigger("meleeOne");
            IsOverheadAttacking = true;
            MyAudioSource.clip = SwordSwing;
            MyAudioSource.Play();
        }
        else if ((Input.GetKeyDown(KeyCode.LeftShift) || CrossPlatformInputManager.GetButtonDown("Attack")) && IsJumping && !IsGrounded() && GetComponent<Health>().Alive && myAnimator.GetInteger("currentState") != 7 && myAnimator.GetBool("blocking") == false)
        {
            myAnimator.SetFloat("walkSpeed", 0);
            myAnimator.SetTrigger("jumpAttack");
            IsJumpAttacking = true;
            MyAudioSource.clip = JumpAttackSound;
            MyAudioSource.Play();
        }
        if((Input.GetKeyDown(KeyCode.F) || CrossPlatformInputManager.GetButtonDown("Defend")) && (myAnimator.GetInteger("currentState") == 2 || myAnimator.GetInteger("currentState") == 3) && GetComponent<Health>().Alive && myAnimator.GetInteger("currentState") != 7 && defensePoints > 0)
        {
            myAnimator.SetBool("blocking", true);
            
        }
        if((Input.GetKeyDown(KeyCode.F) || CrossPlatformInputManager.GetButtonDown("Defend")) && myAnimator.GetInteger("currentState") != 2 && myAnimator.GetInteger("currentState") != 3 && myAnimator.GetBool("blocking"))
        {
            myAnimator.SetBool("blocking", false);
        }        
    }

    public void StartWalkingEnumerator()
    {
        StartCoroutine(WalkingEnumerator());
    }

    public void EndWalkingEnumerator()
    {
        StopCoroutine(WalkingEnumerator());
    }

    private IEnumerator WalkingEnumerator()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float touchHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        MyAudioSource.clip = WalkSound;
        while(Mathf.Abs(touchHorizontal) > 0)
        {
            MyAudioSource.Play();
            yield return new WaitForSeconds(.5f);
        }
    }

    public void MeleeAttack()
    {
        SwordCollider.enabled = true;        
    }

    [SerializeField]
    private void JumpAttackBegin()
    {
        SwordColliderTwo.enabled = true;
    }

    [SerializeField]
    private void JumpAttackEnd()
    {
        SwordColliderTwo.enabled = false;
    }

    [SerializeField]
    private void JumpAttackDownForce()
    {
        myRigidBody.AddForce(new Vector2(0, -jumpForce * 2));
    }

    public void Blocking(GameObject blockedObject)
    {
        int damage;
        if(blockedObject.tag == "TurretShots")
        {
            damage = 10;
            DefenseBar.Instance.ReduceDefense(damage);
            defensePoints -= damage;
        }
        else if(blockedObject.tag == "bullet")
        {
            damage = 5;
            DefenseBar.Instance.ReduceDefense(damage);
            defensePoints -= damage;
        }
    }

    private void StopBlocking()
    {
        if ((myAnimator.GetInteger("currentState") == 5 || myAnimator.GetInteger("currentState") == 6) && defensePoints <= 0)
        {
            myAnimator.SetBool("blocking", false);
        }
    }

    private IEnumerator RepairDefense()
    {
        repairingDefense = true;
        while ((DefenseBar.Instance.DefensePoints < DefenseBar.Instance.MaxDefensePoints) && myAnimator.GetInteger("currentState") != 5 && myAnimator.GetInteger("currentState") != 6)
        {
            DefenseBar.Instance.RepairDefense(1);
            DefensePoints += 1;
            yield return new WaitForSeconds(.1f);        
        }
        repairingDefense = false;
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag == "bullet")
        {
            //DoDamage(5, gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if(collision.tag == "door" && (Input.GetKeyDown(KeyCode.E) || CrossPlatformInputManager.GetButtonDown("Interact")))
        {
            fadePanel.GetComponent<Animator>().SetTrigger("FadeToBlack");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "subwayPlatform")
        {
            RespawnPlayer();
            Switch.Instance.ResetPlatformAndTrain();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "commuterTrain")
        {
            PlayerOnTrain = true;
        }
        else
        {
            PlayerOnTrain = false;
        }
    }

    private void FadeGlow(float fadeDuration, float fadeSpeed)
    {
        float duration = fadeDuration;

        spriteGlow.GlowColor = new Color(spriteGlow.GlowColor.r, spriteGlow.GlowColor.g, spriteGlow.GlowColor.b, Mathf.Lerp(spriteGlow.GlowColor.a, 0, timePassed / duration));

        timePassed += Time.deltaTime * fadeSpeed;
        if (spriteGlow.GlowColor.a <= 0.1f)
        {
            fadeComplete = true;
            spriteGlow.OutlineWidth = 0;
        }            
    }

    public void RespawnPlayer()
    {
        GetComponent<Transform>().position = RespawnPoint;
    }

    private void PlayerDeath()
    {
        if (healthComponent.Life <= 0 && healthComponent.Alive)
        {
            myAnimator.SetTrigger("death");
            healthComponent.Alive = false;
        }
    }

    public void PlayFootStep()
    {
        if(IsGrounded())
        {
            MyAudioSource.clip = walkSound;
            MyAudioSource.Play();
        }        
    }

    public void BtnJump()
    {
        jumpButtonPressed = true;
    }

    /*
    public void BtnAttack()
    {
        myAnimator.SetTrigger("attack");
    }

    public void BtnMove(float direction)
    {
        this.direction = direction;
        this.move = true;
    }
    */

}
