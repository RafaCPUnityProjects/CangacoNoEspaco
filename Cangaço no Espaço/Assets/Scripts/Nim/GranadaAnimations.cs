using UnityEngine;
using System.Collections;

public class GranadaAnimations : MonoBehaviour {

	Rigidbody2D rb;
	public Animator anim;
    public float delayShowSprite;
    public GameObject myGranada;

	void Awake () {
		rb = GetComponent<Rigidbody2D> ();
	}

	void OnEnable(){
        Invoke("ShowSprite", delayShowSprite);
	}
	
	void Update () {
		anim.SetFloat ("horizontal", Mathf.Abs(rb.velocity.x));
		anim.SetFloat ("vertical", Mathf.Abs(rb.velocity.y));

		Debug.Log ("velx:"+rb.velocity.x);
	}

    void ShowSprite()
    {
        myGranada.SetActive(true);
        anim.SetTrigger("large");
    }
}
