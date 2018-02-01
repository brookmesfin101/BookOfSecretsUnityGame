using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class White_Turret_IdleState : ITurretState
{

    private WhiteTurret whiteturret;

    private float idleDuration;

    private float idleTimer;

    public void Enter(WhiteTurret whiteturret)
    {
        this.whiteturret = whiteturret;
        idleDuration = UnityEngine.Random.Range(5, 10);
    }

    public void Execute()
    {
        Idle();
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

    private void Idle()
    {
        whiteturret.myAnimator.SetFloat("walkSpeed", 0);

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            whiteturret.ChangeState(new White_Turret_PatrolState());
        }
    }
}
