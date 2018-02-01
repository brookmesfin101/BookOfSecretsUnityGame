using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {

    public Animator myAnimator;

    protected bool facingRight = true;

    protected int enemyDirection = 1;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private GameObject coinPrefab;

    private bool healthIsZero = false;

    private bool respawning = false;

    private bool beingDamaged = false;

    [SerializeField]
    protected Transform coinSpawnPosition;

    public bool HealthIsZero
    {
        get
        {
            return healthIsZero;
        }

        set
        {
            healthIsZero = value;
        }
    }

    protected bool BeingDamaged
    {
        get
        {
            return beingDamaged;
        }

        set
        {
            beingDamaged = value;
        }
    }

    protected bool Respawning
    {
        get
        {
            return respawning;
        }

        set
        {
            respawning = value;
        }
    }

    // Use this for initialization
    public virtual void Start () {
        myAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected void ChangeDirection(float horizontal)
    {
        if(facingRight == true && horizontal < 0)
        {
            facingRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if(facingRight == false && horizontal > 0)
        {
            facingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    protected void ChangeEnemyDirection()
    {
        if(facingRight == true)
        {
            facingRight = false;
            enemyDirection = -1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if(facingRight == false)
        {
            facingRight = true;
            enemyDirection = 1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public bool IsGrounded()
    {
        var grounded = false;
        foreach (Transform point in groundPoints)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    grounded = true;
                }
            }
        }
        return grounded;
    }

    public IEnumerator Delay(int multiplier)
    {
        yield return new WaitForSeconds(.2f * multiplier);
    }

    private IEnumerator FlashCharacter(GameObject character, bool isDamage)
    {
        if(isDamage && character.GetComponentInChildren<SpriteRenderer>())
        {
            var sprite = character.GetComponentInChildren<SpriteRenderer>();
            sprite.color = Color.red;
            yield return new WaitForSeconds(.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(.1f);
            sprite.color = Color.red;
            yield return new WaitForSeconds(.1f);
            sprite.color = Color.white;
        }
    }

    protected IEnumerator RespawnFlashCharacter(GameObject character)
    {
        Color spriteColor = character.GetComponentInChildren<SpriteRenderer>().color;
        yield return new WaitForSeconds(.2f);
        character.GetComponentInChildren<SpriteRenderer>().color = Color.clear;
        yield return new WaitForSeconds(.2f);
        character.GetComponentInChildren<SpriteRenderer>().color = spriteColor;
        yield return new WaitForSeconds(.2f);
        character.GetComponentInChildren<SpriteRenderer>().color = Color.clear;
        yield return new WaitForSeconds(.2f);
        character.GetComponentInChildren<SpriteRenderer>().color = spriteColor;
        respawning = false;
    }

    public void InitateRespawnFlashCharacter(GameObject character)
    {
        if (character)
        {
            StartCoroutine(RespawnFlashCharacter(character));
        }
        else
        {
            Debug.Log("Character passed to RespawnFlashCharacter is null");
        }
    }

    public void StopRespawnFlashCharacter(GameObject character)
    {
        if (character)
        {
            StopCoroutine(RespawnFlashCharacter(character));
        }
        else
        {
            Debug.Log("Character passed to RespawnFlashCharacter is null");
        }
    }

    public void DoDamage(int damage, GameObject characterDamaged)
    {
        if(characterDamaged.GetComponent<Health>())
        {
            Debug.Log(characterDamaged.name + " damaged");
            characterDamaged.GetComponent<Character>().BeingDamaged = true;
            characterDamaged.GetComponent<Health>().Life = characterDamaged.GetComponent<Health>().Life - damage;
            if(characterDamaged.tag == "Player")
            {
                HealthBar.Instance.TakeDamage(damage);
            }            
            StartCoroutine(FlashCharacter(characterDamaged, true));
        }
        else
        {
            Debug.LogWarning("No Health Component Attached to Character DoDamage was called on");
        }
    }

    public void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }

    public void SpawnCoins(int numberOfCoins)
    {
        for (var i = 0; i < numberOfCoins; i++)
        {
            GameObject coin = Instantiate(coinPrefab, new Vector3(coinSpawnPosition.position.x, coinSpawnPosition.position.y, coinSpawnPosition.position.z), transform.rotation);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), coin.GetComponent<Collider2D>());
        }
    }
}
