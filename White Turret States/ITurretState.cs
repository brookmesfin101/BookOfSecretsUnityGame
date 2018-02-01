using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretState{

    void Execute();
    void Enter(WhiteTurret whiteturret);
    void Exit();
    void OnTriggerEnter2D(Collider2D other);
    void OnTriggerExit2D(Collider2D other);
}
