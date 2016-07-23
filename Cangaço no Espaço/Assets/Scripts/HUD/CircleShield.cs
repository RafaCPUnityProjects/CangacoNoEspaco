using UnityEngine;
using System.Collections;

public class CircleShield : MonoBehaviour {

    public float speed = 3f;
    
	void Update () {
        transform.Rotate(new Vector3(0f, 0f, speed * Time.deltaTime));
	}
}
