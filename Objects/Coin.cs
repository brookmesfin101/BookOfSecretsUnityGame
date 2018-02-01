using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private void Awake()
    {
        if(GameObject.FindObjectOfType<JetPackEnemy>())
        {
            JetPackEnemy[] jetpackEnemies = GameObject.FindObjectsOfType<JetPackEnemy>();

            foreach(JetPackEnemy jetpackenemy in jetpackEnemies)
            {
                Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), jetpackenemy.GetComponent<BoxCollider2D>(), true);
            }
        }        
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D point in collision.contacts)
        {
            if(point.collider.tag == "Player")
            {
                GetComponent<Animator>().SetTrigger("acquired");
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
