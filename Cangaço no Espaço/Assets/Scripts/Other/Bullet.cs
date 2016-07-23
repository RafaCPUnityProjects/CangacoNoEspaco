using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float lifespan = 0.5f; //distance of bullet
    float timedLifeSpan;

    public int damage = 1;
	public LayerMask wallLayer;

	public GameObject explosion;

    [Range(0.1f, 1f)]
    public float timeRange = 1f;    //create variations in time of the bullet

	public bool paralize;
	public float paralizeSeconds;

    //Use OnEnable for pooling instead of Start!
    void OnEnable()
	{
        //Setting variables
        timedLifeSpan = Random.Range(lifespan * timeRange, lifespan);
	}

	void OnDisable()
	{
		
	}

	void Update () {
        timedLifeSpan -= Time.deltaTime;
		if (timedLifeSpan <= 0f) {
			Impact ();
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		//if layer of collider is in one of layers of "wallLayer"
		if((wallLayer.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer){
			//Debug.Log ("Vel:"+GetComponent<Rigidbody2D>().velocity.magnitude);
			Invoke ("Impact", 0.1f / GetComponent<Rigidbody2D>().velocity.magnitude);
		}
	}

	public void Impact(){
		if(explosion != null)
			Instantiate (explosion, transform.position, Quaternion.identity);
		Destroy (this.gameObject);
	}
}
