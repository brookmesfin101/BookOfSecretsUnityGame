using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

    [SerializeField]
    private Collider2D other;

    [SerializeField]
    private Collider2D coin;

    private void Awake()
    {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), other, true);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), coin, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 13);
    }
}
