using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JP_PatrolState : IEnemyState {

    private JetPackEnemy jetPackEnemy;

    public void Enter(JetPackEnemy jetPackEnemy)
    {
        this.jetPackEnemy = jetPackEnemy;
    }

    public void Execute()
    {
        jetPackEnemy.Walk();
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
