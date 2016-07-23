using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RoomControl : MonoBehaviour
{
	public List<GameObject> roomEnemies = new List<GameObject>();
	public Room2RoomTeleport[] teleports;
	public Retroboy.DungeonGenerator dg;
	public tk2dSprite minimapSprite;

	//private bool waiting = false;
	private bool insideRoom = false;
	private bool canLeaveRoom;
	private Color startColor;

	public void InitializeRoom()
	{
		//teleports = transform.GetComponentsInChildren<Room2RoomTeleport>();
		startColor = minimapSprite.color;

		roomEnemies.Clear();
		EnemyHealth[] enemyHealth = transform.GetComponentsInChildren<EnemyHealth>();
		foreach (var enemy in enemyHealth)
		{
			roomEnemies.Add(enemy.gameObject);

			enemy.gameObject.SetActive(false);
		}
		SetTeleportStates(true);
	}

	public void Update()
	{
		if (insideRoom)
		{
			for (int i = 0; i < roomEnemies.Count; i++)
			{
				if (roomEnemies[i] == null)
				{
					roomEnemies.RemoveAt(i);
				}
			}
			SetTeleportStates(roomEnemies.Count <= 0);
		}
	}

	public void EnteredRoom()
	{
		insideRoom = true;
		minimapSprite.color = Color.yellow;
	}

	public void ExitedRoom()
	{
		insideRoom = false;
		minimapSprite.color = startColor;
	}

	public void RoomReached()
	{
		StartCoroutine(WaitAndShow());
		startColor = Color.white;
	}

	public IEnumerator WaitAndShow()
	{
		//waiting = true;
		yield return new WaitForSeconds(dg.waitTimeToShowEnemies);
		foreach (var enemy in roomEnemies)
		{
			if (enemy)
			{
				enemy.SetActive(true);
			}
		}
		//waiting = false;
	}

	private void SetTeleportStates(bool active)
	{
		foreach (var tp in teleports)
		{
			tp.ActivateTeleport(active);
		}
	}

	public void ConnectTo(GameObject roomController)
	{
		RoomControl targetRoom = roomController.GetComponent<RoomControl>();

		Room2RoomTeleport myTeleport = ClosestTeleport(roomController.transform);
		Room2RoomTeleport targetTeleport = targetRoom.ClosestTeleport(this.transform);

		if (myTeleport != null && targetTeleport != null)
		{
			myTeleport.teleportTarget = targetTeleport;
			targetTeleport.teleportTarget = myTeleport;
			LineRenderer lr = myTeleport.GetComponentInChildren<LineRenderer>();
			if (lr != null)
			{
				lr.SetPosition(0, myTeleport.transform.position);
				lr.SetPosition(1, targetTeleport.transform.position);
				lr.SetWidth(0.1f, 0.1f);
			}
		}
	}

	public Room2RoomTeleport ClosestTeleport(Transform target)
	{
		float closestDistance = float.MaxValue;
		int closestTeleportIndex = -1;
		for (int i = 0; i < teleports.Length; i++)
		{
			if (teleports[i].teleportTarget == null)
			{
				float calculatedDistance = Vector3.Distance(teleports[i].transform.position, target.position);
				if (calculatedDistance < closestDistance)
				{
					closestTeleportIndex = i;
					closestDistance = calculatedDistance;
				}
			}
		}
		if (closestTeleportIndex >= 0)
		{
			return teleports[closestTeleportIndex];
		}
		else
		{
			Debug.LogError("No teleports available " + this.transform.parent.name);
			return null;
		}
	}
}
