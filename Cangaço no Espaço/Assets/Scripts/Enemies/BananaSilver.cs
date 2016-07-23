using UnityEngine;
using System.Collections;

public class BananaSilver : MonoBehaviour {

    public Animator enemyAnimator;
    Rigidbody2D rb;
    public float timeChasePlayer = 3f;
    public float force = 1f;

    SpriteRenderer sprite;

    bool chasing = true;


    void Awake () {
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
	}

    void Start()
    {
        InvokeRepeating("ChasePlayer", timeChasePlayer, timeChasePlayer);
    }
	
	void ChasePlayer() {

        if (chasing) {
            rb.velocity = Vector2.zero;
            rb.AddForce((transform.position - NimMove.instance.transform.position) * -force, ForceMode2D.Impulse);
        }
        
    }

    void Update()
    {
        sprite.flipX = (transform.position.x < NimMove.instance.transform.position.x);
    }

    public void StopChase()
    {
        chasing = false;
    }

    public void ReturnChase()
    {
        chasing = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMelee") || other.CompareTag("Bullet"))
        {
            rb.velocity = Vector2.zero;
            enemyAnimator.SetBool("rage", false);
        }
    }
}
