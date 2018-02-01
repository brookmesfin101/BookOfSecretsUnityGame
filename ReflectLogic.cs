using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectLogic : MonoBehaviour {

    [SerializeField]
    private int damage;

    [SerializeField]
    private int angle;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.GetType() == typeof(CapsuleCollider2D) && collider2D.gameObject.tag == "Player" && (Player.Instance.myAnimator.GetInteger("currentState") == 5 || Player.Instance.myAnimator.GetInteger("currentState") == 6))
        {
            if(Player.Instance.MyAudioSource)
            {
                Player.Instance.MyAudioSource.clip = Player.Instance.BlockSound;
                Player.Instance.MyAudioSource.Play();
            }            
            Player.Instance.myAnimator.SetTrigger("blockingHit");
            float yReflection = Random.Range(-10, 10);
            float yRotation = 180;

            Vector2 objectVelocity = GetComponent<Rigidbody2D>().velocity;            
            Vector2 reflectVelocity = new Vector2(-objectVelocity.x, yReflection);

            ReduceSpriteAlpha();

            Player.Instance.Blocking(gameObject);

            if(Player.Instance.GetComponent<Transform>().position.x - GetComponentInParent<Transform>().position.x > 0)
            {
                yRotation = 0;
            }

            if (gameObject.tag == "bullet")
            {
                if(yRotation == 180)
                {
                    Vector2 reflect = Vector2.Reflect(objectVelocity, Vector2.right);
                    //Debug.Log(reflect);
                    //Debug.Log(Vector2.Angle(objectVelocity, reflect));
                    GetComponent<Transform>().Rotate(new Vector3(0, yRotation, 180 - Vector2.Angle(objectVelocity, reflect)));
                    GetComponent<Rigidbody2D>().velocity = reflect;
                }
                else
                {
                    Vector2 reflect = Vector2.Reflect(objectVelocity, Vector2.left);
                    Debug.Log(Vector2.Angle(objectVelocity, reflect));
                    GetComponent<Transform>().Rotate(new Vector3(0, yRotation, Vector2.Angle(objectVelocity, reflect) + angle));
                    Debug.Log(Vector2.Angle(objectVelocity, reflect));
                    GetComponent<Rigidbody2D>().velocity = reflect;
                }
            }
            else
            {
                GetComponent<Transform>().Rotate(0, yRotation, AngleBetweenVector2(objectVelocity, reflectVelocity));
                GetComponent<Rigidbody2D>().velocity = reflectVelocity;
            }
        }
        else if(collider2D.gameObject.tag == "Player")
        {
            if (Player.Instance.MyAudioSource)
            {
                Player.Instance.MyAudioSource.clip = Player.Instance.HitSound;
                Player.Instance.MyAudioSource.Play();
            }

            switch (gameObject.tag)
            {
                case "bullet": Player.Instance.DoDamage(5, Player.Instance.gameObject); break;
                case "TurretShots": Player.Instance.DoDamage(10, Player.Instance.gameObject); break;
            }
            Destroy(gameObject);

            //Player.Instance.DoDamage(10, Player.Instance.gameObject);
            //HealthBar.Instance.TakeDamage(damage);
        }
    }

    private void ReduceSpriteAlpha()
    {
        Component[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, .82f);
        }
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 difference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, difference) * sign;
    }
}
