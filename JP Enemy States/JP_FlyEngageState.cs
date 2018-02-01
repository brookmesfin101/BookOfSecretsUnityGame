using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JP_FlyEngageState : IEnemyState {

    private JetPackEnemy jetPackEnemy;

    public void Enter(JetPackEnemy jetPackEnemy)
    {
        this.jetPackEnemy = jetPackEnemy;
        jetPackEnemy.myAnimator.SetFloat("walkSpeed", 0);
        //jetPackEnemy.myAnimator.SetBool("flyUp", true);
    }

    public void Execute()
    {

        jetPackEnemy.myAnimator.SetBool("45_DegreeShot", true);
    }

    public void Exit()
    {
        if(!jetPackEnemy.HealthIsZero)
        {
            jetPackEnemy.FlyDown();
            jetPackEnemy.MyRigidBody.gravityScale = 1;
        }        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
