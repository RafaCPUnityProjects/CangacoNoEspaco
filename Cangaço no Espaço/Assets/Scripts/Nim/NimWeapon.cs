using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NimWeapon : MonoBehaviour {

	public GameObject bullet;

	//Weapon stats
	public float force = 40f;
	public float cadence = 1f;
	public int bulletsPerShot = 1;
	[Range(0f, 1f)]
	public float precision = 1f;
	float invPrecision;

	public NimMove nimMove;
	public Animator nimAnimator;

	Vector2 bulletDirection;

	public enum target {Player, Left, Right, Up, Down};
	public target[] shootTarget; 

	//ScreenShake shake;

	public bool diagonal;
	public bool triggerByAnimation;

	WeaponController weaponController;

    [HideInInspector]
	public EnergyHud ammoSlider;

	public Transform gunpointHorizontal;
	public Transform gunpointUp;
	public Transform gunpointDown;
	private Transform gunpoint;

	public GameObject shootFx;
	public GameObject shootFxUp;
	public GameObject shootFxDown;

    float timePressedButton = 0f;


	public float grenadeMaxSeconds = 3f;
	public float grenadeMinSeconds = 0.5f;

	public EnergyHud hudWatermelon;
	public EnergyHud hudPineapple;
	public EnergyHud hudBanana;

	void Start(){

		weaponController = transform.parent.GetComponent<WeaponController> ();
		//shake = FindObjectOfType<ScreenShake> ();

		//each weapon has his own energyController and energyHud
		if (this.gameObject.name == "WatermelonGun")
			ammoSlider = hudWatermelon;
		else if(this.gameObject.name == "PineappleGun")
			ammoSlider = hudPineapple;
		else if(this.gameObject.name == "BananaGun")
			ammoSlider = hudBanana;

		invPrecision = 1 - precision;

		DisableFx ();
	}

	void Update(){

		ammoSlider.SpriteVisible (!weaponController.Empty ());

		if (Input.GetButtonUp("Fire1")) {

			if (this.gameObject.name == "WatermelonGun") {
				nimAnimator.SetTrigger ("StopWatermelonAttack");
				nimAnimator.SetTrigger ("StopWatermelonAttackUp");
				nimAnimator.SetTrigger ("StopWatermelonAttackDown");
			}
		}

	}

	public void Shoot(float timePressed = 0f)
	{
        timePressedButton = timePressed;
        
        if (ammoSlider.CanShoot ()) {

			if (!triggerByAnimation)
				ShootBullet ();
		}
	}
    

    public void PrepareGrenade()
	{
		if (nimAnimator != null)
		{
			if(nimMove != null){
				bulletDirection = nimMove.GetLookDirection ();
			}

			if (bulletDirection.normalized.Equals (Vector2.down)){
				nimAnimator.SetTrigger ("granadaAttackDown");
			}
			else if(bulletDirection.normalized.Equals(Vector2.up)){
				nimAnimator.SetTrigger("granadaAttackUp");
			}
			else{
				nimAnimator.SetTrigger("granadaAttack");
			}
		}
	}

	public void ShootGrenade(){
		if (ammoSlider.Max ())
			nimAnimator.SetTrigger("granadaAttackShoot");
       

    }

	public void ShootWatermelon()
	{
       
        if (ammoSlider.CanShoot ()) {

			if (!triggerByAnimation)
				ShootBullet ();
			
			ShootAnimationWatermelon ();
		}
	}
		
	private void ShootAnimationWatermelon()
	{
		if (nimAnimator != null)
		{
			if(nimMove != null){
				bulletDirection = nimMove.GetLookDirection ();
			}

			if (bulletDirection.normalized.Equals (Vector2.down)){
				if (shootFxDown != null) {
					shootFxDown.SetActive (true);
					Invoke ("DisableFx", 0.2f);
				}
				nimAnimator.SetTrigger ("watermelonAttackDown");
			}
			else if(bulletDirection.normalized.Equals(Vector2.up)){
				if (shootFxUp != null) {
					shootFxUp.SetActive (true);
					Invoke ("DisableFx", 0.2f);
				}
				nimAnimator.SetTrigger("watermelonAttackUp");
			}
			else{
				if (shootFx != null) {
					shootFx.SetActive (true);
					Invoke ("DisableFx", 0.2f);
				}
				nimAnimator.SetTrigger("watermelonAttack");
			}
		}
	}

	void DisableFx(){
		if(shootFx != null)
			shootFx.SetActive (false);
		if(shootFxUp != null)
			shootFxUp.SetActive (false);
		if(shootFxDown != null)
			shootFxDown.SetActive (false);
	}

	public void ShootBullet(){

        //JukeboxPlayer.control.Play ("Effects", "NimShoot", true);
        
            if (GameSave.nimWeapon == CurrentWeapon.WatermelonGun &&  RetroJukebox.control != null)
                RetroJukebox.control.PlayOneShot("NimShoot", NimMove.instance.transform.position);
            if (GameSave.nimWeapon == CurrentWeapon.PineappleGun && RetroJukebox.control != null)
                RetroJukebox.control.PlayOneShot("NimGranade", NimMove.instance.transform.position);


        for (int i = 0; i < bulletsPerShot; i++){

			if(nimMove != null){
				bulletDirection = nimMove.GetLookDirection ();

				SetGunpoint (bulletDirection);
			}


			Vector2 randomizedDirection = new Vector2(bulletDirection.x + Random.Range(0f, invPrecision), 
														bulletDirection.y + Random.Range(0f, invPrecision));


			//New bullet for each direction
			if (!diagonal) {
				foreach (target t in shootTarget) {
					if (t == target.Left) {
						NewBullet (Vector2.left);
					} else if (t == target.Right) {
						NewBullet (Vector2.right);
					} else if (t == target.Up) {
						NewBullet (Vector2.up);
					} else if (t == target.Down) {
						NewBullet (Vector2.down);
					} else {
						NewBullet (randomizedDirection);
					}
				}
			} else {
				//diagonal bullets
				foreach (target t in shootTarget) {
					if (t == target.Left) {
						NewBullet (Vector2.left + Vector2.down);
					} else if (t == target.Right) {
						NewBullet (Vector2.right + Vector2.up);
					} else if (t == target.Up) {
						NewBullet (Vector2.up + Vector2.left);
					} else if (t == target.Down) {
						NewBullet (Vector2.down + Vector2.right);
					} else {
						NewBullet (randomizedDirection);
					}
				}
			}
			diagonal = !diagonal;

		}
	}

	void NewBullet(Vector2 direction){
		GameObject poolBullet = bullet.Spawn (gunpoint.position, Quaternion.identity);

		Rigidbody2D rb = poolBullet.GetComponent<Rigidbody2D> ();

		if(rb != null)
        {
            if(timePressedButton > 0f)
            {
                //pineapple shoot style
                rb.AddForce(direction * force * Mathf.Clamp(timePressedButton, grenadeMinSeconds, grenadeMaxSeconds));
            }
            else
            {
                rb.AddForce(direction * force);
            }
            
        }
			
	}

	void SetGunpoint(Vector2 direction){
		
		if (direction.normalized.Equals(Vector2.down))
			gunpoint = gunpointDown;
		else if(direction.normalized.Equals(Vector2.up))
			gunpoint = gunpointUp;
		else
			gunpoint = gunpointHorizontal;

		if (gunpoint == null)
			gunpoint = transform;
	}

}
