using UnityEngine;

public class PrefabInstance : MonoBehaviour
{
	public GameObject prefab;

	void Awake()
	{
		Destroy(GetComponent<tk2dSprite>());
		GameObject go = (GameObject)Instantiate(prefab);
		go.transform.parent = this.transform;
		go.transform.localPosition = Vector3.zero;
	}
}