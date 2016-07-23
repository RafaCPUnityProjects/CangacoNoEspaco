using UnityEngine;
using System.Collections;

public class BananaSimpleWalk : MonoBehaviour {

    Rigidbody2D rb;

    Vector2 direction;
    public float walkForce = 1f;
    public float attackForce = 2f;

    public Animator enemyAnimator;
    public SpriteRenderer mainSprite;
    float changeTime = 0f;

    EnemyAttack enemyAttack;
    EnemyHealth enemyHealth;

    bool changed;   //running
    //bool attack;

    int previousLife;

	bool canAttackAgain = true;
	public float timeBewteenAttacks = 3f;

    void Awake () {
        rb = GetComponent<Rigidbody2D>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyHealth = GetComponent<EnemyHealth>();

        previousLife = enemyHealth.lives;
    }

    void Start()
    {
        enemyAnimator.SetTrigger("goUp");
        MoveTo(Vector2.up, walkForce);
    }

    void CheckAttack()
    {
        //only if banana is "look at" nim
        //if (direction == Vector2.left && transform.position.x > NimMove.instance.transform.position.x)
        //    attack = true;
        //if (direction == Vector2.right && transform.position.x < NimMove.instance.transform.position.x)
        //    attack = true;

        //only if banana is "look at" nim
        //if (direction == Vector2.down && transform.position.y > NimMove.instance.transform.position.y)
        //    attack = true;
        //if (direction == Vector2.up && transform.position.y < NimMove.instance.transform.position.y)
        //    attack = true;
    }

    void Update()
    {
        //Banana damaged
        if(previousLife != enemyHealth.lives)
        {
            changed = false;    //stop charge
            previousLife = enemyHealth.lives;

            //opposite direction
            if (direction == Vector2.left)
                MoveTo(Vector2.right, walkForce);
            else if (direction == Vector2.right)
                MoveTo(Vector2.left, walkForce);
            else if (direction == Vector2.up)
                MoveTo(Vector2.down, walkForce);
            else if (direction == Vector2.down)
                MoveTo(Vector2.up, walkForce);

        }

        if (direction == Vector2.left)
        {
            enemyAnimator.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if(direction == Vector2.right)
        {
            enemyAnimator.transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        //Changed = charged, attacking
        enemyAttack.enabled = changed;
		enemyAnimator.SetBool("rage", changed);
        
        changeTime -= Time.deltaTime;
        if (rb.velocity.magnitude <= 0.03f) {
            changeTime = 1f;

            if (direction == Vector2.up)
                enemyAnimator.SetTrigger("goDown");
            if (direction == Vector2.down)
                enemyAnimator.SetTrigger("goUp");
            if (direction == Vector2.left || direction == Vector2.right)
                enemyAnimator.SetTrigger("goLeftRight");

            ChangeDirection(walkForce);
            
            changed = false;
			//attack = false;
        }

		if (SimilarY() && !changed && canAttackAgain)
        {
            if (transform.position.x > NimMove.instance.transform.position.x)
            {
                enemyAnimator.SetTrigger("goLeftRight");
                MoveTo(Vector2.left, walkForce * attackForce);
            }
            else
            {
                enemyAnimator.SetTrigger("goLeftRight");
                MoveTo(Vector2.right, walkForce * attackForce);
            }
            
			canAttackAgain = false;
			Invoke ("EnableAttack", timeBewteenAttacks);

            changed = true;
			Debug.Log("Near Y");
        }

		if (SimilarX() && !changed && canAttackAgain)
        {
            if (transform.position.y > NimMove.instance.transform.position.y)
            {
                enemyAnimator.SetTrigger("goDown");
                MoveTo(Vector2.down, walkForce * attackForce);
            }
            else
            {
                enemyAnimator.SetTrigger("goUp");
                MoveTo(Vector2.up, walkForce * attackForce);
            }

			canAttackAgain = false;
			Invoke ("EnableAttack", timeBewteenAttacks);
                
            changed = true;
			Debug.Log("Near X");
        }

    }

    bool SimilarY()
    {
        //Dont change if banana back direction
        if (direction == Vector2.right && transform.position.x > NimMove.instance.transform.position.x)
            return false;
        if (direction == Vector2.down && transform.position.x < NimMove.instance.transform.position.x)
            return false;

        if (transform.position.y + 0.1f > NimMove.instance.transform.position.y)
        {
            if (transform.position.y - 0.1f < NimMove.instance.transform.position.y)
            {
                return true;
            }
                
        }
        return false;
            
    }

    bool SimilarX()
    {
        //Dont change if banana back direction
        if (direction == Vector2.up && transform.position.y > NimMove.instance.transform.position.y)
            return false;
        if (direction == Vector2.down && transform.position.y < NimMove.instance.transform.position.y)
            return false;

        if (transform.position.x + 0.1f > NimMove.instance.transform.position.x)
        {
            if (transform.position.x - 0.1f < NimMove.instance.transform.position.x)
            {
                return true;
            }
               
        }
        return false;

    }

    bool SimilarVector(Vector2 a, Vector2 b)
    {
        return (Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y));
    }

    public void ChangeDirection(float force)
    {
        MoveTo(Vector2.Reflect(-direction, rb.velocity.normalized), force);
    }
    
    void MoveTo(Vector2 dir, float force)
    {
        rb.velocity = Vector2.zero;
        direction = dir;
        rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
        
        //transform.LookAt(NimMove.instance.transform);
        //rb.AddRelativeForce(Vector3.forward * walkForce, ForceMode.Force);
    }
    
	void EnableAttack(){
		canAttackAgain = true;
	}
}
