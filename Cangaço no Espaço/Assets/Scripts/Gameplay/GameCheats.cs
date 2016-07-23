using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCheats : MonoBehaviour
{
	public Text imortalStateText;
	public bool imortal = false;
	public WeaponController weaponController;
	public NimHealth nimHealth;

	void Start()
	{
		SetImortalText();
	}

	private void FindNimHealth()
	{
		if (!nimHealth)
		{
			nimHealth = FindObjectOfType<NimHealth>();
		}
	}

	private void FindWeaponController()
	{
		if (!weaponController)
		{
			weaponController = FindObjectOfType<WeaponController>();
		}
	}

	public void ChangeImortalState()
	{
		imortal = !imortal;
		SetImortalText();
	}

	public void MeleeGun()
	{
		FindWeaponController();
		weaponController.PickMelee();
	}


	public void WatermelonGun()
	{
		FindWeaponController();

		weaponController.PickFireGun();
	}

	public void PineappleGun()
	{
		FindWeaponController();

		weaponController.PickPineapple();
	}

	public void BananaGun()
	{
		FindWeaponController();

		weaponController.PickBanana();
	}

	private void SetImortalText()
	{
		imortalStateText.text = "Imortality: " + imortal;
	}

	public void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void AddLife()
	{
		FindNimHealth();

		nimHealth.AddHealth();
		
	}

	public void ReduceLife()
	{
		FindNimHealth();

		nimHealth.Damage(1);
	}

#if !UNITY_EDITOR
	void Awake()
	{
		this.gameObject.SetActive(false);
	}
#endif
}
