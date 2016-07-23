using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float timedDestroy = 1f;
	float tDestroy;

    //When Pineapple is active to explode, can be destroyed by player

    public int lives = 3;
    int startLife;
    public bool died = false;

    public GameObject dropLife;
    public GameObject glitchPrefab;
    GameController gameController;

    public GameObject[] dropOnDie;
    [Range(0.1f, 1f)]
    public float chanceToDrop = 1f;

    WeaponController weapon;

	public string deathAudio = "EnemyDeath";
	//public string explodeAudio = "PineappleExplosion";

	public bool childParent;

    public float soundDelay = 1.5f;

    void OnEnable () {
		tDestroy = timedDestroy;	
	}

    void Awake()
    {
        GameplayEvents.nimOnStage.Subscribe(OnNimOnStage);
    }

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        
        if (RetroJukebox.control != null)
            RetroJukebox.control.PlayExplosion(NimMove.instance.transform.position, soundDelay);
    }

    void Update(){
		
		tDestroy -= Time.deltaTime;

		if (tDestroy <= 0f) {

            //Recycle this pooled explosion instance
            //gameObject.Recycle();

			if(childParent)
				transform.parent.parent.gameObject.Recycle();
			else
				transform.parent.gameObject.Recycle();
		}
	}

    private void OnNimOnStage(NimHealth nimHealth)
    {
        weapon = nimHealth.GetComponent<NimAttack>().weaponController;
    }

    public void Damaged(int damage)
    {
        if (died)
            return;

        lives -= damage;

        if (lives <= 0)
        {
            died = true;

            DropItem();
            DropGlitch();

            //if(deathAudio.Length != 0)
            //	JukeboxPlayer.control.Play ("Effects", deathAudio, false, 0f, this.gameObject);
            if (RetroJukebox.control != null)
                RetroJukebox.control.PlayOneShot("EnemyDeath", NimMove.instance.transform.position);

            Destroy(transform.parent.gameObject);
        }
    }

    void DropItem()
    {
        if (dropOnDie.Length > 0)
        {
            if (Random.value <= chanceToDrop + 0.01f)
            {

                if (weapon != null && GameSave.nimWeapon.Equals(CurrentWeapon.WatermelonGun)
                    && dropOnDie[Random.Range(0, dropOnDie.Length)].name == "Drop FireGun")
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
				else if (weapon != null && GameSave.nimWeapon.Equals(CurrentWeapon.BananaGun)
					&& dropOnDie[Random.Range(0, dropOnDie.Length)].name == "Drop Banana")
				{
					if (dropLife != null)
						Instantiate(dropLife, transform.position, Quaternion.identity);
				}
                else if (weapon != null && weapon.NoGun() && dropOnDie[Random.Range(0, dropOnDie.Length)].name == "Drop Melee")
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

    private void DropGlitch()
    {

        if (glitchPrefab != null)
        {
            GameObject newGlitch = (GameObject)Instantiate(glitchPrefab, transform.position, Quaternion.identity);
            newGlitch.transform.parent = gameController.canvasGlitch;
        }

    }
}
