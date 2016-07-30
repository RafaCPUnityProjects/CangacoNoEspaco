using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour
{
	public GameObject lifeElements;
	public GameObject speedElements;
	public GameObject strengthElements;
	public int fullLifeSpriteId;
	public int emptyLifeSpriteId;

	private tk2dSprite[] lifeSprites;
	private tk2dSprite[] speedSprites;
	private tk2dSprite[] strengthSprites;

	void Start()
	{
		lifeSprites = lifeElements.GetComponentsInChildren<tk2dSprite>();
		speedSprites = speedElements.GetComponentsInChildren<tk2dSprite>();
		strengthSprites = strengthElements.GetComponentsInChildren<tk2dSprite>();
	}

	public void SetLife(int currentLife, int maxLife)
	{
		for (int i = 0; i < lifeSprites.Length; i++)
		{
			lifeSprites[i].gameObject.SetActive(i < maxLife);

			if (i < currentLife)
			{
				lifeSprites[i].SetSprite(fullLifeSpriteId);
			}
			else
			{
				lifeSprites[i].SetSprite(emptyLifeSpriteId);
			}
		}
	}

	public void SetSpeed(int currentSpeed)
	{
		for (int i = 0; i < speedSprites.Length; i++)
		{
			speedSprites[i].gameObject.SetActive(i < currentSpeed);
		}
	}

	public void SetStrength(int currentStrength)
	{
		for (int i = 0; i < strengthSprites.Length; i++)
		{
			strengthSprites[i].gameObject.SetActive(i < currentStrength);
		}
	}
}
