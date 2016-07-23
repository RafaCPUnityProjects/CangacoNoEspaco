using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Retroboy;

public class BossController : MonoBehaviour
{
	public GameObject[] bossAspects;
	public int bossTotalHealth = 40;
	public int lifePerAspect = 10;

	Stack<GameObject> aspectSequence = new Stack<GameObject>();
	GameObject currentBoss;

	public void InitializeBoss()
	{
		EnemyHealth[] enemiesLeft = FindObjectsOfType<EnemyHealth>();
		bossTotalHealth += enemiesLeft.Length;
		int aspectIndex = UnityEngine.Random.Range(0, bossAspects.Length);
		for (int i = lifePerAspect; i <= bossTotalHealth; i += lifePerAspect)
		{
			aspectSequence.Push(bossAspects[aspectIndex]);

			aspectIndex++;
			if (aspectIndex >= bossAspects.Length)
			{
				aspectIndex = 0;
			}
		}
		print("boss stages:" + aspectSequence.Count);
		ChangeAspect();
	}

	public void ChangeAspect()
	{
		print("aspectchanged");
		if (aspectSequence.Count > 0)
		{
			currentBoss = Instantiate(aspectSequence.Pop(), this.transform.position, Quaternion.identity) as GameObject;
			currentBoss.GetComponent<EnemyHealth>().SetupBoss(lifePerAspect, (BossController)this);
		}
		else
		{
			BossKilled();
		}
	}

	public void BossDied()
	{
		print("bossDied");
		Destroy(currentBoss);
		ChangeAspect();
	}

	private void BossKilled()
	{
		print("bosskilled");
		FindObjectOfType<NimHealth>().PlayerWon();

		Destroy(this.gameObject);
	}
}
