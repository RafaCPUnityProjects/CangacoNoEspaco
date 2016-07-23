using UnityEngine;
using System.Collections;

public class ShootByAnimation : MonoBehaviour {

	public GunWeapon gun;

	public void TriggerShoot(){
		gun.ShootBullet ();
	}
}
