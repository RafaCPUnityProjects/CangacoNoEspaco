using UnityEngine;
using UnityEngine.SceneManagement;

public class NimHealth : MonoBehaviour
{
    public float timeToWaitBeforeRestart = 10f;
    public static NimHealth instance;

    public int lives
    {
        get { return GameSave.nimHealth; }
        set { GameSave.nimHealth = value; }
    }
    public int maxLives = 3;
    //public Collider2D healthCollider;
    GameController gameController;

    bool invincible = false;
    public float invincibleTime = 3f;
    public float invincibleDashTime = 1f;

    public SpriteRenderer nimSprite;
    //Color originalColor;

    public Animator nimAnimator;

    bool won = false;
    bool canRestart = false;

    public Collider2D healthCollider;

	private GameCheats gameCheats;

    public GameObject nimHud;

    void Awake()
    {
        if (lives <= 0)
        {
            lives = maxLives;
        }

        if (instance == null)
            instance = this;

		gameCheats = FindObjectOfType<GameCheats>();

        //DontDestroyOnLoad (this);	//<- why?

        //save color
        //if (nimSprite != null)
        //	originalColor = nimSprite.color;

        nimHud = FindObjectOfType<NimHud>().gameObject;
    }

    void Start()
    {
        GameplayEvents.nimOnStage.Publish(this);

        if (gameController == null)
            gameController = FindObjectOfType<GameController>();

        if (gameController != null)
            gameController.SetDeath(false);
        
    }

    void Update()
    {
        if (canRestart && Input.GetButtonDown("Fire1"))
        {
            Restart();
        }

    }

    private bool Won()
    {
        return won;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            if (other.GetComponent<Bullet>().paralize)
            {
                NimMove.instance.Paralize(other.GetComponent<Bullet>().paralizeSeconds);
            }

            Damage(other.GetComponent<Bullet>().damage);

            other.gameObject.GetComponent<Bullet>().Impact();

        }

        else if (other.CompareTag("DropLife"))
		{
			AddHealth();
			Destroy(other.gameObject);
		}

		else if (other.CompareTag("Coin"))
        {
            GameSave.instance.CoinCollect(other.transform.parent.gameObject.GetComponent<DropCoin>().coinsDropped);
            //JukeboxPlayer.control.Play("Effects", "PickupBit", true);
            if (RetroJukebox.control != null)
                RetroJukebox.control.PlayOneShot("PickBit", transform.position);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Explosion"))
        {
            Damage(1);
        }

        else if (other.gameObject.CompareTag("Acid"))
        {
            Damage(1);
        }
    }

	public void AddHealth()
	{
		if (lives < maxLives)
			lives++;
        //JukeboxPlayer.control.Play("Effects", "PickupLife", true);
        if (RetroJukebox.control != null)
            RetroJukebox.control.PlayOneShot("PickLife", transform.position);
    }

	void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            Damage(1);
        }
    }

    public void Damage(int damagePower)
    {
#if UNITY_EDITOR
		if (gameCheats.imortal)
		{
			return;
		}
#endif
		if (NimMove.instance.isDashing())
            return;

        if (invincible)
            return;

        if (lives > 0)
        {
            //JukeboxPlayer.control.Play("Effects", "NimHurt", true);
            if (RetroJukebox.control != null)
                RetroJukebox.control.PlayOneShot("NimHurt", NimMove.instance.transform.position);
            lives -= damagePower;
        }

        if (lives <= 0)
        {
            //JukeboxPlayer.control.Play ("Effects", "GameOver", true);
            PlayerDied();
        }
        else if (NimMove.instance.paralized)
        {
            //no nim animator
        }
        else {
            if (nimAnimator != null)
                nimAnimator.SetTrigger("damaged");
            PlayerFlicker();
        }

    }

    void PlayerDied()
    {
        Invoke("CanRestart", 1f);

        healthCollider.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        invincible = true;

        Invoke("DestroyMe", 1f);

        GameSave.instance.NewTry();

        GameplayEvents.nimDied.Publish(this);

        if (gameController != null)
        {
            gameController.SetDeath(true);
        }

    }

    void DestroyMe()
    {
        nimSprite.gameObject.SetActive(false);
        GameObject.Find("NimHud").SetActive(false);
        transform.FindChild("Shadow").gameObject.SetActive(false);
        //Destroy(this.gameObject);     //dont destroy gameobject, components needed to restart game
    }

    public void PlayerWon()
    {
        Invoke("CanRestart", timeToWaitBeforeRestart);

        won = true;
        GameplayEvents.nimDied.Publish(this);

        if (gameController != null)
        {
            gameController.WinGame();
        }
    }

    public bool Died()
    {
        return (lives <= 0);
    }

    void CanRestart()
    {
        canRestart = true;
    }

    void PlayerFlicker()
    {
        CancelInvoke("OffInvincible");
        invincible = true;
        Invoke("OffInvincible", invincibleTime);
    }

    public void InvincibleDash()
    {
        CancelInvoke("InvincibleDash");
        invincible = true;
        Invoke("OffInvincible", invincibleDashTime);
    }

    void OffInvincible()
    {
        invincible = false;
    }

    void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
