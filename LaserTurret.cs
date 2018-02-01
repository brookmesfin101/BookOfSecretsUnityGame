using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Character {

    private static LaserTurret instance;

    private float time = 0;
    private float damageTime = 0;
    private float dontshootDuration = 3;
    private float shootDuration = 6;

    private AudioSource myAudioSource;

    [SerializeField]
    private AudioClip laserShootSound;

    public static LaserTurret Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LaserTurret>();
            }
            return instance;
        }

        set
        {
            instance = value;
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

    public AudioClip LaserShootSound
    {
        get
        {
            return laserShootSound;
        }

        set
        {
            laserShootSound = value;
        }
    }

    // Use this for initialization
    public override void Start () {
        base.Start();
        Instance = this;
        MyAudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Shoot();

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DamagingPlayer(collision);
        }
    }

    private void Shoot()
    {
        time += Time.deltaTime;

        if(time > dontshootDuration)
        {
            myAnimator.SetBool("shoot", true);

            if (time > shootDuration)
            {
                time = 0;
                myAnimator.SetBool("shoot", false);
            }            
        }
    }    

    private void DamagingPlayer(Collider2D collision)
    {
        damageTime += Time.deltaTime;
        if (damageTime > .02)
        {
            DoDamage(1, collision.gameObject);
            damageTime = 0;
        }
    }

    public void PlaySound()
    {
        MyAudioSource.clip = laserShootSound;
        MyAudioSource.Play();
    }
}
