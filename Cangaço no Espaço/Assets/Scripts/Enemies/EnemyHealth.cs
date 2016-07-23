using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{

	public int lives = 3;
	int startLife;

	public GameObject[] dropOnDie;
	[Range(0.1f, 1f)]
	public float chanceToDrop = 1f;

	public GameObject dropLife;

	public Animator enemyAnimator;

	FollowTarget ft;
	public float pushForce = 0.02f;

	public bool died = false;

	public bool invincible = false;

	public enum eMonsterType { rock, avoider, killer }
	//public eMonsterType monsterType;

	//only for using with boss
	BossAI bossAI;

	public Collider2D weakPoint;
	Collider2D bodyCollider;

	WeaponController weapon;

	public GameObject explosion;
	public bool explodeOnTouch;
	public float timeToExplode = 2f;

	public GameObject glitchPrefab;
	GameController gameController;

	public float dieDelay = 0f;

	//public string deathAudio = "EnemyDeath";

	Transform shadow;

	public GameObject dropCoin;
	public GameObject dropCoin10;

    public int minCoins = 3;
	public int maxCoins = 5;

	public float blinkTime = 0.1f;

	public bool destroyAfterExplode;

	private BossController bossController;
	private bool isBoss = false;

	Blink blink;

	void Awake()
	{
		ft = GetComponent<FollowTarget>();
		bossAI = GetComponent<BossAI>();

		startLife = lives;

		GameplayEvents.nimOnStage.Subscribe(OnNimOnStage);

		shadow = transform.FindChild("Shadow");

		blink = transform.FindChild ("Sprite").GetComponent<Blink> ();
	}

	public void SetupBoss(int bossLife, BossController bossController)
	{
		lives = bossLife;
		this.bossController = bossController;
		this.transform.parent = bossController.transform;
		isBoss = true;
	}

	void Start()
	{
		bodyCollider = GetComponent<Collider2D>();
		gameController = FindObjectOfType<GameController>();
	}

	private void OnNimOnStage(NimHealth nimHealth)
	{
		weapon = nimHealth.GetComponent<NimAttack>().weaponController;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (died)
			return;

		//avoid damage on body collision if we have weak point
		if (weakPoint != null)
			return;

		if (invincible)
		{
			if (other.CompareTag("PlayerMelee") || other.CompareTag("Bullet"))
			{
				if (enemyAnimator != null)
					enemyAnimator.SetTrigger("blink");
			}
			return;
		}

		if (other.CompareTag("PlayerMelee"))
		{
			Debug.Log("Damaged by Melee");
			Damaged(NimAttack.instance.meleeDamage);
			invincible = true;
			Invoke("DisableInvincible", blinkTime);
		}

		else if (other.CompareTag("Bullet"))
		{
			other.gameObject.GetComponentInParent<Bullet>().Impact();
			Damaged(other.GetComponentInParent<Bullet>().damage);
		}

		else if (other.gameObject.CompareTag("ExplosionPush"))
		{
			//trigger explosion
			NimClose();
		}

		else if (other.gameObject.CompareTag("Explosion") && destroyAfterExplode)
		{
			if (other.GetComponent<TriggerDamage>() != null)
				Damaged(other.GetComponent<TriggerDamage>().damage);

		}
	}

	void DisableInvincible()
	{
		invincible = false;
	}

	public void NimClose()
	{
		if (enemyAnimator != null)
			enemyAnimator.SetTrigger("close");

		Invoke("Explode", timeToExplode);
	}

	void Explode()
	{
		if (died)
			return;

		if (explosion != null)
			explosion.Spawn(transform.position, transform.rotation);

		if (destroyAfterExplode)
		{
			died = true;
			GameplayEvents.enemyDied.Publish(this);
			Destroy(this.gameObject);
		}
		else
		{
			//Special pineapple
			CancelInvoke();

			invincible = true;
			Invoke("DisableInvincible", 1f);

			if (enemyAnimator != null)
				enemyAnimator.SetTrigger("triggerDamaged");
		}

	}

	/*void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Explosion"))
		{
			Debug.Log ("Damaged by explosion");
			Damaged(2);
		}
	}*/

	public void Damaged(int damage)
	{
		if (died)
			return;

		//Trigger explosion if receive damage
		if (explodeOnTouch)
			NimClose();

		//push
		if (ft != null)
			transform.Translate(ft.GetDirection() * -pushForce);

		lives -= damage;

		if (bossAI != null)
			bossAI.TriggerHealthChanged((float)lives / (float)startLife);

		if (lives <= 0)
		{
			died = true;

			if (bodyCollider != null)
				bodyCollider.enabled = false;
			if (weakPoint != null)
				weakPoint.enabled = false;

            //if (deathAudio.Length != 0)
            //JukeboxPlayer.control.Play("Effects", deathAudio, false, 0f, this.gameObject);
            if (RetroJukebox.control != null)
                RetroJukebox.control.PlayOneShot("EnemyDeath", NimMove.instance.transform.position);

            if (CompareTag("Boss"))
			{

			}

			DropGlitch();

			if (shadow != null)
				shadow.gameObject.SetActive(false);

			if (explodeOnTouch)
				CancelInvoke("Explode");

			if (enemyAnimator != null)
				enemyAnimator.SetTrigger("dead");

			DropCoins();
			DropItem();
			Invoke("Killme", dieDelay);
		}
		else {
			if (enemyAnimator != null)
				enemyAnimator.SetTrigger("triggerDamaged");

			if (blink != null)
				blink.enabled = true;
		}
	}

	void Killme()
	{
		GameplayEvents.enemyDied.Publish(this);
		if (isBoss)
		{
			bossController.BossDied();
			return;
		}
		Destroy(this.gameObject);
	}

	void DropItem()
	{
		if (dropOnDie.Length > 0)
		{
			if (Random.value <= chanceToDrop + 0.01f)
			{

				if (weapon != null && GameSave.nimWeapon.Equals(CurrentWeapon.WatermelonGun)
					&& dropOnDie[Random.Range(0, dropOnDie.Length)].name == "Drop Watermelon")
				{
					if (dropLife != null)
						Instantiate(dropLife, transform.position, Quaternion.identity);
				}
				else if (weapon != null && GameSave.nimWeapon.Equals(CurrentWeapon.BananaGun)
					&& dropOnDie[Random.Range(0, dropOnDie.Length)].name == "Drop Banana")
				{
					if (dropLife != null)
						Instantiate(dropLife, transform.position, Quaternion.identity);
				}
				else if (weapon != null && GameSave.nimWeapon.Equals(CurrentWeapon.PineappleGun)
					&& dropOnDie[Random.Range(0, dropOnDie.Length)].name == "Drop Pineapple")
				{
					if (dropLife != null)
						Instantiate(dropLife, transform.position, Quaternion.identity);
				}
				else if (weapon.NoGun() && dropOnDie[Random.Range(0, dropOnDie.Length)].name == "Drop Melee")
				{
					if (dropLife != null)
						Instantiate(dropLife, transform.position, Quaternion.identity);
				}
				else
				{
					if (dropOnDie != null)
					{
						GameObject dropItem = dropOnDie[Random.Range(0, dropOnDie.Length)];
						Instantiate(dropItem, transform.position, Quaternion.identity);
					}

				}
			}
		}
	}

	void DropCoins()
	{

		int coinsToDrop = Random.Range(minCoins, maxCoins);

		if (dropCoin != null && dropCoin10 != null)
		{
			for (int i = 0; i < coinsToDrop; i++)
			{
				Instantiate(dropCoin, transform.position, Quaternion.identity);
			}
		}
	}

	private void DropGlitch()
	{

		if (glitchPrefab != null)
		{
			GameObject newGlitch = (GameObject)Instantiate(glitchPrefab, transform.position, Quaternion.identity);
			newGlitch.transform.parent = gameController.canvasGlitch;
		}

	}
}
