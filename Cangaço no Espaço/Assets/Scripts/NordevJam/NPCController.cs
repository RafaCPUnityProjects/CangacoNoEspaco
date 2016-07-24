using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour
{
	public tk2dSprite bodySprite;
	public tk2dSprite headSprite;
	public tk2dSprite faceSprite;
	public tk2dSprite shirtSprite;
	public tk2dSprite pantSprite;
	public tk2dSprite feetSprite;
	public tk2dSprite knifeSprite;

	public tk2dSpriteCollection bodyCollection;
	public tk2dSpriteCollection headCollection;
	public tk2dSpriteCollection faceCollection;
	public tk2dSpriteCollection shirtCollection;
	public tk2dSpriteCollection pantCollection;
	public tk2dSpriteCollection knifeCollection;

	public int bodyNumber;
	public int headNumber;
	public int faceNumber;
	public int shirtNumber;
	public int pantNumber;
	public int knifeNumber;


	private Animator myAnimator;

	void Start()
	{
		myAnimator = GetComponent<Animator>();
	}

	void Update()
	{
		bool walking = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
		myAnimator.SetBool("Walking", walking);

		if (Input.GetButtonDown("Fire1"))
		{
			myAnimator.SetTrigger("Attack");
		}

		if (Input.GetButtonDown("Fire2"))
		{
			RandomClothing();
		}
	}

	void RandomClothing()
	{
		int body = Random.Range(0, bodyNumber);
		int head = Random.Range(0, headNumber);
		int face = Random.Range(0, faceNumber);
		int shirt = Random.Range(0, shirtNumber);
		int pant = Random.Range(0, pantNumber);
		int knife = Random.Range(0, bodyNumber);

		bodySprite.spriteId = body;
		headSprite.spriteId = head;
		Debug.Log("CAbeca id: " + head);
		faceSprite.spriteId = face;
		shirtSprite.spriteId = shirt;
		pantSprite.spriteId = pant;
		knifeSprite.spriteId = knife;
	}
}
