/*
Example class using the A* algorithm to track target position
*/

using UnityEngine;
using System.Collections;

public class ExampleSeekerUnit : MonoBehaviour
{
	public Transform target;
	public Retroboy.DungeonGenerator dg;
	public float speed = 5.0f;
	Vector3[] path;
	int targetIndex;
	bool searchingTarget = false;

	void Start()
	{
		//StartCoroutine("FindTarget");
	}

	void Update()
	{
		if (target == null)
		{
			target = FindObjectOfType<NimHealth>().transform;
		}
		else
		{
			if (dg.mapPrinted)
			{
				if (!searchingTarget)
				{
					StartCoroutine("FindTarget");
				}
			}
		}
	}

	IEnumerator FindTarget()
	{
		searchingTarget = true;
		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound); //Example Path Requisition
		yield return null;
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		print("path found: " + pathSuccessful);
		if (pathSuccessful)
		{
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
		else
		{
			searchingTarget = false;
		}
	}

	IEnumerator FollowPath()
	{
		print("following path");
		Vector3 currentWaypoint = path[0];
		while (true)
		{
			if (transform.position == currentWaypoint)
			{
				targetIndex++;
				if (targetIndex >= path.Length)
				{
					searchingTarget = false;
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;
		}
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawSphere(path[i], .1f);

				if (i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}
#endif
}
