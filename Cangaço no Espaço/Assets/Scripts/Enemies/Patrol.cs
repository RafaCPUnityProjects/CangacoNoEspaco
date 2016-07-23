using UnityEngine;
using System.Collections;
using Retroboy;

public class Patrol : MonoBehaviour
{

	//http://answers.unity3d.com/questions/429623/enemy-movement-from-waypoint-to-waypoint.html
	//http://www.blueraja.com/blog/404/how-to-use-unity-3ds-linear-interpolation-vector3-lerp-correctly

	public Transform[] waypoint;        // The amount of Waypoint you want
	public float patrolSpeed = 3f;       // The walking speed between Waypoints
	public bool loop = true;       // Do you want to keep repeating the Waypoints
								   //public float dampingLook= 6.0f;          // How slowly to turn
	public float pauseDuration = 0;   // How long to pause at a Waypoint

	private float curTime;
	private int currentWaypoint = 0;
	//private Rigidbody2D character;

	//lerping
	public float timeTakenDuringLerp = 3f;
	private bool _isLerping;
	private Vector3 _startPosition;
	private Vector3 _endPosition;
	private float _timeStartedLerping;

	public GameObject waypoints;

	const float TILESIZE = 0.217f;
	public int roomSize = 7;

	//float scale;
	//public tk2dBaseSprite sprite;
	public GameObject dot;

	//private DungeonGenerator dg;
	//private Rect myRoom;

	//public Vector3 startPos;

	public bool stop = false;
	Animator anim;

	void Start()
	{
		//startPos = transform.position;
		//character = GetComponent<Rigidbody2D>();
		//dg = FindObjectOfType<DungeonGenerator>();
		//FindMyRoom();
		CreateWaypoints(roomSize, roomSize);

		anim = transform.FindChild ("Sprite").GetComponent<Animator> ();
	}

	//void FindMyRoom()
	//{
	//	scale = dg.tileMap.SpriteCollectionInst.invOrthoSize / (dg.tileMap.SpriteCollectionInst.halfTargetHeight * 2);
	//	print("scale = " + scale);
	//	Vector3 pos = transform.position / scale;
	//	print("pos: " + pos);
	//	bool found = false;
	//	scale = TILESIZE;
	//	if (dg)
	//	{
	//		for (int i = 0; i < dg.allRooms.Count; i++)
	//		{
	//			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
	//			go.transform.position = scale * dg.allRooms[i].roomSpace.center;
	//			go.transform.localScale = scale * (new Vector3(dg.allRooms[i].roomSpace.size.x, 1, dg.allRooms[i].roomSpace.size.y));
	//			go.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
	//			go.name = "MAP ROOM";
	//			if (dg.allRooms[i].roomSpace.Contains(pos))
	//			{
	//				myRoom = dg.allRooms[i].roomSpace;
	//				found = true;
	//				print("my room found: " + i);
	//				break;
	//			}
	//		}
	//	}
	//	else
	//	{
	//		print("no dg");
	//	}

	//	if (!found)
	//	{
	//		print("myRoom not found");
	//	}
	//}

	public void CreateWaypoints(int tilesWidth, int tilesHeight, int margin = 0)
	{
		//if (myRoom != null)
		//{
		//	waypoint[0] = scale * (new Vector2(myRoom.xMin, myRoom.yMin));
		//	waypoint[1] = scale * (new Vector2(myRoom.xMax, myRoom.yMin));
		//	waypoint[2] = scale * (new Vector2(myRoom.xMax, myRoom.yMax));
		//	waypoint[3] = scale * (new Vector2(myRoom.xMin, myRoom.yMax));
		//}
		//else
		//{
		//enemy is on the center of room
		float tilesX = tilesWidth / 2 - margin;
		float tilesY = tilesHeight / 2 - margin;

		GameObject newWaypoint;
		newWaypoint = (GameObject)Instantiate(new GameObject(),
			transform.position + new Vector3(tilesX * TILESIZE, tilesY * TILESIZE, 0f),
			transform.rotation);
		newWaypoint.transform.parent = waypoints.transform;
		waypoint[0] = newWaypoint.transform;
		newWaypoint = (GameObject)Instantiate(new GameObject(),
			transform.position + new Vector3(tilesX * TILESIZE, -tilesY * TILESIZE, 0f),
			transform.rotation);
		newWaypoint.transform.parent = waypoints.transform;
		waypoint[1] = newWaypoint.transform;
		newWaypoint = (GameObject)Instantiate(new GameObject(),
			transform.position + new Vector3(-tilesX * TILESIZE, -tilesY * TILESIZE, 0f),
			transform.rotation);
		newWaypoint.transform.parent = waypoints.transform;
		waypoint[2] = newWaypoint.transform;
		newWaypoint = (GameObject)Instantiate(new GameObject(),
			transform.position + new Vector3(-tilesX * TILESIZE, tilesY * TILESIZE, 0f),
			transform.rotation);
		newWaypoint.transform.parent = waypoints.transform;
		waypoint[3] = newWaypoint.transform;
		//}

		//CreateDots();
	}

