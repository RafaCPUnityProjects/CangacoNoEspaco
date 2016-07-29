using UnityEngine;
using System.Collections;

public class BarrilController : MonoBehaviour
{
	[Range(0.0f, 1.0f)]
	public float dropChance;
	public GameObject[] possibleDrops;
	public int barrilLife = 1;

	public int barrilOpenId;
	public int barrilDoubledId;

	public void DropItem()
	{
		barrilLife--;
		if (barrilLife > 0)
		{
			if (UnityEngine.Random.Range(0.0f, 1.0f) <= dropChance)
			{
				if (possibleDrops.Length > 0)
				{
					GameObject go = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Length)];
					Instantiate(go, transform.position, Quaternion.identity);
				}
			}

			GetComponent<tk2dSprite>().SetSprite(barrilOpenId);
		}
		else
		{
			GetComponent<tk2dSprite>().SetSprite(barrilDoubledId);
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}
}
