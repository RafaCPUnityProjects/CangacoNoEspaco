using UnityEngine;
using System.Collections;

public class Room2RoomTeleport : MonoBehaviour
{
	public Room2RoomTeleport teleportTarget;
	public RoomControl roomControl;

	public bool justTeleportedTo = false;

	public Color unusedColor = Color.white;
	public Color usedColor = Color.red;
	public Color inactiveColor = Color.gray;

	bool used = false;
	Animator myAnimator;
	SpriteRenderer sprite;
	private TeleportState state = TeleportState.activeUnused;

	void Start()
	{
		myAnimator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
		roomControl = transform.parent.parent.GetComponentInChildren<RoomControl>();

		if (teleportTarget == null)
		{
			gameObject.SetActive(false);
		}
		else
		{
			UpdateState();
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (!justTeleportedTo)
		{
			if (other.tag == "Player")
			{
				if (state != TeleportState.inactive)
				{
					roomControl.ExitedRoom();
					other.transform.position = teleportTarget.transform.position;
					teleportTarget.justTeleportedTo = true;
					teleportTarget.roomControl.RoomReached();
					used = true;
					ActivateTeleport(true);
					teleportTarget.used = true;
					teleportTarget.roomControl.EnteredRoom();
				}
			}
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (justTeleportedTo)
		{
			if (other.tag == "Player")
			{
				justTeleportedTo = false;
			}
		}
	}

	public void ActivateTeleport(bool activate)
	{
		if (activate)
		{
			if (used)
			{
				state = TeleportState.activeUsed;
			}
			else
			{
				state = TeleportState.activeUnused;
			}
		}
		else
		{
			state = TeleportState.inactive;
		}
		UpdateState();
	}

	private void UpdateState()
	{
		bool active;
		Color color;

		switch (state)
		{
			case TeleportState.inactive:
				active = false;
				color = inactiveColor;
				break;
			case TeleportState.activeUnused:
				active = true;
				color = unusedColor;
				break;
			case TeleportState.activeUsed:
				active = true;
				color = usedColor;
				break;
			default:
				active = false;
				color = inactiveColor;
				break;
		}

		if (myAnimator && myAnimator.isActiveAndEnabled)
		{
			myAnimator.SetBool("isActive", active);
		}

		if (sprite)
		{
			sprite.color = color;
		}
	}
}

public enum TeleportState
{
	inactive,
	activeUnused,
	activeUsed,
}
