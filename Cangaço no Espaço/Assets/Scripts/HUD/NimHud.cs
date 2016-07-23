using UnityEngine;
using System.Collections;

public class NimHud : MonoBehaviour {

	public static NimHud instance;

    public GameObject hudMelee;
	public GameObject hudWatermelon;
	public GameObject hudPineapple;
	public GameObject hudBanana;

	public GameObject hudForce;

    void Awake(){
		if (instance == null)
			instance = this;
        DontDestroyOnLoad(this);
	}

	public void SetHud(GameObject hud)
    {
		hudMelee.SetActive(false);
		hudWatermelon.SetActive(false);
		hudPineapple.SetActive(false);
		hudBanana.SetActive(false);

		hud.SetActive(true);
    }
    
}
