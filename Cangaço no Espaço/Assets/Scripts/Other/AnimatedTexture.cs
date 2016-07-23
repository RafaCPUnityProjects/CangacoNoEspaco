using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour {

	public int materialIndex = 0;
	public Vector2 uvAnimationRate = Vector2.up + Vector2.right;
	public string textureName = "_MainTex";

	Vector2 uvOffset = Vector2.zero;

	Renderer myRenderer;

	void Awake(){
		myRenderer = GetComponent<Renderer> ();
	}

	void LateUpdate() 
	{
		uvOffset += (-uvAnimationRate * Time.deltaTime);

		if(myRenderer.enabled){
			myRenderer.materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
		}
	}
}
