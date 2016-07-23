using UnityEngine;
using System.Collections;

public class DropMove : MonoBehaviour {

    const float timeToEnableCollision = 0.2f;
	[HideInInspector]
    public bool canBePicked;

    void OnEnable () {
        canBePicked = false;
        Invoke("EnablePickup", timeToEnableCollision);
	}

    void EnablePickup () {
        canBePicked = true;
    }

}
