using UnityEngine;
using System.Collections;
//using System;

public class NPCController : MonoBehaviour
{
	public bool NPC = true;
	public Animator myAnimator;

	public tk2dSprite bodySprite;
	public tk2dSprite headSprite;
	public tk2dSprite faceSprite;
	public tk2dSprite shirtSprite;
	public tk2dSprite pantSprite;
	public tk2dSprite feetSprite;
	public tk2dSprite knifeSprite;

	//public tk2dSpriteCollection bodyCollection;
	//public tk2dSpriteCollection headCollection;
	//public tk2dSpriteCollection faceCollection;
	//public tk2dSpriteCollection shirtCollection;
	//public tk2dSpriteCollection pantCollection;
	//public tk2dSpriteCollection knifeCollection;

	public int bodyCount = 4;
	public int headCount = 9;
	public int faceCount = 5;
	public int shirtCount = 8;
	public int pantCount = 5;
	public int feetCount = 1;
	public int knifeCount = 6;

	public float moveSpeed = 0.01f;

	public BoxCollider2D peixeiraCollider;

	private Vector2 moveVector;
	private bool walking;
	private bool facingLeft = true;
	private int strenght;

	void Start()
	{
		//myAnimator = GetComponent<Animator>();
	}

	void Update()
	{
		if (!NPC)
		{
			moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

			if (Input.GetButtonDown("Fire1"))
			{
				myAnimator.SetTrigger("Attack");
			}

			if (Input.GetButtonDown("Fire2"))
			{
				RandomClothing();
			}
		}

		if (facingLeft && moveVector.x > 0 || !facingLeft && moveVector.x < 0)
		{
			Flip();
		}

		walking = moveVector != Vector2.zero;

		myAnimator.SetBool("Walking", walking);

		
		if (walking)
		{
			Move();
		}
	}

	void Move()
	{
		transform.Translate(moveVector * moveSpeed);
	}

	void Flip()
	{
		facingLeft = !facingLeft;
		Vector3 originalScale = transform.localScale;
		transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
	}

	void RandomClothing()
	{
		int body = Random.Range(0, bodyCount);
		int head = Random.Range(-1, headCount);
		int face = Random.Range(-1, faceCount);
		int shirt = Random.Range(-1, shirtCount);
		int pant = Random.Range(0, pantCount);
		int feet = Random.Range(-1, feetCount);
		int knife = Random.Range(0, bodyCount);

		bodySprite.spriteId = body;
		if (head >= 0)
		{
			headSprite.gameObject.SetActive(true);
			headSprite.spriteId = head;
		}
		else
		{
			headSprite.gameObject.SetActive(false);
		}
		if (face >= 0)
		{
			faceSprite.gameObject.SetActive(true);
			faceSprite.spriteId = head;
		}
		else
		{
			faceSprite.gameObject.SetActive(false);
		}
		if (shirt >= 0)
		{
			shirtSprite.gameObject.SetActive(true);
			shirtSprite.spriteId = head;
		}
		else
		{
			shirtSprite.gameObject.SetActive(false);
		}
		pantSprite.spriteId = pant;
		if (feet >= 0)
		{
			feetSprite.gameObject.SetActive(true);
			feetSprite.spriteId = head;
		}
		else
		{
			feetSprite.gameObject.SetActive(false);
		}
		knifeSprite.spriteId = knife;

		RescaleTk2dSpriteCollider(knifeSprite);
	}

	void RescaleTk2dSpriteCollider(tk2dSprite target)
	{
		Vector2 S = target.GetBounds().size;
		BoxCollider2D targetCollider = target.GetComponent<BoxCollider2D>();
		targetCollider.size = S;
		targetCollider.offset = Vector2.zero;// = new Vector2((S.x/2f), (S.y/2f));
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("NPC Attack"))
		{
			if (peixeiraCollider.IsTouching(other))
			{
				if (other.tag == "Punchable")
				{
					other.GetComponent<NPCController>().TakeDamage(strenght);
				}
			}
		}
	}

	public void TakeDamage(int strenght)
	{
		Debug.Log(name + " took " + strenght + " damage");
	}
}
