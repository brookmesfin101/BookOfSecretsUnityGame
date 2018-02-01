using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Switch : MonoBehaviour {

    private static Switch instance;

    [SerializeField]
    private Sprite redSwitch;

    [SerializeField]
    private Sprite greenSwitch;

    [SerializeField]
    private GameObject platformRaiser;

    [SerializeField]
    private Vector3 platformInitialPosition;

    [SerializeField]
    private AudioClip raisingPlatformSound;

    private AudioSource myAudioSource;

    private Vector3 newRespawnPoint;

    private float counter;

    private bool delayStarted;

    private bool inSwitchArea;

    public static Switch Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Switch>();
            }
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    public bool DelayStarted
    {
        get
        {
            return delayStarted;
        }

        set
        {
            delayStarted = value;
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

    // Use this for initialization
    void Start () {
        MyAudioSource = GetComponent<AudioSource>();
        counter = 0;
        newRespawnPoint = GetComponentInChildren<Transform>().localPosition;
        DelayStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
        RaisePlatform();

        if (CommuterTrain.Instance.gameObject.tag == "commuterTrain" && GetComponent<SpriteRenderer>().sprite == redSwitch && platformRaiser.GetComponent<Transform>().localPosition.y > 5.99 &&
            DelayStarted == false)
            StartCoroutine(DelayStartUp());

        if (inSwitchArea && (Input.GetKeyDown(KeyCode.E) || CrossPlatformInputManager.GetButtonDown("Interact")) && GetComponent<SpriteRenderer>().sprite != redSwitch)
            SwitchSprite();
    }

    private void SwitchSprite()
    {        
        if((Input.GetKeyDown(KeyCode.E) || CrossPlatformInputManager.GetButtonDown("Interact")) && GetComponent<SpriteRenderer>().sprite != redSwitch)
        {
            MyAudioSource.clip = raisingPlatformSound;
            MyAudioSource.Play();
            GetComponent<SpriteRenderer>().sprite = redSwitch;
            Player.Instance.RespawnPoint = newRespawnPoint;            
        }
    }

    public void ResetPlatformAndTrain()
    {
        Debug.Log("ResetPlatformAndTrain Called");
        if(CommuterTrain.Instance.tag == "commuterTrain")
        {
            DelayStarted = false;
            CommuterTrain.Instance.HorizontalSpeed = 0;
            CommuterTrain.Instance.MyRigidBody.velocity = new Vector3(CommuterTrain.Instance.HorizontalSpeed * Time.deltaTime, 0, 0);
            CommuterTrain.Instance.StartEngine = false;
            CommuterTrain.Instance.GetComponent<Transform>().position = CommuterTrain.Instance.TrainSpawnPoint;
            CommuterTrain.Instance.MyAudioSource.clip = CommuterTrain.Instance.TrainMovingSound;
            CommuterTrain.Instance.MyAudioSource.volume = 0;
            CommuterTrain.Instance.MyAudioSource.loop = true;
            Player.Instance.myAnimator.SetTrigger("respawn");
        }
            
        platformRaiser.GetComponent<Transform>().localPosition = platformInitialPosition;
        GetComponent<SpriteRenderer>().sprite = greenSwitch;
    }

    private IEnumerator DelayStartUp()
    {
        DelayStarted = true;
        yield return new WaitForSeconds(2f);
        CommuterTrain.Instance.StartEngine = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            inSwitchArea = true;
        }
    }

    private void RaisePlatform()
    {
        if(platformRaiser.GetComponent<Transform>().localPosition.y < 6.01 && GetComponent<SpriteRenderer>().sprite == redSwitch)
        {
            counter += Time.deltaTime;

            if(counter > .01)
            {
                Vector2 updatedPlatform = new Vector2 (platformRaiser.GetComponent<Transform>().localPosition.x, platformRaiser.GetComponent<Transform>().localPosition.y + 1.5f * Time.deltaTime);
                platformRaiser.GetComponent<Transform>().localPosition = updatedPlatform;
            }
        }
        else if(platformRaiser.GetComponent<Transform>().localPosition.y >= 6.01 && GetComponent<SpriteRenderer>().sprite == redSwitch && MyAudioSource.isPlaying)
        {
            MyAudioSource.Stop();
        }
    }
}
