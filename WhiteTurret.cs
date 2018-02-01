using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTurret : Character {

    private WhiteTurret instance;

    [SerializeField]
    private Vector2 groundSightLine;

    [SerializeField]
    private Vector2 reversePlayerSightLine;

    [SerializeField]
    private Vector2 playerSightLine;

    [SerializeField]
    private Vector2 wallSightLine;

    private Rigidbody2D myRigidBody;

    [SerializeField]
    private float runSpeed;    

    private ITurretState currentState;    

    [SerializeField]
    private GameObject firstShots;

    [SerializeField]
    private Transform firstShotsBarrels;

    [SerializeField]
    private Transform secondShotsBarrels;

    private Health healthComponent;

    [SerializeField]
    private int reflectedTurretShotDamage;

    [SerializeField]
    private LayerMask patrolBounds;

    [SerializeField]
    private AudioSource firstAudioSource;

    [SerializeField]
    private AudioSource secondAudioSource;

    [SerializeField]
    private AudioClip twinLaserSound;

    [SerializeField]
    private AudioClip alertedSound;

    [SerializeField]
    private AudioClip hitSound;

    [SerializeField]
    private AudioClip destroyedSound;


    [SerializeField]
    Vector2 force;

    private bool enemyAlerted = false;

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

    public WhiteTurret Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<WhiteTurret>();
            }
            return instance;
        }
        
        set
        {
            instance = value;
        }
    }

    public Health HealthComponent
    {
        get
        {
            return healthComponent;
        }

        set
        {
            healthComponent = value;
        }
    }

    public AudioSource FirstAudioSource
    {
        get
        {
            return firstAudioSource;
        }

        set
        {
            firstAudioSource = value;
        }
    }

    public AudioSource SecondAudioSource
    {
        get
        {
            return secondAudioSource;
        }

        set
        {
            secondAudioSource = value;
        }
    }

    public AudioClip DestroyedSound
    {
        get
        {
            return destroyedSound;
        }

        set
        {
            destroyedSound = value;
        }
    }

    private GameObject target;

    // Use this for initialization
    public override void Start () {
        base.Start();
        MyRigidBody = GetComponent<Rigidbody2D>();
        HealthComponent = GetComponent<Health>();
        ChangeState(new White_Turret_IdleState());
    }
	
	// Update is called once per frame
	void Update () {
        

        if(target && TargetXPositionRelativeToWhiteTurret() > 10 && HealthComponent.Alive)
        {
            target = null;
            ChangeState(new White_Turret_PatrolState());
        }

        //Debug.Log(myRigidBody.velocity.y);
    }

    private void FixedUpdate()
    {
        currentState.Execute();

        GroundDetection();
        PlayerDetection();
        WallDetection();

        if (target && !HealthIsZero)
        {
            if(!FacingPlayer())
            {
                ChangeEnemyDirection();
                ChangeSightLineDirection();
            }
        }
    }

    private void PlayerDetection()
    {
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 2), reversePlayerSightLine, Color.blue);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 2), playerSightLine, Color.blue);
        RaycastHit2D[] playerDetection = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y + 2), playerSightLine, 8, 1 << 9);
        RaycastHit2D[] reversePlayerDetection = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y + 2), reversePlayerSightLine, 2, 1 << 9);
        if ((reversePlayerDetection.Length > 0) && target == null)
        {
            enemyAlerted = true;
            foreach (var hit in reversePlayerDetection)
            {
                target = hit.collider.gameObject;
            }

            SecondAudioSource.clip = alertedSound;
            SecondAudioSource.Play();

            ChangeState(new White_Turret_EngageState());
        }

        if ((playerDetection.Length > 0) && target == null)
        {
            enemyAlerted = true;
            foreach (var hit in playerDetection)
            {
                target = hit.collider.gameObject;
            }

            SecondAudioSource.clip = alertedSound;
            SecondAudioSource.Play();

            ChangeState(new White_Turret_EngageState());
        }
    }

    public void ChangeState(ITurretState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }

    private void GroundDetection()
    {
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 2), groundSightLine, Color.green);

        RaycastHit2D[] groundHits = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y + 2), groundSightLine, 5, 1 << 8);

        if (groundHits.Length == 0 && myAnimator.GetLayerWeight(1) == 0 && MyRigidBody.velocity.y == 0 && IsGrounded())
        {            
            ChangeEnemyDirection();
            ChangeSightLineDirection();
        }
    }

    private void WallDetection()
    {
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 2), wallSightLine, Color.green);

        RaycastHit2D[] wallHits = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y + 2), wallSightLine, 2, patrolBounds);

        if(wallHits.Length > 0 && myAnimator.GetLayerWeight(1) == 0 && MyRigidBody.velocity.y == 0 && IsGrounded())
        {
            ChangeEnemyDirection();
            ChangeSightLineDirection();
        }
    }

    [SerializeField]
    private void FirstShot()
    {
        int rotation = 0;
        Vector2 force = this.force;
        if (!facingRight)
        {
            rotation = 180;
            force = new Vector2(-this.force.x, 0);
        }

        GameObject firsts = Instantiate(firstShots, firstShotsBarrels.position, Quaternion.Euler(0, rotation, 0));
        //firsts.GetComponent<Transform>().SetParent(firstShotsBarrels);
        firsts.GetComponent<Rigidbody2D>().AddForce(force);
        FirstAudioSource.clip = twinLaserSound;
        FirstAudioSource.Play();
        Destroy(firsts, 5f);
    }

    [SerializeField]
    private void SecondShot()
    {
        int rotation = 0;
        Vector2 force = this.force;
        if (!facingRight) {
            rotation = 180;
            force = new Vector2(-this.force.x, 0);
        }

        GameObject seconds = Instantiate(firstShots, secondShotsBarrels.position, Quaternion.Euler(0, rotation, 0));
        //seconds.GetComponent<Transform>().SetParent(secondShotsBarrels);
        seconds.GetComponent<Rigidbody2D>().AddForce(force);

        Destroy(seconds, 5f);
    }

    private void ChangeSightLineDirection()
    {
        groundSightLine = new Vector2(groundSightLine.x * -1, groundSightLine.y);
        playerSightLine = new Vector2(playerSightLine.x * -1, playerSightLine.y);
        wallSightLine = new Vector2(wallSightLine.x * -1, wallSightLine.y);
    }

    public void Walk()
    {
        MyRigidBody.velocity = new Vector3(enemyDirection * runSpeed * Time.fixedDeltaTime, MyRigidBody.velocity.y, 0);
        myAnimator.SetFloat("walkSpeed", Mathf.Abs(runSpeed));
    }

    private float TargetXPositionRelativeToWhiteTurret()
    {
        if (target)
        {
            return Mathf.Abs(target.transform.position.x - transform.position.x);
        }
        return 999;
    }

    private bool FacingPlayer()
    {
        if(target)
        {
            if(target.transform.position.x - transform.position.x >= 0 && facingRight)
            {
                return true;
            }
            else if(target.transform.position.x - transform.position.x < 0 && !facingRight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.gameObject.tag == "TurretShots" && collision.gameObject.GetComponentInChildren<SpriteRenderer>().color.a < .9)
        {
            if(healthComponent.Life > 0)
            {
                HitByPlayer(reflectedTurretShotDamage);
            }
            else
            {
                HealthIsZero = true;
                SecondAudioSource.clip = DestroyedSound;
                SecondAudioSource.Play();
                ChangeState(new White_Turret_DeathState());
            }
        }
        if(collision.tag == "SwordCollider" && !HealthIsZero)
        {
            if(HealthComponent.Life > 0)
            {
                HitByPlayer(Player.Instance.MeleeDamage);
            }
            else
            {
                HealthIsZero = true;
                SecondAudioSource.clip = DestroyedSound;
                SecondAudioSource.Play();
                ChangeState(new White_Turret_DeathState());
            }
        }
    }

    private void HitByPlayer(int damage)
    {
        //Alert Enemy if Enemy hasn't noticed player yet
        if(!target)
        {
            target = Player.Instance.gameObject;
            ChangeState(new White_Turret_EngageState());
        }
        FirstAudioSource.clip = hitSound;
        FirstAudioSource.Play();
        myAnimator.SetTrigger("hit");
        DoDamage(damage, gameObject);
    }

    

}
