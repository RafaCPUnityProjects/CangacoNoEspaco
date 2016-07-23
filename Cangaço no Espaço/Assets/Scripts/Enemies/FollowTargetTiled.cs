using UnityEngine;
using System.Collections;

public class FollowTargetTiled : MonoBehaviour
{
	private const float raycastSize = 0.5f;
	//bool walk = true;
	public Transform target;

	bool facingRight = true;

	Rigidbody2D rb;
	public float walkForce = 1.2f;
	Vector3 normalizedMove;

	public bool walkAway = false;
	//float jumpTime = 0.5f;

	float tileSize = 0.21f; //physic tile size, not pixel size

	public Animator animator;

	//Set by animation
	public bool timeToJump;

	public LayerMask wallLayerMask;

	EnemyHealth health;

	//Bounds bounds;

	public bool followForever;

    Vector3 prevPosition;

    float timeJumpToPlayer = 0f;
    const float T_JUMPTOPLAYER = 3f;

    //PUSH EXPLOSION
    bool pushed;
    Vector3 pushDir;
    public float pushForce = 1f;
    public float pushTime = 0.5f;


    void Awake()
	{
		//bounds = GetComponent<CircleCollider2D>().bounds;
		rb = GetComponent<Rigidbody2D>();

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("PlayerNear"))
		{
			target = other.transform;
		}
	}

	void Start()
	{
        prevPosition = transform.position;

        health = GetComponent<EnemyHealth>();

		if (followForever)
			target = FindObjectOfType<NimMove> ().transform;
	}

	void Update()
	{
        //PUSH EXPLOSION
        if (pushed)
        {
            pushed = false;
            //Invoke("StopPushing", pushTime);
            rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
            return;
        }

        if (health.died)
		{
			rb.velocity = Vector2.zero;
			return;
		}

		if (target == null)
		{
			return;
		}

        //Blocked on a wall
        /*if (prevPosition == transform.position)
        {
            rb.AddForce((target.position - transform.position).normalized * walkForce, ForceMode2D.Impulse);
            return;
        }*/

        //if (prevPosition == transform.position)

        timeJumpToPlayer -= Time.deltaTime;
        if (prevPosition == transform.position && timeJumpToPlayer <= 0f)
        {
            timeJumpToPlayer = T_JUMPTOPLAYER;
            rb.velocity = Vector2.zero;
            rb.AddForce((target.position - transform.position).normalized * walkForce, ForceMode2D.Impulse);
            return;
        }



        if (timeToJump)
		{
			timeToJump = false;
			Jump();
		}

		//Flip sprite
		if (target.position.x < transform.position.x && facingRight)
		{
			Flip();
		}
		if (target.position.x > transform.position.x && !facingRight)
		{
			Flip();
		}

        prevPosition = transform.position;
        
    }

	void Jump()
	{
		if (Mathf.RoundToInt(target.position.x / tileSize * 2) == Mathf.RoundToInt(transform.position.x / tileSize * 2))
		{
			JumpX();
		}
		else if (Mathf.RoundToInt(target.position.y / tileSize * 2) == Mathf.RoundToInt(transform.position.y / tileSize * 2))
		{
			JumpY();
		}
		else
		{
			float rnd = Random.value;

			if (rnd < 0.4f)
			{
				JumpX();
			}
			else if (rnd < 0.8f)
			{
				JumpY();
			}
			else
			{
				Stop();
			}
		}
	}

	void JumpX(bool forceJump = false)
	{
		rb.velocity = Vector2.zero;

		Vector2 forceDirection = Vector2.zero;

		if (target.position.x < transform.position.x) //target is left
		{
			if (walkAway)//run away, go right
			{
				RaycastHit2D raycastRightHit = Physics2D.Raycast(transform.position, Vector2.right, raycastSize, wallLayerMask);
				if (!forceJump && raycastRightHit.collider != null)//right path blocked
				{
					JumpY(true);
				}
				else
				{
					forceDirection = Vector3.right;
				}
			}
			else//chase target
			{
				forceDirection = Vector2.left;
			}
		}
		else //target is right
		{
			if (walkAway)//run away, go left
			{
				RaycastHit2D raycastLeftHit = Physics2D.Raycast(transform.position, Vector2.left, raycastSize, wallLayerMask);
				if (!forceJump && raycastLeftHit.collider != null)//left path blocked
				{
					JumpY(true);
				}
				else
				{
					forceDirection = Vector3.left;
				}
			}
			else//chase target
			{
				forceDirection = Vector2.right;
			}
		}
		rb.AddForce(forceDirection * walkForce, ForceMode2D.Impulse);

		if (animator != null)
		{
			animator.SetTrigger("jump");
		}
	}

	void JumpY(bool forceJump = false)
	{
		rb.velocity = Vector2.zero;

		Vector2 forceDirection = Vector2.zero;

		if (target.position.y < transform.position.y) //target is down
		{
			if (walkAway)//run away, go up
			{
				RaycastHit2D raycastUpHit = Physics2D.Raycast(transform.position, Vector2.up, raycastSize, wallLayerMask);
				if (!forceJump && raycastUpHit.collider != null)//up path blocked
				{
					JumpX(true);
				}
				else
				{
					forceDirection = Vector3.up;
				}
			}
			else//chase target
			{
				forceDirection = Vector2.down;
			}
		}
		else //target is up
		{
			if (walkAway)//run away, go down
			{
				RaycastHit2D raycastDownHit = Physics2D.Raycast(transform.position, Vector2.down, raycastSize, wallLayerMask);
				if (!forceJump && raycastDownHit.collider != null)//down path blocked
				{
					JumpX(true);
				}
				else
				{
					forceDirection = Vector3.down;
				}
			}
			else//chase target
			{
				forceDirection = Vector2.up;
			}
		}
		rb.AddForce(forceDirection * walkForce, ForceMode2D.Impulse);

		if (animator != null)
		{
			animator.SetTrigger("jump");
		}
	}

	public void Stop()
	{
		rb.velocity = Vector2.zero;
		//walk = false;
	}

	public void Move()
	{
		//walk = true;
	}

	void Flip()
	{
		// Switch the way the player is labelled as facing
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public Vector3 GetDirection()
	{
		Vector3 direction = facingRight ? Vector3.right : Vector3.left;
		return direction;
	}

    //PUSH EXPLOSION
    
    public void Push(Vector3 direction)
    {
        pushed = true;
        pushDir = direction;
    }

    void StopPushing()
    {
        pushed = false;
    }
}
