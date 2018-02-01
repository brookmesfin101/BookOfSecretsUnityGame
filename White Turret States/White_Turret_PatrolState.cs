using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class White_Turret_PatrolState : ITurretState {

    private WhiteTurret whiteturret;

    public void Enter(WhiteTurret whiteturret)
    {
        this.whiteturret = whiteturret;
    }

    public void Execute()
    {
        whiteturret.Walk();
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
}
