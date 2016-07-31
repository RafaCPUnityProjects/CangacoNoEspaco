using System;
using UnityEngine;
using System.Collections;

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

	public int bodyCount = 4;
	public int headCount = 9;
	public int faceCount = 5;
	public int shirtCount = 8;
	public int pantCount = 5;
	public int feetCount = 1;
	public int knifeCount = 6;

	public BoxCollider2D peixeiraCollider;

	public Transform target;

	public GameObject[] possibleDrops;
	[Range(0.0f, 1.0f)]
	public float dropChance;

	public float minDistance = 1f;
	public float maxDistance = 5f;
	public bool imTheBoss = false;

	[SerializeField]
	public BodyInfo myBodyInfo;

	private Vector2 moveVector;
	private bool walking;
	private bool facingLeft = true;
	//private int currentStrength;

	private float distance;
	private int currentLife;
	private bool dead = false;

	public NDJGameController gameController;
	public bool canMove = true;
	private float speedMultiplier = 0.01f;

	void Start()
	{
		if (NPC)
		{
			target = GameObject.FindGameObjectWithTag("Player").transform;
		}
		else
		{
			RandomizeMyClothing();

		}
		currentLife = myBodyInfo.maxLife;
		//currentStrength = myBodyInfo.strength;
	}

    void Alerta()
    {
       
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("NPC Attack"))
		{
			if (peixeiraCollider.IsTouching(other))
			{
				if (other.tag == "Punchable")
				{
					other.transform.parent.parent.GetComponent<NPCController>().TakeDamage(myBodyInfo.strength);
				}
				else if (other.tag == "Tall Grass")
                    
                {
					Destroy(other.gameObject);
                    RetroJukebox.control.PlayOneShot("Grama", transform.position);
                }
				else if (other.tag == "Barril")
				{
					other.GetComponent<BarrilController>().DropItem();
                    RetroJukebox.control.PlayOneShot("BateBarril", transform.position);

                }
			}
		}
	}
   
	void Update()
	{
		if (dead || !canMove)
		{
			moveVector = Vector2.zero;
		}
		else if (!NPC)
		{
			moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

			if (Input.GetButtonDown("Fire1"))
			{
                RetroJukebox.control.Play("PAtaque", transform.position);
                if (myBodyInfo.knife != -1)
				{
					myAnimator.SetTrigger("Attack");
                    
                }
			}

			//if (Input.GetButtonDown("Fire2"))
			//{
			//	RandomizeMyClothing();
			//}
		}
		else
		{
			distance = Vector3.Distance(transform.position, target.position);
			//Vector3 direction = target.position - transform.position;
			//Debug.DrawRay(transform.position, direction);
			//RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
			//Debug.Log("Hit: " + hit.collider.name);
			//if (hit.collider.tag != "Wall")
			//{
            if (distance < minDistance)
                RetroJukebox.control.Play("IAlerta", transform.position);
            if (distance < maxDistance)
            {
               
                if (distance > minDistance)
				{
                   
                    moveVector = Vector3.Normalize(target.position - transform.position);
                               

                }
				else
				{
					if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("NPC Attack"))
					{
						if (myBodyInfo.knife != -1)
						{
							myAnimator.SetTrigger("Attack");                           
                            RetroJukebox.control.Play("IAtaque", transform.position);
                           // RetroJukebox.control.Play("IAlerta", transform.position);
                           // RetroJukebox.control.Stop("IAlerta", transform.position);
                        }
					}
				}
			}
			//}
			else
			{
				moveVector = Vector2.zero;
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
		transform.Translate(moveVector * myBodyInfo.speed * speedMultiplier);
       
    }

	void Flip()
	{
		facingLeft = !facingLeft;
		Vector3 originalScale = transform.localScale;
		transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
	}

	public void RandomizeMyClothing(bool starterPlayer = false)
	{
		int body = UnityEngine.Random.Range(0, bodyCount);
		int head = UnityEngine.Random.Range(-1, headCount);
		int face = UnityEngine.Random.Range(-1, faceCount);
		int shirt = UnityEngine.Random.Range(-1, shirtCount);
		int pant = UnityEngine.Random.Range(0, pantCount);
		int feet = UnityEngine.Random.Range(-1, feetCount);
		int knife;
		if (starterPlayer)
		{
			knife = -1;
		}
		else
		{
			knife = UnityEngine.Random.Range(0, knifeCount);
		}

		myBodyInfo = new BodyInfo(body, head, face, shirt, pant, feet, knife);

		DressPlayer();
	}

	public void LoadBossInfo()
	{
		myBodyInfo = myBodyInfo.GetSavedBody();
		imTheBoss = true;
		DressPlayer();
	}

	public void DressPlayer(BodyInfo bodyInfo)
	{
		myBodyInfo = bodyInfo;
		DressPlayer();
	}

	private void DressPlayer()
	{
		bodySprite.spriteId = myBodyInfo.body;

		if (myBodyInfo.head >= 0)
		{
			headSprite.gameObject.SetActive(true);
			headSprite.spriteId = myBodyInfo.head;
		}
		else
		{
			headSprite.gameObject.SetActive(false);
		}

		if (myBodyInfo.face >= 0)
		{
			faceSprite.gameObject.SetActive(true);
			faceSprite.spriteId = myBodyInfo.head;
		}
		else
		{
			faceSprite.gameObject.SetActive(false);
		}

		if (myBodyInfo.shirt >= 0)
		{
			shirtSprite.gameObject.SetActive(true);
			shirtSprite.spriteId = myBodyInfo.head;
		}
		else
		{
			shirtSprite.gameObject.SetActive(false);
		}

		pantSprite.spriteId = myBodyInfo.pant;

		if (myBodyInfo.feet >= 0)
		{
			feetSprite.gameObject.SetActive(true);
			feetSprite.spriteId = myBodyInfo.feet;
		}
		else
		{
			feetSprite.gameObject.SetActive(false);
		}

		if (myBodyInfo.knife >= 0)
		{
			knifeSprite.gameObject.SetActive(true);
			knifeSprite.spriteId = myBodyInfo.knife;
			RescaleTk2dSpriteCollider(knifeSprite);
		}
		else
		{
			knifeSprite.gameObject.SetActive(false);
		}
	}

	void RescaleTk2dSpriteCollider(tk2dSprite target)
	{
		Vector2 S = target.GetBounds().size;
		BoxCollider2D targetCollider = target.GetComponent<BoxCollider2D>();
		targetCollider.size = S;
		targetCollider.offset = Vector2.zero;// = new Vector2((S.x/2f), (S.y/2f));
	}

	public Vector2 GetInput()
	{
		return moveVector;
	}

	public void PickNormalWeapon(int spriteId)
	{
		myBodyInfo.knife = spriteId;
		DressPlayer();
	}

	public void PickBigWeapon(int spriteId)
	{
		myBodyInfo.knife = spriteId;
		DressPlayer();
	}

	public void PickHealth(int health)
	{
		currentLife += health;
        //RetroJukebox.control.Play("PPLMais", transform.position);
        if (currentLife > myBodyInfo.maxLife)
		{
			currentLife = myBodyInfo.maxLife;
		}
		else if (currentLife <= 0)
		{
			currentLife = 0;
			Die();
		}

		gameController.ChangeHealth(currentLife, myBodyInfo.maxLife);
	}

	public void PickHealthBuff(int healthBuff)
	{
		myBodyInfo.maxLife += healthBuff;
        RetroJukebox.control.PlayOneShot("PPLMais", transform.position);
        PickHealth(myBodyInfo.maxLife);
	}

	public void PickSpeedBuff(int speedBuff)
	{
		myBodyInfo.speed += speedBuff;
        RetroJukebox.control.PlayOneShot("PPVMais", transform.position);
        gameController.ChangeSpeed(myBodyInfo.speed);
	}

	public void PickStrengthBuff(int strengthBuff)
	{
		myBodyInfo.strength += (int)strengthBuff;
        RetroJukebox.control.PlayOneShot("PPFMais", transform.position);
        gameController.ChangeStrength(myBodyInfo.strength);
	}

	public void TakeDamage(int strength)
	{
		Debug.Log(name + " took " + strength + " damage");
		currentLife -= strength;
        RetroJukebox.control.Play("IDano", transform.position);
        RetroJukebox.control.Stop("IAtaque", transform.position);
        if (!NPC)
		{
			gameController.ChangeHealth(currentLife, myBodyInfo.maxLife);
            RetroJukebox.control.Play("PDano", transform.position);
            RetroJukebox.control.Stop("PAtaque", transform.position);
        }
		if (currentLife <= 0)
		{
			Die();
            RetroJukebox.control.Play("IMorte", transform.position);
           // RetroJukebox.control.Play("MortePU", transform.position);
        }
	}

	private void Die()
	{
		dead = true;
		myAnimator.SetTrigger("Dead");
        RetroJukebox.control.Stop("IAlerta", transform.position);
        if (NPC)
		{

			if (imTheBoss)
			{
				gameController.BossDefeated();
                RetroJukebox.control.Play("BMorte", transform.position);
                RetroJukebox.control.Play("MortePLayer", transform.position);
            }
			else
			{
				DropItem();
			}
		}
		else
		{
			gameController.PlayerDefeated();
            RetroJukebox.control.Play("PMorte", transform.position);
            RetroJukebox.control.Play("MortePLayer", transform.position);
        }
	}

	private void DropItem()
	{
		if (UnityEngine.Random.Range(0.0f, 1.0f) <= dropChance)
		{
			if (possibleDrops.Length > 0)
			{
				GameObject go = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Length)];
				Instantiate(go, transform.position, Quaternion.identity);
			}
		}
	}

	void RestartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}
}

