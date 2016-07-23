using UnityEngine;
using System.Collections;

public class NearExplosion : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {     
        //Explode when collide with player
        if (other.CompareTag("PlayerFeet")){
            //transform.parent.GetComponent<EnemyHealth>().Damaged(2);
            transform.parent.GetComponent<EnemyHealth>().NimClose();
        }
    }
}
