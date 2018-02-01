using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour {

    [SerializeField]
    private float spinSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Transform>().Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player.Instance.RespawnPlayer();
            Switch.Instance.ResetPlatformAndTrain();
            Player.Instance.myAnimator.SetTrigger("respawn");
        }
    }

    private void KillPlayer()
    {
        
    }
}
