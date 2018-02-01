using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {

    string[] destroyable = {"Player", "whiteTurret", "coin", "jetPackEnemy"};

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(string objectToDestroy in destroyable)
        {
            if(collision.gameObject.tag == objectToDestroy && objectToDestroy != "Player")
            {
                Destroy(collision.gameObject);
            }
            else if(collision.gameObject.tag == "Player")
            {
                Player.Instance.RespawnPlayer();
            }
        }        
    }
}
