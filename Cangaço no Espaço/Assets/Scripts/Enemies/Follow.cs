using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	Transform target;

	void Awake(){
		//GameplayEvents.nimOnStage.Subscribe (OnNimOnStage);
	}

	void Start(){
		target = transform.parent;
		transform.parent = null;
	}

	//void OnNimOnStage(NimHealth nimHealth){
	//	target = nimHealth.transform;
	//}

	void Update () {
		if(target != null)
			transform.position = target.position;
	}
}
