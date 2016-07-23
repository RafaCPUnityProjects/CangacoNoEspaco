using UnityEngine;
using System.Collections;

public class BossTeleport : MonoBehaviour
{
	public Vector3 offset = new Vector3(4, 2, 0);

	Transform teleportTarget;
	BossController bossController;

	void Awake()
	{
		teleportTarget = GameObject.Find("BossRoom").transform;
		bossController = FindObjectOfType<BossController>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			bossController.InitializeBoss();
			other.transform.position = teleportTarget.position + offset;
		}
	}
}