	void CreateDots()
	{
		CreateLine(waypoint[0].position, waypoint[1].position);
		CreateLine(waypoint[1].position, waypoint[2].position);
		CreateLine(waypoint[2].position, waypoint[3].position);
		CreateLine(waypoint[3].position, waypoint[0].position);
	}

	void CreateLine(Vector3 origin, Vector3 dest)
	{

		float dist = Vector3.Distance(origin, dest);
		int nTiles = Mathf.RoundToInt(dist / TILESIZE);

		for (int i = 0; i <= nTiles; i++)
		{
			GameObject newDot = (GameObject)Instantiate(dot, Vector3.Lerp(origin, dest, (float)i / nTiles), Quaternion.identity);
			ShowRandomDelay srd = newDot.GetComponent<ShowRandomDelay>();
			srd.seconds = (float)i / 10;
		}
	}

	void Update()
	{
		if (stop)
			return;
		
		if (currentWaypoint < waypoint.Length)
		{
			DoPatrol();
		}
		else {
			if (loop)
			{
				currentWaypoint = 0;
			}
		}
	}

	void SetAnimation(){

		Vector3 target;

		if (currentWaypoint < waypoint.Length)
			target = waypoint[currentWaypoint].position;
		else
			target = waypoint[0].position;

		target.z = transform.position.z; // Keep waypoint at character's height
		Vector3 moveDirection = target - transform.position;

		if (Mathf.Abs(moveDirection.normalized.y) <= 0.1f) {
			//right
			anim.SetTrigger("goLeftRight");
		} else {
			//updown
			if (moveDirection.normalized.y > 0.1f) {
				//up
				anim.SetTrigger("goUp");
			} else if (moveDirection.normalized.y < 0.1f) {
				//down
				anim.SetTrigger("goDown");
			}
		}
	}

	void DoPatrol()
	{

		Vector3 target = waypoint[currentWaypoint].position;
		target.z = transform.position.z; // Keep waypoint at character's height
		Vector3 moveDirection = target - transform.position;



		if (moveDirection.magnitude < 0.01f)
		{
			if (curTime == 0)
				curTime = Time.time; // Pause over the Waypoint
			
			if ((Time.time - curTime) >= pauseDuration)
			{
				currentWaypoint++;
				curTime = 0;

				SetAnimation ();
			}
		}
		else
		{
			//var rotation= Quaternion.LookRotation(target - transform.position);
			//transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);

			//transform.position = moveDirection.normalized * patrolSpeed * Time.deltaTime;
			//transform.position = Vector3.Lerp (transform.position, target, Time.deltaTime);

			if (!_isLerping)
			{
				StartLerping(target);
			}
		}
	}

	void FixedUpdate()
	{
		if (_isLerping)
		{
			float timeSinceStarted = Time.time - _timeStartedLerping;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

			if (percentageComplete >= 1.0f)
			{
				_isLerping = false;
			}
		}
	}


	void StartLerping(Vector3 target)
	{
		_isLerping = true;
		_timeStartedLerping = Time.time;

		_startPosition = transform.position;
		_endPosition = target;
	}



}