[System.Serializable]
public struct BodyInfo
{
	public int body;
	public int head;
	public int face;
	public int shirt;
	public int pant;
	public int feet;
	public int knife;
	public int maxLife;
	public int speed;
	public int strength;

	public BodyInfo(int body, int head, int face, int shirt, int pant, int feet, int knife, int maxLife, int speed, int strength)
	{
		this.body = body;
		this.head = head;
		this.face = face;
		this.shirt = shirt;
		this.pant = pant;
		this.feet = feet;
		this.knife = knife;
		this.maxLife = maxLife;
		this.speed = speed;
		this.strength = strength;
	}

	public BodyInfo(int body, int head, int face, int shirt, int pant, int feet, int knife)
	{
		this.body = body;
		this.head = head;
		this.face = face;
		this.shirt = shirt;
		this.pant = pant;
		this.feet = feet;
		this.knife = knife;
		this.maxLife = 3;
		this.speed = 1;
		this.strength = 1;
	}

	public void SaveBody()
	{
		PlayerPrefs.SetInt("Body", body);
		PlayerPrefs.SetInt("Head", head);
		PlayerPrefs.SetInt("Face", face);
		PlayerPrefs.SetInt("Shirt", shirt);
		PlayerPrefs.SetInt("Pant", pant);
		PlayerPrefs.SetInt("Feet", feet);
		PlayerPrefs.SetInt("Knife", knife);
		PlayerPrefs.SetInt("MaxLife", maxLife);
		PlayerPrefs.SetInt("Speed", speed);
		PlayerPrefs.SetInt("Strength", strength);

	}

	public BodyInfo GetSavedBody()
	{
		body = PlayerPrefs.GetInt("Body", 0);
		head = PlayerPrefs.GetInt("Head", -1);
		face = PlayerPrefs.GetInt("Face", -1);
		shirt = PlayerPrefs.GetInt("Shirt", -1);
		pant = PlayerPrefs.GetInt("Pant", 0);
		feet = PlayerPrefs.GetInt("Feet", -1);
		knife = PlayerPrefs.GetInt("Knife", 0);
		maxLife = PlayerPrefs.GetInt("MaxLife", 3);
		speed = PlayerPrefs.GetInt("Speed", 1);
		strength = PlayerPrefs.GetInt("Strength", 1);
		return new BodyInfo(body, head, face, shirt, pant, feet, knife, maxLife, speed, strength);
	}
}
