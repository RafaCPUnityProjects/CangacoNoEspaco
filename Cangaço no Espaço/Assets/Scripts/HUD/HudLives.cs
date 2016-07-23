using UnityEngine;
using System.Collections;

public class HudLives : MonoBehaviour {

	LineRenderer lr;
	public Transform[] vertexes;
	Vector3[] positions;

	void Awake(){
		lr = GetComponent<LineRenderer> ();
	}

	void Start(){

		positions = new Vector3[vertexes.Length+1];
		VertexPositions ();
	}

	void Update(){
		VertexPositions ();
	}

	void VertexPositions(){
		for(int i = 0; i < vertexes.Length; i++){
			positions[i] = vertexes[i].position;
		}
		//last position is the first vertex again
		positions[vertexes.Length] = vertexes[0].position;

		lr.SetPositions(positions);
	}
}
