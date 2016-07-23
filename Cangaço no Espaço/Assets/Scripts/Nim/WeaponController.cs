using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum CurrentWeapon
{
	None, PineappleGun, WatermelonGun, BananaGun
}

public class WeaponController : MonoBehaviour {

	//public CurrentWeapon currentGun
 //   {
 //       get {return GameSave.nimWeapon; }
 //       set {GameSave.nimWeapon = value; }
 //   }

	public NimWeapon pineappleGun;
	public NimWeapon watermelonGun;
	public NimWeapon bananaGun;

	//[HideInInspector]
	//public NimWeapon myGunSlot1
 //   {
 //       set { GameSave.nimGunWeapon = value; }
 //       get { return GameSave.nimGunWeapon; }
 //   } //activegun

	public NimAttack nimAttack;

	public bool shooting;
    public bool triggerButtonShootUp;
	public bool triggerButtonShootDown;
    float timerButtonDown = 0f;

	ForceHud forceHudGrenade;
	EnergyHud energyHudGrenade;
	bool prepareGrenade = false;

    //void Start()
    //{
    //    switch (GameSave.nimWeapon)
    //    {
    //        case CurrentWeapon.None:
    //            PickMelee();
    //            break;
    //        case CurrentWeapon.PineappleGun:
    //            PickPineapple();
    //            break;
    //        case CurrentWeapon.WatermelonGun:
    //            PickFireGun();
    //            break;
    //        case CurrentWeapon.BananaGun:
    //            PickBanana();
    //            break;
    //        default:
    //            break;
    //    }
    //}

	void Update(){

		if (shooting) {
			if(GameSave.nimWeapon == CurrentWeapon.WatermelonGun)
				watermelonGun.ShootWatermelon ();
			else if(GameSave.nimWeapon == CurrentWeapon.BananaGun)
				bananaGun.Shoot ();
			
            //save time pressing button for pineapple shoot
            timerButtonDown += Time.deltaTime;
        }

		if (triggerButtonShootDown)
		{
			triggerButtonShootDown = false;

			if (GameSave.nimWeapon == CurrentWeapon.PineappleGun) {

				forceHudGrenade = NimHud.instance.hudForce.GetComponent<ForceHud>();
				energyHudGrenade = NimHud.instance.hudPineapple.GetComponent<EnergyHud>();


				//suficient energy to throw grenade
				if (energyHudGrenade.Max ()) {
					prepareGrenade = true;
					pineappleGun.PrepareGrenade ();
					forceHudGrenade.StartPush ();
				}
			}
		}

		if (triggerButtonShootUp && prepareGrenade)
        {
            triggerButtonShootUp = false;
			prepareGrenade = false;

			if (GameSave.nimWeapon == CurrentWeapon.PineappleGun) {

				if (energyHudGrenade.Max ()) {
					pineappleGun.ShootGrenade ();	//animation
					pineappleGun.Shoot (timerButtonDown);
					forceHudGrenade.StopPush ();
				}
			}

            timerButtonDown = 0f;
        }

	}




    public void PickMelee()
    {
        GameSave.nimWeapon = CurrentWeapon.None;
        GameSave.nimGunWeapon = null;   //none

		NimHud.instance.SetHud(NimHud.instance.hudMelee);
    }

    public void PickFireGun()
    {
        GameSave.nimWeapon = CurrentWeapon.WatermelonGun;
        GameSave.nimGunWeapon = watermelonGun;

		NimHud.instance.SetHud(NimHud.instance.hudWatermelon);
    }

    public void PickPineapple()
    {
        GameSave.nimWeapon = CurrentWeapon.PineappleGun;
        GameSave.nimGunWeapon = pineappleGun;

		NimHud.instance.SetHud(NimHud.instance.hudPineapple);
    }

    public void PickBanana(){
        GameSave.nimWeapon = CurrentWeapon.BananaGun;
        GameSave.nimGunWeapon = bananaGun;

		NimHud.instance.SetHud(NimHud.instance.hudBanana);
	}

	

    

    

    public bool GunCharged(){
		if (GameSave.nimGunWeapon != null)
			return true;
		else
			return false;
	}

    public bool NoGun()
    {
        return (GameSave.nimGunWeapon == null);
    }

    public bool Empty(){
		return (GameSave.nimWeapon == CurrentWeapon.None);
	}
}
