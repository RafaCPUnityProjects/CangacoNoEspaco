using UnityEngine;
using System.Collections;

public class PickableItem : MonoBehaviour
{
	public PickableItemType myType;
	public int modifier = 1;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.parent.tag == "Player")
		{
			NPCController npcController = other.transform.parent.GetComponent<NPCController>();
			ChangePlayer(npcController);
			Destroy(this.gameObject);
		}
	}

	void ChangePlayer(NPCController controller)
	{
		switch (myType)
		{
			case PickableItemType.normalWeapon:
				controller.PickNormalWeapon(modifier);
				break;
			case PickableItemType.bigWeapon:
				controller.PickBigWeapon(modifier);
				break;
			case PickableItemType.health:
				controller.PickHealth(modifier);
				break;
			case PickableItemType.healthBuff:
				controller.PickHealthBuff(modifier);
				break;
			case PickableItemType.speedBuff:
				controller.PickSpeedBuff(modifier);
				break;
			case PickableItemType.strengthBuff:
				controller.PickStrengthBuff(modifier);
				break;
			default:
				break;
		}
	}
}

public enum PickableItemType
{
	normalWeapon,
	bigWeapon,
	health,
	healthBuff,
	speedBuff,
	strengthBuff,
}
