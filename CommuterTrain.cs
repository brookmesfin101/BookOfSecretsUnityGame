using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommuterTrain : MonoBehaviour {

    private static CommuterTrain instance;

    private Rigidbody2D myRigidBody;

    [SerializeField]
    private float horizontalSpeed;

    [SerializeField]
    private float verticalSpeed;

    [SerializeField]
    private int[] layersToIgnore;

    private float counter;

    private bool startEngine;

    private float initialPlayerRunSpeed;

    [SerializeField]
    private AudioClip trainScreechSound;

    [SerializeField]
    private AudioClip trainMovingSound;

    [SerializeField]
    private Vector3 trainSpawnPoint;

    [SerializeField]
    private Collider2D level1BuildingCollider;

    private bool stopTrain = false;

    private AudioSource myAudioSource;

    public bool StartEngine
    {
        get
        {
            return startEngine;
        }

        set
        {
            startEngine = value;
        }
    }

    public static CommuterTrain Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CommuterTrain>();
            }
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    public Vector3 TrainSpawnPoint
    {
        get
        {
            return trainSpawnPoint;
        }

        set
        {
            trainSpawnPoint = value;
        }
    }

    public float HorizontalSpeed
    {
        get
        {
            return horizontalSpeed;
        }

        set
        {
            horizontalSpeed = value;
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

    public AudioClip TrainScreechSound
    {
        get
        {
            return trainScreechSound;
        }

        set
        {
            trainScreechSound = value;
        }
    }

    public AudioClip TrainMovingSound
    {
        get
        {
            return trainMovingSound;
        }

        set
        {
            trainMovingSound = value;
        }
    }

    // Use this for initialization
    void Start () {
        MyRigidBody = GetComponent<Rigidbody2D>();
        counter = 0;
        StartEngine = false;
        initialPlayerRunSpeed = Player.Instance.RunSpeed;
        MyAudioSource = GetComponent<AudioSource>();

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), level1BuildingCollider);

        if (gameObject.tag == "commuterTrain")
        {
            HorizontalSpeed = 0;
            MyRigidBody.velocity = new Vector3(HorizontalSpeed * Time.deltaTime, verticalSpeed, 0);
        }
            
        else
        {
            HorizontalSpeed = 400;
            MyRigidBody.velocity = new Vector3(HorizontalSpeed * Time.deltaTime, verticalSpeed, 0);
        }                    
    }

    private void FixedUpdate()
    {
        if (gameObject.tag == "commuterTrain" && !stopTrain)
        {
            if (StartEngine)
            {
                StartUpEngine();
            }

            IncreasePlayerRunSpeed();
        }
        else if (stopTrain)
        {
            HorizontalSpeed = 0;
            IncreasePlayerRunSpeed();
            MyRigidBody.velocity = new Vector3(HorizontalSpeed * Time.deltaTime, verticalSpeed, 0);
        }

        if(StartEngine && !MyAudioSource.isPlaying && !stopTrain)
        {
            MyAudioSource.Play();
        }
        else if(StartEngine && MyAudioSource.isPlaying && MyAudioSource.volume < .7)
        {
            MyAudioSource.volume += .001f;
        }
        else if (stopTrain)
        {
            //MyAudioSource.Stop();
        }
    }

    public void StartUpEngine()
    {
        counter += Time.deltaTime;

        if (counter > .05 && HorizontalSpeed < 1000)
        {
            HorizontalSpeed += 3;
            counter = 0;
        }

        MyRigidBody.velocity = new Vector3(HorizontalSpeed * Time.deltaTime, verticalSpeed, MyRigidBody.velocity.y);      
    }

    private void IncreasePlayerRunSpeed()
    {
        if(Player.Instance.PlayerOnTrain)
        {
            Player.Instance.RunSpeed = HorizontalSpeed + 200;
        }
        else if(!Player.Instance.PlayerOnTrain || HorizontalSpeed < 50)
        {
            Player.Instance.RunSpeed = initialPlayerRunSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "trainStop")
        {
            stopTrain = true;
            MyAudioSource.Stop();
            MyAudioSource.clip = TrainScreechSound;
            MyAudioSource.loop = false;
            MyAudioSource.Play();
        }
    }
}
