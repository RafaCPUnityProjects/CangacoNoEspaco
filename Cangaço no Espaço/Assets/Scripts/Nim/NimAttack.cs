using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NimAttack : MonoBehaviour
{
	public static NimAttack instance;

	NimMove nimMove;
	NimHealth nimHealth;

	public Animator nimAnimator;

	//TODO: put on melee script
	//bool meleeAttack = false;
	public Collider2D meleeCollider;
	public Collider2D meleeColliderUp;
	public Collider2D meleeColliderDown;
	public int meleeDamage = 2;

	public WeaponController weaponController;
	public Transform weaponsTransform;

	ScreenShake shake;

	public GameObject meleeFx;
	public Transform meleeTransform;
	public GameObject meleeFxUp;
	public Transform meleeTransformUp;
	public GameObject meleeFxDown;
	public Transform meleeTransformDown;

	public EnergyHud energyHud;

	public float timeMeleeActive = 0.2f;

    public GameObject dropMelee;
    public GameObject dropWatermelon;
    public GameObject dropPineapple;
    public GameObject dropBanana;


    void Awake()
	{
		if (instance == null)
			instance = this;

		DisableMeleeColliders();
		nimMove = GetComponent<NimMove>();
		nimHealth = GetComponent<NimHealth>();
	}

	void Start()
	{
		shake = Camera.main.GetComponent<ScreenShake>();
        switch (GameSave.nimWeapon)
        {
            case CurrentWeapon.None:
				energyHud = NimHud.instance.hudMelee.GetComponent<EnergyHud>();
				weaponController.PickMelee();
                break;
            case CurrentWeapon.PineappleGun:
				energyHud = NimHud.instance.hudPineapple.GetComponent<EnergyHud>();
				weaponController.PickPineapple();
                break;
            case CurrentWeapon.WatermelonGun:
				energyHud = NimHud.instance.hudWatermelon.GetComponent<EnergyHud>();
				weaponController.PickFireGun();
                break;
            case CurrentWeapon.BananaGun:
				energyHud = NimHud.instance.hudBanana.GetComponent<EnergyHud>();
				weaponController.PickBanana();
                break;
            default:
				energyHud = NimHud.instance.hudMelee.GetComponent<EnergyHud>();
				weaponController.PickMelee();
                break;
        }
	}

	void Update()
	{
		if (GameController.instance.IsGamePaused())
			return;

		if (NimMove.instance.paralized) {
			return;
		}
		
		//Melee hud visible?
		if(energyHud != null)
			energyHud.SpriteVisible (!weaponController.GunCharged() || weaponController.Empty());

		if (nimHealth.Died())
			return;

		//Controller Slot1
		if (!weaponController.GunCharged() || weaponController.Empty())
		{
			//Melee attack
			if (Input.GetButtonDown("Fire1") && energyHud.CanShoot())
				AttackMelee();
		}
		else
		{
			//Gun attack
			weaponController.shooting = Input.GetButton("Fire1");
            weaponController.triggerButtonShootUp = Input.GetButtonUp("Fire1");
			weaponController.triggerButtonShootDown = Input.GetButtonDown("Fire1");

        }


		//Controller Slot2
		if (Input.GetButton("Fire2"))
		{

		}

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("DropBanana") && other.GetComponent<DropMove>().canBePicked)
		{
			Destroy(other.gameObject);

			ChangeDrop(other);
			weaponController.PickBanana ();

			//JukeboxPlayer.control.Play("Effects", "PickupWeapon", true);

		}
		if (other.CompareTag("DropPineapple") && other.GetComponent<DropMove>().canBePicked)
		{
			Destroy(other.gameObject);

			ChangeDrop(other);
			weaponController.PickPineapple();

            //JukeboxPlayer.control.Play("Effects", "GrenadePineapple", true);
            if (RetroJukebox.control != null)
                RetroJukebox.control.PlayOneShot("PickPineapple", NimMove.instance.transform.position);

        }
		if (other.CompareTag("DropFireGun") && other.GetComponent<DropMove>().canBePicked)
		{
			Destroy(other.gameObject);

            ChangeDrop(other);
            weaponController.PickFireGun();

            //JukeboxPlayer.control.Play("Effects", "WatermelonShot", true);
            if (RetroJukebox.control != null)
                RetroJukebox.control.PlayOneShot("PickWartermeloon", NimMove.instance.transform.position);

        }
        if (other.CompareTag("DropMelee") && other.GetComponent<DropMove>().canBePicked)
		{
			Destroy(other.gameObject);

            ChangeDrop(other);
            weaponController.PickMelee();

            //JukeboxPlayer.control.Play("Effects", "ApplePunch", true);
            if (RetroJukebox.control != null)
                RetroJukebox.control.PlayOneShot("PickApple", NimMove.instance.transform.position);
        }

    }

    void ChangeDrop(Collider2D coll)
    {
        Vector3 dropPos;
        dropPos = coll.transform.position;

        GameObject newDrop = (GameObject)Instantiate(GetWeaponDrop(), dropPos, Quaternion.identity);
        newDrop.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.value, Random.value, Random.value) / 8, ForceMode2D.Impulse);

        //opposite dir
        //Vector3 impulseDir = -transform.position - other.transform.position;
        //newDrop.GetComponent<Rigidbody2D>().AddForce(impulseDir, ForceMode2D.Impulse);
    }

    GameObject GetWeaponDrop()
    {
        GameObject drop = dropMelee;

        if (!weaponController.GunCharged() || weaponController.Empty())
            drop = dropMelee;
        else if (GameSave.nimWeapon == CurrentWeapon.WatermelonGun)
            drop = dropWatermelon;
        else if (GameSave.nimWeapon == CurrentWeapon.PineappleGun)
            drop = dropPineapple;
        else if (GameSave.nimWeapon == CurrentWeapon.BananaGun)
            drop = dropBanana;

        return drop;
    }

	void AttackMelee()
	{
        //JukeboxPlayer.control.Play("Effects", "NimMelee", true);
        if(RetroJukebox.control != null)
            RetroJukebox.control.PlayOneShot("NimMelee", NimMove.instance.transform.position);

        TriggerMeleeAnimation ();

		ActiveMeleeCollider();
		shake.TriggerShake();
	}

	void TriggerMeleeAnimation(){
		
		if (nimMove.GetLookDirection().Equals(Vector2.up))
			nimAnimator.SetTrigger("triggerMeleeUp");
		else if (nimMove.GetLookDirection().Equals(Vector2.down))
			nimAnimator.SetTrigger("triggerMeleeDown");
		else
			nimAnimator.SetTrigger("triggerMelee");
	}

	void ActiveMeleeCollider()
	{
		CancelInvoke("DisableMeleeColliders");
		Invoke ("DisableMeleeColliders", timeMeleeActive);

		if (nimMove.GetLookDirection().Equals(Vector2.up))
		{
			meleeColliderUp.gameObject.SetActive(true);
			ObjectPool.Instantiate(meleeFxUp, meleeTransformUp.position, meleeTransformUp.rotation);
		}
		else if (nimMove.GetLookDirection().Equals(Vector2.down))
		{
			meleeColliderDown.gameObject.SetActive(true);
			ObjectPool.Instantiate(meleeFxDown, meleeTransformDown.position, meleeTransformDown.rotation);
		}
		else {

			meleeCollider.gameObject.SetActive(true);
			GameObject newMelee = (GameObject)ObjectPool.Instantiate(meleeFx, meleeTransform.position, meleeTransform.rotation);
			newMelee.transform.localScale = meleeTransform.lossyScale;
		}
	}

	void ShowMeleeFx(GameObject fx, Transform tr)
	{
		fx.SetActive(true);
		fx.transform.position = tr.position;
		fx.transform.rotation = tr.rotation;
		fx.transform.localScale = tr.lossyScale;
	}

	void DisableMeleeColliders()
	{
		meleeCollider.gameObject.SetActive(false);
		meleeColliderUp.gameObject.SetActive(false);
		meleeColliderDown.gameObject.SetActive(false);
	}


}
