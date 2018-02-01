using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState {

    void Execute();
    void Enter(JetPackEnemy jetPackEnemy);
    void Exit();
    void OnTriggerEnter2D(Collider2D other);
    void OnTriggerExit2D(Collider2D other);
}
