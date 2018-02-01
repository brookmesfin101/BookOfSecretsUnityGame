using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField]
    private int life;

    [SerializeField]
    private int maxLife;

    private bool alive;

    public int Life
    {
        get
        {
            return life;
        }

        set
        {
            life = value;
        }
    }

    public bool Alive
    {
        get
        {
            return alive;
        }

        set
        {
            alive = value;
        }
    }

    public int MaxLife
    {
        get
        {
            return maxLife;
        }

        set
        {
            maxLife = value;
        }
    }

    // Use this for initialization
    void Start () {
        Alive = true;
        Life = MaxLife;
	}
	
	// Update is called once per frame
	void Update () {
        
	}    

    public void ResetHealth()
    {
        Life = MaxLife;
        Alive = true;
    }
}
