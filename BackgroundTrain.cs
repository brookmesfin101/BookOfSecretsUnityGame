using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTrain : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private float trainSound;

    private AudioSource myAudioSource;

    private bool travelingRight;

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

    // Use this for initialization
    void Start () {
        travelingRight = true;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (travelingRight)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed * Time.deltaTime, 0);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-speed * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "trainStop" && travelingRight)
        {
            travelingRight = false;
        }
        else if(collision.gameObject.tag == "trainStop" && !travelingRight)
        {
            travelingRight = true;
        }
    }
}
