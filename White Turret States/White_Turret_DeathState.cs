using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class White_Turret_DeathState : ITurretState {
    WhiteTurret whiteTurret;

    public void Enter(WhiteTurret whiteturret)
    {
        this.whiteTurret = whiteturret;
        whiteTurret.myAnimator.SetTrigger("death");
        whiteTurret.HealthComponent.Alive = false;
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
