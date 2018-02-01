using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class White_Turret_EngageState : ITurretState {
    WhiteTurret whiteturret;

    public void Enter(WhiteTurret whiteturret)
    {
        this.whiteturret = whiteturret;
        whiteturret.myAnimator.SetBool("engage", true);
        whiteturret.myAnimator.SetFloat("walkSpeed", 0);
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        whiteturret.myAnimator.SetBool("engage", false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

    }

    public void OnTriggerExit2D(Collider2D other)
    {

    }
}
