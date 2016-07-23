using UnityEngine;
using System.Collections;

public class NimMove : MonoBehaviour
{
    public static NimMove instance;

	public Vector2 walkSpeed = Vector2.one;
	public bool walkFacingForward = true;
	SpriteRenderer sprite;
	NimHealth health;
	Vector2 input;
	//Transform myTransform;

	Rigidbody2D rb;
	[Range(0.5f, 2f)]
	public float walkForce = 1f;
	Vector3 normalizedMove;

	bool stopMove = false;
	bool lookRight = true;  //default look position
	//bool lookDown = false;
	Vector2 lastDirection = Vector2.zero;

	//current animation
	public Animator nimAnimator;
	static int attackState = Animator.StringToHash("Base Layer.attack");
	static int attackStateUp = Animator.StringToHash("Base Layer.attackUp");
	static int attackStateDown = Animator.StringToHash("Base Layer.attackDown");
	static int shootState = Animator.StringToHash("Base Layer.shoot");
	static int dieState = Animator.StringToHash("Base Layer.die");

	AnimatorStateInfo currentState;

	float joystickDeadzone = 0.1f;

	bool dashing;
	FadeTrail trail;
	public float dashForce = 2f;

	public bool paralized;
	public Color paralisisColor = Color.blue;
	Color originalColor;

	//Explosion push
	bool pushed;
	Vector3 pushDir;
	public float pushForce = 1f;
	public float pushTime = 0.5f;

    WeaponController wc;

	void Awake()
	{
        if(instance == null)
            instance = this;

		//myTransform = transform;
		health = GetComponent<NimHealth>();

		rb = GetComponent<Rigidbody2D>();
	}

	void Start(){
		sprite = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
		originalColor = sprite.color;

		trail = sprite.GetComponent<FadeTrail> ();

        wc = FindObjectOfType<WeaponController>();

    }

	void Update()
	{
		if (paralized) {
			return;
		}

		if (GameController.instance.IsGamePaused())
			return;

		if (stopMove || health.Died())
			return;

        //Vector2 moveDirecion = Vector2.zero;
        input.x = Input.GetAxisRaw("Horizontal");
		input.y = Input.GetAxisRaw("Vertical");

		if (input.magnitude < joystickDeadzone)
		{
			input = Vector2.zero;
		}

		//input = new Vector3(h, v, 0f);

		normalizedMove = new Vector3(input.normalized.x * walkSpeed.x, input.normalized.y * walkSpeed.y, 0f);
		//myTransform.Translate (normalizedMove * Time.deltaTime);


		FlipControl();

		//Save last look direction
		if (input.x != 0 || input.y != 0)
		{
			lastDirection = GetLookDirection();
		}

    }

	void FixedUpdate()
	{
		if (GameController.instance.IsGamePaused())
			return;

		if (paralized) {
			return;
		}

		AnimationControl();
		KeyControl();

		if (Input.GetButtonDown("Fire2") && (input.x != 0 || input.y != 0)) {

            //if energy melee, take energy and dash
            if (GameSave.nimWeapon == CurrentWeapon.PineappleGun)
            {
                if (wc.pineappleGun.ammoSlider.CanDash())
                {
                    Dash();
                }
			}
            else if (GameSave.nimWeapon == CurrentWeapon.WatermelonGun)
            {
                if (wc.watermelonGun.ammoSlider.CanDash())
                {
                    Dash();
                }
            }
            else if (GameSave.nimWeapon == CurrentWeapon.BananaGun)
            {
                if (wc.bananaGun.ammoSlider.CanDash())
                {
                    Dash();
                }
            }
            else if (GetComponent<NimAttack>().energyHud.CanDash())
            {
                Dash();
            }
        }

		if (health.Died ()) {
			rb.velocity = Vector2.zero;
			return;
		}
		
		if (!dashing) {
			if(stopMove)
				rb.velocity = Vector2.zero;
			else
				rb.velocity = normalizedMove * walkForce;
		}

		if (pushed) {
			Invoke ("StopPushing",pushTime);
			rb.AddForce (pushDir * pushForce, ForceMode2D.Force);
		}
			
	}

    void Dash()
    {
        dashing = true;
        health.InvincibleDash();

        rb.AddForce(CardinalVector(normalizedMove) * walkForce * dashForce, ForceMode2D.Impulse);
        //rb.AddForce (lastDirection * walkForce * dashForce, ForceMode2D.Impulse);
        trail.Enable();

        Invoke("StopDash", 0.1f);
    }

    public bool isDashing()
    {
        return dashing;
    }

    Vector3 CardinalVector(Vector3 direction)
    {
        //http://answers.unity3d.com/questions/31744/how-do-i-snap-vector-to-the-nearest-cardinal-value.html
        //Check which component of the vector(x, y, or z) has the biggest absolute value.
        //Make that component 1, and all others 0.
        //Change the sign to the original sign of that component.

        Vector3 snapCardinalDirection = Vector3.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                //X
                if (direction.x > 0)
                    snapCardinalDirection = new Vector3(1, 0, 0);
                else
                    snapCardinalDirection = new Vector3(-1, 0, 0);
            }
        }
        else
        {
            if (Mathf.Abs(direction.y) > Mathf.Abs(direction.z))
            {
                //Y
                if (direction.y > 0)
                    snapCardinalDirection = new Vector3(0, 1, 0);
                else
                    snapCardinalDirection = new Vector3(0, -1, 0);
            }
            else
            {
                //Z
                if (direction.z > 0)
                    snapCardinalDirection = new Vector3(0, 0, 1);
                else
                    snapCardinalDirection = new Vector3(0, 0, -1);
            }
        }

        return snapCardinalDirection;
    }

	public void Push(Vector3 direction){
		pushed = true;
		pushDir = direction;
	}

	void StopPushing(){
		pushed = false;
	}

	public void Paralize(float seconds){
		paralized = true;
		rb.velocity = Vector2.zero;
		sprite.color = paralisisColor;

		Invoke ("StopParalize", seconds);
	}

	void StopParalize(){
		paralized = false;
		sprite.color = originalColor;
	}

	void StopDash(){
		dashing = false;
		trail.Stop ();
	}

	private void AnimationControl()
	{
		if (nimAnimator == null)
			return;

		//Check player state animation
		currentState = nimAnimator.GetCurrentAnimatorStateInfo(0);

		stopMove = (currentState.fullPathHash == attackState
			|| currentState.fullPathHash == attackStateUp
			|| currentState.fullPathHash == attackStateDown
			|| currentState.fullPathHash == shootState
			|| currentState.fullPathHash == dieState);
	}

	private void KeyControl()
	{
		if (Input.GetButton("Fire1"))
		{
			stopMove = true;
		}
        
    }

	private void FlipControl()
	{
		if (input.x > 0f && !lookRight)
		{
			lookRight = !lookRight;
			Flip();
		}

		if (input.x < 0f && lookRight)
		{
			lookRight = !lookRight;
			Flip();
		}
	}

	public Vector2 GetInput()
	{
		return input;
	}

	public Vector2 GetLookDirection()
	{
		Vector2 output = lastDirection;

		if (input != Vector2.zero)
		{
			if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
			{
				if (input.x >= 0)
				{
					output = Vector2.right;
				}
				else
				{
					output = Vector2.left;
				}
			}
			else
			{
				if (input.y >= 0)
				{
					output = Vector2.up;
				}
				else
				{
					output = Vector2.down;
				}
			}
		}
		return output;
	}

	void Flip()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
