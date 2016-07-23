using UnityEngine;
using System.Collections;

public class FadeTrail : MonoBehaviour {

	//http://forum.unity3d.com/threads/how-to-create-ghost-or-after-image-on-an-animated-sprite.334079/

	public float repeatRate = 0.2f;
	public float lifeTime = 0.5f;

	public void Stop(){
		CancelInvoke ();
	}

	public void Enable(){
		InvokeRepeating("SpawnTrail", 0, repeatRate); // replace 0.2f with needed repeatRate
	}

	void SpawnTrail()
	{
		if (!this.gameObject.activeSelf)
			return;

		GameObject trailPart = new GameObject();
		SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
		trailPartRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
		trailPart.transform.position = transform.position;
		Destroy(trailPart, lifeTime);

		trailPartRenderer.flipX = (transform.parent.localScale.x < 0);
		trailPartRenderer.sortingLayerName = "Background";
		trailPartRenderer.sortingOrder = 0;

		StartCoroutine("FadeTrailPart", trailPartRenderer);
	}

	IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
	{
		Color color = trailPartRenderer.color;
		color.a -= 0.5f; // replace 0.5f with needed alpha decrement
		trailPartRenderer.color = color;

		yield return new WaitForEndOfFrame();
	}
}
