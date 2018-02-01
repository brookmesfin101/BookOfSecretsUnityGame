using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JP_FallState : IEnemyState
{
    private JetPackEnemy jetPackEnemy;
    private float deathTimer;
    private float deathDuration = 10;
    private bool deathFadeOutComplete = false;
    private bool coinsSpawned = false;

    public void Enter(JetPackEnemy jetPackEnemy)
    {
        this.jetPackEnemy = jetPackEnemy;
        jetPackEnemy.myAnimator.SetTrigger("Fall");        
    }

    public void Execute()
    {
        //Death Fall
        jetPackEnemy.MyRigidBody.gravityScale = 2;
        if(!jetPackEnemy.IsGrounded())
        {
            jetPackEnemy.MyRigidBody.AddForce(new Vector2(0, -40));
        }
        else
        {
            if(!coinsSpawned)
            {
                jetPackEnemy.SpawnCoins(1);
                coinsSpawned = true;
            }
            
            if (deathTimer > deathDuration && !deathFadeOutComplete)
            {
                DeathFadeOut();
            }

            deathTimer += Time.deltaTime;
        }
    }

    public void Exit()
    {
        
    }

    private void DeathFadeOut()
    {
        Color spriteColor = jetPackEnemy.GetComponentInChildren<SpriteRenderer>().color;
        jetPackEnemy.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(spriteColor, new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0), .025f);
        if(spriteColor.a <= .02)
        {
            jetPackEnemy.DestroyEnemy(jetPackEnemy.gameObject);
            deathFadeOutComplete = true;            
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
