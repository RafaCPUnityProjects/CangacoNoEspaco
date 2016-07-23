using UnityEngine;
using System.Collections;

public class BananaSilverAttack : MonoBehaviour {

    public Animator enemyAnimator;
    Rigidbody2D rb;

    public float timeAttackAgain = 2f;
    bool canAttack = true;

    public float attackForce = 1.6f;
    public float timeAttack = 1f;

    public float attackRangeDistance = 0.1f;

    BananaSilver bananaSilver;

    float TIME_CHECKATTACK = 0.1f;

    enum LookDirection {UP, DOWN, LEFT, RIGHT }
    LookDirection ld = LookDirection.LEFT;

	public float distanceAttack = 1f;

	bool isAttacking = false;

    void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>(); 
    }

    void Start()
    {
        InvokeRepeating("CheckAttack", TIME_CHECKATTACK, TIME_CHECKATTACK);
        bananaSilver = transform.parent.GetComponent<BananaSilver>();
    }

	void CheckAttack()
    {
		if(!isAttacking)
        	LookToPlayer();

        if (canAttack)
            Attack();
    }

    void Attack()
    {
		//Check distance
		if (Vector2.Distance (NimMove.instance.transform.position, transform.position) > distanceAttack)
			return;


        if ((transform.position.y < NimMove.instance.transform.position.y + attackRangeDistance) && (transform.position.y > NimMove.instance.transform.position.y - attackRangeDistance))
        {
            if (transform.position.x < NimMove.instance.transform.position.x)
            {
                //enemyAnimator.SetTrigger("goLeftRight");
                MoveTo(Vector2.right, attackForce);
            }
            else
            {
                //enemyAnimator.SetTrigger("goLeftRight");
                MoveTo(Vector2.left, attackForce);
            }
        }


        if ((transform.position.x < NimMove.instance.transform.position.x + attackRangeDistance) && (transform.position.x > NimMove.instance.transform.position.x - attackRangeDistance))
        {
            if (transform.position.y < NimMove.instance.transform.position.y)
            {
                //enemyAnimator.SetTrigger("goUp");
                MoveTo(Vector2.up, attackForce);
            }
            else
            {
                //enemyAnimator.SetTrigger("goDown");
                MoveTo(Vector2.down, attackForce);
            }
        }
        
        canAttack = false;
		Invoke("EnableAttack", timeAttackAgain);
    }

    void LookToPlayer()
    {

        if ((transform.position.y < NimMove.instance.transform.position.y + attackRangeDistance) && (transform.position.y > NimMove.instance.transform.position.y - attackRangeDistance))
        {
            if (transform.position.x < NimMove.instance.transform.position.x)
            {
                if(ld != LookDirection.LEFT)
                    enemyAnimator.SetTrigger("goLeftRight");
                ld = LookDirection.LEFT;
            }
            else
            {
                if (ld != LookDirection.RIGHT)
                    enemyAnimator.SetTrigger("goLeftRight");
                ld = LookDirection.RIGHT;
            }
        }


        if ((transform.position.x < NimMove.instance.transform.position.x + attackRangeDistance) && (transform.position.x > NimMove.instance.transform.position.x - attackRangeDistance))
        {
            if (transform.position.y < NimMove.instance.transform.position.y)
            {
                if (ld != LookDirection.UP)
                    enemyAnimator.SetTrigger("goUp");
                ld = LookDirection.UP;
            }
            else
            {
                if (ld != LookDirection.DOWN)
                    enemyAnimator.SetTrigger("goDown");
                ld = LookDirection.DOWN;
            }
        }
        
    }

    void EnableAttack()
    {
        canAttack = true;
    }

    void MoveTo(Vector2 dir, float force)
    {
		isAttacking = true;

        rb.velocity = Vector2.zero;
        //direction = dir;
        rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);

        bananaSilver.StopChase();
        Invoke("Rage", 0.1f);
    }

    void Rage()
    {
        enemyAnimator.SetBool("rage", true);
        Invoke("StopRage", timeAttack);
    }

    void StopRage()
    {
		isAttacking = false;
        bananaSilver.ReturnChase();
        enemyAnimator.SetBool("rage", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerMelee") || other.CompareTag("Bullet"))
        {
            rb.velocity = Vector2.zero;
            enemyAnimator.SetBool("rage", false);
        }
       
    }
}
