using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JP_IdleState : IEnemyState {

    private JetPackEnemy jetPackEnemy;

    private float idleDuration;

    private float idleTimer;

    public void Enter(JetPackEnemy jetPackEnemy)
    {
        this.jetPackEnemy = jetPackEnemy;
        idleDuration = UnityEngine.Random.Range(5, 10);
    }

    public void Execute()
    {    
        Idle();
        //Debug.Log("idleTimer = " + idleTimer);
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
        jetPackEnemy.myAnimator.SetFloat("walkSpeed", 0);

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            jetPackEnemy.ChangeState(new JP_PatrolState());
        }
    }
}
