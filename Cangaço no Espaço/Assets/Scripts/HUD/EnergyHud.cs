using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnergyHud : MonoBehaviour {

	public Sprite[] hudSprites;
	SpriteRenderer sr;
	Slider energySlider;
	int maxEnergy;
	public int energy;

	[Range(0.1f, 1f)]
	public float energyPerShoot = 0.5f;

	public float timeBetweenShoots = 1f;   //seconds
	private bool canShoot = true;

	public float timeToRecoverHeat = 3f;   //seconds
	//private bool weaponHeat;

	public float timeToRecoverOne = 0.1f;   //seconds

	public Color mainColor;
	public Color noEnergyColor;

    public int dashesPerEnergy = 2;
    
	void Start(){

		maxEnergy = hudSprites.Length;
		energy = maxEnergy;

		sr = GetComponent<SpriteRenderer> ();
		InvokeRepeating ("RecoverEnergy", 0f, timeToRecoverOne);
	}

	void Update(){
		UpdateSprite ();
		UpdateColor ();

        if (this.isActiveAndEnabled){
            sr.enabled = true;
        }
    }

	public void SpriteVisible(bool enabled){
		if(sr != null)
			sr.enabled = enabled;
	}

	void UpdateSprite(){
		int e = hudSprites.Length - energy;

		if (e <= hudSprites.Length - 1 && e >= 0) {
			sr.sprite = hudSprites [e];
		}
	}

	void UpdateColor(){
		if (canShoot && energy >= EnergyCost()) {
			sr.color = mainColor;
		} else {
			sr.color = noEnergyColor;
		}
	}

	void RecoverEnergy(){

		//disable energy recovering betweenShoots time
		if (canShoot) {
			energy++;

			if (energy > maxEnergy)
				energy = maxEnergy;
		}
	}

	public bool Max(){
		return energy >= maxEnergy;
	}

	public bool Min(){
		return energy <= 0;
	}

	void EnableShoot(){
		canShoot = true;
	}

	void DisableHeat()
	{
		//weaponHeat = false;
	}

	public bool CanShoot(){

		//Still cannot shoot (timeBetweenShoots prevent this)
		if (!canShoot)
			return false;

        //DisableShoot();

        //returns true if dropped energy
        if(DropEnergy())
        {
            DisableShoot();
            return true;
        }
        else
        {
            return false;
        }
	}

    public bool CanDash()
    {
        if (!canShoot)
            return false;

        int energyToDrop = maxEnergy / dashesPerEnergy;

        if (energy >= energyToDrop)
        {
            energy = energy - energyToDrop;
            if (energy <= 0)
            {
                energy = 0;
                Invoke("DisableHeat", timeToRecoverHeat);
                //weaponHeat = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    void DisableShoot()
    {
        canShoot = false;
        CancelInvoke ("EnableShoot");
        Invoke("EnableShoot", timeBetweenShoots);
    }

    bool DropEnergy(){
		
		int energyToDrop = EnergyCost();

		if (energy >= energyToDrop)
		{
			energy = energy - energyToDrop;
			if (energy <= 0)
			{
				energy = 0;
				Invoke("DisableHeat", timeToRecoverHeat);
				//weaponHeat = true;
			}

			//Disable shoot for some msec
			//canShoot = false;
			//CancelInvoke ("EnableShoot");
			//Invoke("EnableShoot", timeBetweenShoots);

			return true;
		}
		else
		{
			return false;
		}
	}
    
    int EnergyCost(){
		return (int)(maxEnergy * energyPerShoot);
	}

}
