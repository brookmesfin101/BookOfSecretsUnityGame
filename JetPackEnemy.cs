using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPackEnemy : Character {

    private static JetPackEnemy instance;

    public static JetPackEnemy Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<JetPackEnemy>();
            }
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    private Rigidbody2D myRigidBody;

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

    private IEnemyState currentState;

    [SerializeField]
    private Vector2 groundSightLine = new Vector3(3.14f, 5f);

    [SerializeField]
    private Vector2 playerSightLine;

    [SerializeField]
    private Vector2 reversePlayerSightLine;

    [SerializeField]
    private Vector2 gunSightLine;

    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private float jumpFactor;

    [SerializeField]
    private float testField;

    [SerializeField]
    private GameObject blastPrefab;

    [SerializeField]
    private AudioSource firstAudioSource;

    [SerializeField]
    private AudioSource secondAudioSource;

    private bool enemyAlerted = false;

    [SerializeField]
    private Transform endOfGun;

    [SerializeField]
    private float blastSpeed;

    [SerializeField]
    private float airSpeed;

    private bool airMoveProperty;

    [SerializeField]
    private AudioClip shootLaserSound;

    [SerializeField]
    private AudioClip jetPackSound;

    [SerializeField]
    private AudioClip jetPackDeathSound;

    [SerializeField]
    private AudioClip jetPackHitSound;

    public float JumpFactor
    {
        get
        {
            return jumpFactor;
        }        
    }    

    public Transform EndOfGun
    {
        get
        {
            return endOfGun;
        }

        set
        {
            endOfGun = value;
        }
    }

    public bool AirMoveProperty
    {
        get
        {
            return airMoveProperty;
        }

        set
        {
            airMoveProperty = value;
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

    private GameObject target;    

    List<int> directionsList = new List<int>();

    // Use this for initialization
    public override void Start () {
        base.Start();
        MyRigidBody = GetComponent<Rigidbody2D>();
        ChangeState(new JP_IdleState());
        AirMoveProperty = true;
    }
	
	// Update is called once per frame
	void Update () {
        
        ManageFlyEngageState();

        if(BeingDamaged)
        {
            FirstAudioSource.clip = jetPackHitSound;
            FirstAudioSource.Play();
            if (currentState is JP_FlyEngageState && !HealthIsZero)
            {                
                myAnimator.SetTrigger("JetPack_Hurt");                
            }
            else if(!HealthIsZero)
            {
                myAnimator.SetTrigger("Hurt");
            }
            BeingDamaged = false;
        }


        //Debug.Log("EnemyRelative to Player x position = " + TargetXPositionRelativeToJPEnemy());
        //Debug.Log("Enemy Relative to Player Y position = " + TargetYPositionRelativeToJPEnemy());       
    }

    private void FixedUpdate()
    {
        currentState.Execute();

        PlayerDetection();
        GroundDetection();
        GunController();

        if(GetComponent<Health>().Life <= 0 && !(currentState is JP_FallState))
        {
            HealthIsZero = true;
            FirstAudioSource.clip = jetPackDeathSound;
            FirstAudioSource.Play();
            ChangeState(new JP_FallState());            
        }

        if(myAnimator.GetLayerWeight(1) == 1 && !SecondAudioSource.isPlaying && !HealthIsZero)
        {
            SecondAudioSource.Play();
        }
        else if((myAnimator.GetLayerWeight(1) != 1 && SecondAudioSource.isPlaying) || HealthIsZero)
        {
            SecondAudioSource.Stop();
        }
    }

    private void PlayerDetection()
    {
        Debug.DrawRay(transform.position, reversePlayerSightLine, Color.blue);
        Debug.DrawRay(transform.position, playerSightLine, Color.blue);
        RaycastHit2D[] playerDetection = Physics2D.RaycastAll(transform.position, playerSightLine, 8, 1 << 9);
        RaycastHit2D[] reversePlayerDetection = Physics2D.RaycastAll(transform.position, reversePlayerSightLine, 2, 1 << 9);
        if((reversePlayerDetection.Length > 0) && target == null)
        {
            enemyAlerted = true;
            foreach (var hit in reversePlayerDetection)
            {
                target = hit.collider.gameObject;
            }

            ChangeState(new JP_FlyEngageState());
            StartCoroutine(FlyUp());
            myAnimator.SetLayerWeight(1, 1);
        }

        if ((playerDetection.Length > 0) && target == null)
        {
            enemyAlerted = true;
            foreach (var hit in playerDetection)
            {
                target = hit.collider.gameObject;
            }

            ChangeState(new JP_FlyEngageState());
            StartCoroutine(FlyUp());
            myAnimator.SetLayerWeight(1, 1);
        }            
    }    

    private void GroundDetection()
    {
        Debug.DrawRay(transform.position, groundSightLine, Color.green);

        RaycastHit2D[] groundHits = Physics2D.RaycastAll(transform.position, groundSightLine, 5, 1 << 8);

        if (groundHits.Length == 0 && myAnimator.GetLayerWeight(1) == 0 && MyRigidBody.velocity.y == 0 && IsGrounded())
        {
            ChangeEnemyDirection();
            ChangeSightLineDirection();
        }
    }

    private void ChangeSightLineDirection()
    {
        groundSightLine = new Vector2(groundSightLine.x * -1, groundSightLine.y);
        playerSightLine = new Vector2(playerSightLine.x * -1, playerSightLine.y);
    }

    private void GunController()
    {
        if(!target)
        {
            Debug.DrawRay(new Vector3(transform.position.x + .7f, transform.position.y - .5f, transform.position.z), gunSightLine, Color.red);
        }
        else
        {
            Debug.DrawRay(new Vector3(transform.position.x + .7f, transform.position.y - .5f, transform.position.z), (target.transform.position - transform.position), Color.red);
        }        
    }

    private float TargetXPositionRelativeToJPEnemy()
    {
        if(target)
        {
            return target.transform.position.x - transform.position.x;
        }

        return 999;
    }

    private float TargetYPositionRelativeToJPEnemy()
    {
        if(target)
        {            
            return EndOfGun.position.y - target.transform.position.y;
        }
        return 999;
    }

    private float CalculateAngle()
    {
        if(myAnimator.GetInteger("AirLayerState") == 2)
        {
            if (facingRight == true)
                return 180;
            else
                return 0;            
        }
        else
        {
            if (EndOfGun && target && TargetYPositionRelativeToJPEnemy() >= 0)
            {
                float angle = Mathf.Atan((target.transform.position.x - EndOfGun.position.x) / (EndOfGun.position.y - target.transform.position.y)) * Mathf.Rad2Deg;
                return angle + 90;
            }
            else if (EndOfGun && target && TargetYPositionRelativeToJPEnemy() < 0)
            {
                float angle = Mathf.Atan((target.transform.position.x - EndOfGun.position.x) / (EndOfGun.position.y - target.transform.position.y)) * Mathf.Rad2Deg;
                return angle - 90;
            }
        }        
        return 999;
    }

    [SerializeField]
    private void ShootGun()
    {        
        GameObject blast = Instantiate(blastPrefab, EndOfGun.position, Quaternion.Euler(0, 0, CalculateAngle()));

        if(myAnimator.GetInteger("AirLayerState") == 2)
        {
            if (facingRight)
                blast.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * blastSpeed;
            else
                blast.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * blastSpeed;
        }
        else
        {
            blast.GetComponent<Rigidbody2D>().velocity = new Vector2((target.transform.position.x - EndOfGun.position.x), (target.transform.position.y - EndOfGun.position.y)).normalized * blastSpeed;
        }

        FirstAudioSource.clip = shootLaserSound;
        FirstAudioSource.Play();

        Destroy(blast, 5f);           
    }

    public void ChangeState(IEnemyState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);        
    }

    public void Walk()
    {
        MyRigidBody.velocity = new Vector3(enemyDirection * runSpeed * Time.deltaTime, MyRigidBody.velocity.y, 0);
        myAnimator.SetFloat("walkSpeed", Mathf.Abs(runSpeed));
    }

    private void ManageFlyEngageState()
    {
        if (currentState is JP_FlyEngageState && myAnimator.GetBool("flying") == true)
        {
            if (TargetXPositionRelativeToJPEnemy() > 3.75 && TargetXPositionRelativeToJPEnemy() < 20)
            {                
                if (target.GetComponent<Player>().IsJumping || TargetYPositionRelativeToJPEnemy() < 2.1f)
                {
                    myAnimator.SetBool("0_DegreeShot", true);
                }
                else
                {
                    myAnimator.SetBool("70_DegreeShot", false);
                    myAnimator.SetBool("0_DegreeShot", false);
                }
                if (facingRight == false)
                {
                    ChangeEnemyDirection();
                    ChangeSightLineDirection();
                }
            }
            else if (TargetXPositionRelativeToJPEnemy() <= 3.75 && TargetXPositionRelativeToJPEnemy() > 2.20)
            {
                if (target.GetComponent<Player>().IsJumping || TargetYPositionRelativeToJPEnemy() < 2.1f)
                {
                    myAnimator.SetBool("0_DegreeShot", true);
                    myAnimator.SetBool("45_DegreeShot", true);
                }
                else
                {
                    myAnimator.SetBool("0_DegreeShot", false);
                    myAnimator.SetBool("45_DegreeShot", false);
                    myAnimator.SetBool("70_DegreeShot", true);
                }

                if (facingRight == false)
                {
                    ChangeEnemyDirection();
                    ChangeSightLineDirection();
                }
            }
            else if (TargetXPositionRelativeToJPEnemy() <= 2.20 && TargetXPositionRelativeToJPEnemy() > -1.40)
            {
                myAnimator.SetBool("0_DegreeShot", false);
                myAnimator.SetBool("45_DegreeShot", false);
                myAnimator.SetBool("70_DegreeShot", false);
                
                if (AirMoveProperty == true)
                {
                    StartCoroutine(AirMove());
                }
            }
            else if (TargetXPositionRelativeToJPEnemy() <= -1.40 && TargetXPositionRelativeToJPEnemy() >= -3.30)
            {
                if (facingRight)
                {
                    ChangeEnemyDirection();
                    ChangeSightLineDirection();
                }

                if (target.GetComponent<Player>().IsJumping || TargetYPositionRelativeToJPEnemy() < 2.1f)
                {

                    myAnimator.SetBool("0_DegreeShot", true);                    
                    myAnimator.SetBool("45_DegreeShot", false);
                    myAnimator.SetBool("70_DegreeShot", false);
                }
                else
                {
                    myAnimator.SetBool("0_DegreeShot", false);
                    myAnimator.SetBool("45_DegreeShot", false);
                    myAnimator.SetBool("70_DegreeShot", true);
                }
            }
            else if (TargetXPositionRelativeToJPEnemy() < -3.30 && TargetXPositionRelativeToJPEnemy() > -20)
            {
                if (target.GetComponent<Player>().IsJumping || TargetYPositionRelativeToJPEnemy() < 2.1f)
                {
                    myAnimator.SetBool("0_DegreeShot", true);
                    myAnimator.SetBool("45_DegreeShot", false);
                    myAnimator.SetBool("70_DegreeShot", false);
                }
                else
                {            
                    myAnimator.SetBool("0_DegreeShot", false);
                    myAnimator.SetBool("45_DegreeShot", true);
                    myAnimator.SetBool("70_DegreeShot", false);
                }
            }
            else if(Mathf.Abs(TargetXPositionRelativeToJPEnemy()) > 20 && TargetXPositionRelativeToJPEnemy() != 999)
            {
                target = null;
                enemyAlerted = false;
                myAnimator.SetBool("0_DegreeShot", false);
                myAnimator.SetBool("45_DegreeShot", false);
                myAnimator.SetBool("70_DegreeShot", false);                
                ChangeState(new JP_IdleState());
            }
        }
    }

    private IEnumerator AirMove()
    {        
        AirMoveProperty = false;

        int direction = 0;

        directionsList.Add(direction);

        if (directionsList.Count <= 2)
        {
            direction = 1;
        }
        else if(directionsList.Count > 2 && directionsList.Count < 5)
        {
            direction = -1;
        }
        else
        {
            directionsList.Clear();
        }        

        yield return new WaitForSeconds(.5f);

        if(direction == 1 && facingRight)
        {
            myAnimator.SetBool("flyForward", true);
        }
        else if(direction == 1 && !facingRight)
        {
            myAnimator.SetBool("flyBackward", true);
        }
        else if (direction == -1 && facingRight)
        {
            myAnimator.SetBool("flyBackward", true);
        }
        else if (direction == -1 && !facingRight)
        {
            myAnimator.SetBool("flyForward", true);
        }

        MyRigidBody.velocity = new Vector2(direction * airSpeed, 0);

        yield return new WaitForSeconds(1f);

        AirMoveProperty = true;
        myAnimator.SetBool("flyForward", false);
        myAnimator.SetBool("flyBackward", false);
        MyRigidBody.velocity = Vector2.zero;        
    }

    public IEnumerator FlyUp()
    {        
        MyRigidBody.velocity = new Vector2(0, 0);
        MyRigidBody.AddForce(new Vector2(0, JumpFactor));
        MyRigidBody.gravityScale = 0;

        yield return new WaitForSeconds(.5f);

        MyRigidBody.AddForce(new Vector2(0, -JumpFactor));
        myAnimator.SetBool("flying", true);
    }

    public void FlyDown()
    {
        MyRigidBody.AddForce(new Vector2(0, -JumpFactor));
        myAnimator.SetBool("flyingDown", true);
        myAnimator.SetLayerWeight(1, 0);

        /*
        MyRigidBody.velocity = Vector3.zero;
        MyRigidBody.angularVelocity = 0f;
        
        myAnimator.SetBool("flying", false);
        */

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            myAnimator.SetBool("flyingDown", false);
            MyRigidBody.velocity = Vector3.zero;
            MyRigidBody.angularVelocity = 0f;
            myAnimator.SetLayerWeight(1, 0);
            myAnimator.SetBool("flying", false);
        }        
    }

    private IEnumerator AlertEnemy()
    {
        ChangeEnemyDirection();
        ChangeState(new JP_IdleState());
        yield return new WaitForSeconds(.5f);

        ChangeSightLineDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.transform.parent && collision.transform.parent.tag == "Player" && !enemyAlerted)
        {
            StartCoroutine(AlertEnemy());            
            enemyAlerted = true;
        }
        if(collision.transform.parent && collision.transform.parent.tag == "Player" && enemyAlerted)
        {
            DoDamage(25, gameObject);
        }
    }   
}
