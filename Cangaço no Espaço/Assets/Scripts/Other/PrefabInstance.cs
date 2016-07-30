using UnityEngine;

public class PrefabInstance : MonoBehaviour
{
	public GameObject prefab;

	void Awake()
	{
		GameObject go = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
		//go.transform.parent = this.transform;
		//go.transform.localPosition = Vector3.zero;
		//GetComponent<SpriteRenderer>().enabled = false;
		Destroy(this.gameObject);
	}
}