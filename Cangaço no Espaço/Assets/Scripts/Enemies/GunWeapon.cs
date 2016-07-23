using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
//using System;

public class GunWeapon : MonoBehaviour
{

	public GameObject bullet;

	//Weapon stats
	public float force = 40f;
	public float cadence = 1f;
	public int bulletsPerShot = 1;
	[Range(0f, 1f)]
	public float precision = 0f;    //0: max precision

	bool canShoot = true;
	bool canShootAgain = true;

	Transform playerTransform;

	Vector2 bulletDirection;

	public enum target { Player, Left, Right, Up, Down, UpLeft, UpRight, DownLeft, DownRight };
	public target[] shootTarget;

	//ScreenShake shake;

	public bool diagonal;
	public bool triggerByAnimation;

	public bool allDirections;
	public int directionStep;

	public WeaponController weaponController;

	Animator animator;

	public float rechargeTime = 0f;
	bool recharging = false;
	public int ammoShoots = 10;
	int shoots = 0;

	void Awake()
	{
		GameplayEvents.nimOnStage.Subscribe(OnNimOnStage);
	}

	private void OnNimOnStage(NimHealth nimHealth)
	{
		playerTransform = nimHealth.transform;
	}

	void OnEnable()
	{
		//shake = FindObjectOfType<ScreenShake> ();
		animator = transform.parent.FindChild("Sprite").GetComponent<Animator>();

		CancelInvoke("ShootEnable");
		InvokeRepeating("ShootEnable", 0f, cadence);
	}

	void Update()
	{
		Shoot();
	}

	public void Shoot()
	{
		if (canShoot && canShootAgain)
		{

			canShoot = false;

			if (!triggerByAnimation)
				ShootBullet();

			ShootAnimation();
		}
	}

	private void ShootAnimation()
	{
		if (diagonal)
			animator.SetTrigger("attackDiagonal");
		else
			animator.SetTrigger("attack");
	}

	//TODO: Create bullet class?
	public void ShootBullet()
	{
		if (allDirections)
		{
			AllDirectionsShot();
			return;
		}
		for (int i = 0; i < bulletsPerShot; i++)
		{

			if (playerTransform != null)
			{
				Vector3 playerDirection = (playerTransform.position - transform.position).normalized;
				bulletDirection = new Vector2(playerDirection.x, playerDirection.y);
			}

			Vector2 randomizedDirection = new Vector2(bulletDirection.x + Random.Range(0f, precision),
				bulletDirection.y + Random.Range(0f, precision));


			//New bullet for each direction
			if (!diagonal)
			{
				foreach (target t in shootTarget)
				{
					if (t == target.Left)
					{
						NewBullet(Vector2.left);
					}
					else if (t == target.Right)
					{
						NewBullet(Vector2.right);
					}
					else if (t == target.Up)
					{
						NewBullet(Vector2.up);
					}
					else if (t == target.Down)
					{
						NewBullet(Vector2.down);

					}
					else if (t == target.DownLeft)
					{
						NewBullet(Vector2.down + Vector2.left);
					}
					else if (t == target.DownRight)
					{
						NewBullet(Vector2.down + Vector2.right);
					}
					else if (t == target.UpLeft)
					{
						NewBullet(Vector2.up + Vector2.left);
					}
					else if (t == target.UpRight)
					{
						NewBullet(Vector2.up + Vector2.right);

					}
					else
					{
						//Shoot in player direction
						if (shoots < ammoShoots)
						{
							NewBullet(randomizedDirection);
							shoots++;
						}
						else
						{

							if (!recharging)
							{
								recharging = true;
								Invoke("ResetAmmo", rechargeTime);
							}
						}
					}
				}
			}
			else
			{
				//diagonal bullets
				foreach (target t in shootTarget)
				{
					if (t == target.Left)
					{
						NewBullet(Vector2.left + Vector2.down);
					}
					else if (t == target.Right)
					{
						NewBullet(Vector2.right + Vector2.up);
					}
					else if (t == target.Up)
					{
						NewBullet(Vector2.up + Vector2.left);
					}
					else if (t == target.Down)
					{
						NewBullet(Vector2.down + Vector2.right);
					}
					else
					{
						//Shoot in player direction
						if (shoots < ammoShoots)
						{
							NewBullet(randomizedDirection);
							shoots++;
						}
						else
						{

							if (!recharging)
							{
								recharging = true;
								Invoke("ResetAmmo", rechargeTime);
							}
						}
					}
				}
			}
			diagonal = !diagonal;

		}
	}

	private void AllDirectionsShot()
	{
		if (shoots < ammoShoots)
		{
			shoots++;
			List<Vector2> directions = new List<Vector2>();
			for (int angle = 0; angle <= 360; angle += directionStep)
			{
				Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), -Mathf.Sin(Mathf.Deg2Rad * angle));
				directions.Add(direction);
			}
			NewBullet(directions);
		}
		else
		{
			if (!recharging)
			{
				recharging = true;
				Invoke("ResetAmmo", rechargeTime);
			}
		}
	}

	private void NewBullet(List<Vector2> directions)
	{
		List<GameObject> bullets = new List<GameObject>();
		for (int i = 0; i < directions.Count; i++)
		{
			bullets.Add(bullet.Spawn(transform.position, Quaternion.identity));
		}
		for (int i = 0; i < directions.Count; i++)
		{
			bullets[i].GetComponent<Rigidbody2D>().AddForce(directions[i] * force);
		}
	}

	void ResetAmmo()
	{
		recharging = false;
		shoots = 0;
	}

	void NewBullet(Vector2 direction)
	{
		GameObject poolBullet = bullet.Spawn(transform.position, Quaternion.identity);
		poolBullet.GetComponent<Rigidbody2D>().AddForce(direction * force);
	}

	void ShootEnable()
	{
		canShoot = true;
	}

}
