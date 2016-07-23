using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Retroboy
{
	public class DungeonGenerator : MonoBehaviour
	{
		//random stuff
		[Header("Random sfuff")]
		public string seed = "0";
		public bool randomSeed = false;
		public System.Random prng;
		public System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

		//map stuff
		[Header("Map Stuff")]
		public int mapWidth = 100;
		public int mapHeight = 100;

		//tile ids
		//[Header("Tile IDs")]

		//room stuff
		[Header("Room stuff")]
		public int roomTries = 1000;
		//public VecInt roomYVariance = new VecInt(5, 7);
		public int roomWallDepth = 1;
		public int connectionsPerRoom = 2;
		//public float roomConnectionTreshold = 7f;
		public List<Room> allRooms = new List<Room>();
		//public GameObject roomPrefab;
		public GameObject room4x4;
		public GameObject room5x5;
		public GameObject room6x6;
		public GameObject room7x7;

		//corridor stuff
		//[Header("Corridor stuff")]
		//public int corridorWallDepth = 1;
		//public int extraCorridor = 2;

		//populate stuff
		[Header("Populate stuff")]
		//[Range(1, 20)]
		//public float easinessLevel = 8.0f;
		//public int maxEnemies = 200;
		//public int superSpawnNumber = 8;
		//public SquadList squadListAsset;
		public GameObject cameraPrefab;
		//[SerializeField]
		//public List<Squad> squadList = new List<Squad>();
		public int safeNumber = 1;
		public int bitNumber = 1;

		public Vector2 roomOffset = new Vector2(.5f, .5f);

		public GameObject[] squads4x4;
		public GameObject[] squads5x5;
		public GameObject[] squads6x6;
		public GameObject[] squads7x7;

		public GameObject[] bitSquads4x4;
		public GameObject[] bitSquads5x5;
		public GameObject[] bitSquads6x6;
		public GameObject[] bitSquads7x7;

		public GameObject[] safeRoom4x4;
		public GameObject[] safeRoom5x5;
		public GameObject[] safeRoom6x6;
		public GameObject[] safeRoom7x7;

		public float waitTimeToShowEnemies = 1f;

		//print stuff
		[Header("Print stuff")]
		public tk2dTileMap tileMap;
		public bool mapPrinted = false;

		//private stuff
		private int[,] map;
		private int[,] enemyMap;
		private int[,] holeMap;
		private VecInt roomVariance = new VecInt(4, 7);
		private Room startingRoom;
		private const int mapBorder = 2;
		private List<Corridor> allCorridors = new List<Corridor>();
		private const int holeLayer = 0;
		private const int floorLayer = 1;
		private const int wallLayer = 2;
		private const int enemyLayer = 3;
		private const int maxTeleportCount = 4;
		private float tileSize = 30f / 70f;

		private List<GameObject> roomList = new List<GameObject>();

		private List<Room> safeRooms = new List<Room>();
		private List<Room> bitRooms = new List<Room>();

		//private int highestLevel;


		void Start()
		{
			if (randomSeed)
			{
				seed += DateTime.Now.ToString();
			}
			prng = new System.Random(seed.GetHashCode());
			GenerateDungeon();
		}

		void GenerateDungeon()
		{
			sw.Start();

			InitializeMap();
			print("Map initialized: " + sw.Elapsed.ToString());

			CreateRooms();
			print("Rooms Created: " + sw.Elapsed.ToString());

			ConnectRooms();
			print("Rooms connected: " + sw.Elapsed.ToString());

			ConnectUnconnectedRooms();
			print("Connect unconnected rooms: " + sw.Elapsed.ToString());

			PrintRooms();
			print("Rooms printed: " + sw.Elapsed.ToString());

			PrintCorridors();
			print("Corridors printed: " + sw.Elapsed.ToString());

			CloseGaps();
			print("Gaps Closed: " + sw.Elapsed);

			RemoveBadTiles();
			print("Bad tiles removed: " + sw.Elapsed.ToString());

			//CreateTeleports();
			//print("Teleports printed: " + sw.Elapsed.ToString());

			PlaceWalls();
			print("Walls placed: " + sw.Elapsed.ToString());

			PopulateMap();
			print("Map Populated: " + sw.Elapsed.ToString());

			PrintMap();
			print("Map Printed: " + sw.Elapsed.ToString());

			Instantiate(cameraPrefab);

			sw.Stop();

			mapPrinted = true;
		}

		private void CreateTeleports()
		{
			foreach (var corridor in allCorridors)
			{
				corridor.roomA.roomController.GetComponent<RoomControl>().ConnectTo(corridor.roomB.roomController);
				//corridor.roomB.roomController.GetComponentInChildren<RoomControl>().ConnectTo(corridor.roomA.roomController);
			}
		}

		private void ConnectUnconnectedRooms()
		{
			for (int i = 0; i < allRooms.Count; i++)
			{
				if (!allRooms[i].isConnectedToMainRoom)
				{
					for (int j = 0; j < allRooms[i].roomDistances.Count; j++)
					{
						if (allRooms[i].roomDistances[j].room.isConnectedToMainRoom && allRooms[i].connectedCorridors.Count <= maxTeleportCount)
						{
							Corridor corridor = new Corridor(this, allRooms[i], allRooms[i].roomDistances[j].room, Tiles.empty, Tiles.floor);
							corridor.Connect();
							allCorridors.Add(corridor);
							break;
						}
					}
				}
			}
		}

		private void PopulateMap()
		{
			int closestToCenterIndex = FindClosestToCenter();
			startingRoom = allRooms[closestToCenterIndex];
			allRooms[closestToCenterIndex].SetRoomType(RoomType.start);

			SetRoomLevels(closestToCenterIndex);
			PlaceNim();
			DefineRoomTypes();
			PopulateRooms();
		}

		private void DefineRoomTypes()
		{
			allRooms.Shuffle();
			for (int i = 0; i < allRooms.Count; i++)
			{
				if (allRooms[i].GetRoomType() == RoomType.none)
				{
					if (bitRooms.Count < bitNumber)
					{
						bool goodRoom = true;
						for (int j = 0; j < allRooms[i].connectedRooms.Count; j++)
						{
							if (allRooms[i].connectedRooms[j].GetRoomType() == RoomType.bit || allRooms[i].connectedRooms[j].GetRoomType() == RoomType.start)
							{
								goodRoom = false;
							}
						}
						if (goodRoom)
						{
							bitRooms.Add(allRooms[i]);
							allRooms[i].SetRoomType(RoomType.bit);
						}
					}
					else if (safeRooms.Count < safeNumber)
					{
						bool goodRoom = true;
						for (int j = 0; j < allRooms[i].connectedRooms.Count; j++)
						{
							if (allRooms[i].connectedRooms[j].GetRoomType() == RoomType.safe || allRooms[i].connectedRooms[j].GetRoomType() == RoomType.start)
							{
								goodRoom = false;
							}
						}
						if (goodRoom)
						{
							safeRooms.Add(allRooms[i]);
							allRooms[i].SetRoomType(RoomType.safe);
						}
					}
					else
					{
						//required rooms found
						break;
					}
				}
			}
			//bool ready = false;
			//int maxTries = 1000;
			//while (!ready)
			//{
			//	maxTries--;
			//	if (bitRooms.Count < bitNumber)
			//	{
			//		int index = prng.Next(allRooms.Count);
			//		for (int i = 0; i < allRooms[index].connectedRooms.Count; i++)
			//		{
			//			RoomType roomType = allRooms[index].GetRoomType();
			//			RoomType connectedType = allRooms[index].connectedRooms[i].GetRoomType();
			//			if (roomType == RoomType.none && connectedType != RoomType.start && connectedType != RoomType.bit)
			//			{
			//				bitRooms.Add(allRooms[index]);
			//				allRooms[index].SetRoomType(RoomType.bit);
			//			}
			//		}
			//	}
			//	else if (idleRooms.Count < idleNumber)
			//	{
			//		int index = prng.Next(allRooms.Count);
			//		for (int i = 0; i < allRooms[index].connectedRooms.Count; i++)
			//		{
			//			RoomType roomType = allRooms[index].GetRoomType();
			//			RoomType connectedType = allRooms[index].connectedRooms[i].GetRoomType();
			//			if (roomType == RoomType.none && connectedType != RoomType.start && connectedType != RoomType.bit)
			//			{
			//				idleRooms.Add(allRooms[index]);
			//				allRooms[index].SetRoomType(RoomType.safe);
			//			}
			//		}
			//	}
			//	else
			//	{
			//		ready = true;
			//	}

			//	if (maxTries <= 0)
			//	{
			//		Debug.LogError("Max tries exceeded!");
			//		return;
			//	}
			//}

			for (int i = 0; i < allRooms.Count; i++)
			{
				if (allRooms[i].GetRoomType() == RoomType.none)
				{
					allRooms[i].SetRoomType(RoomType.combat);
				}
				allRooms[i].roomController.name = "Room " + allRooms[i].roomSpace.center + " type: " + Enum.GetName(typeof(RoomType), allRooms[i].GetRoomType());
			}
		}

		private void PlaceNim()
		{
			List<Room> startRoom = new List<Room>();
			startRoom.Add(startingRoom);
			PrintInRandomRoom(startRoom, Tiles.nim, true);
		}

		private void PopulateRooms()
		{
			allRooms.Sort();
			for (int i = allRooms.Count - 1; i >= 0; i--)
			{
				int roomSize = (int)allRooms[i].roomSpace.height - (roomWallDepth * 2); //square rooms
				GameObject[] roomPrefabPool;
				roomPrefabPool = EnemyRandomPool(roomSize, allRooms[i].GetRoomType());
				allRooms[i].SetRoomColor(GetRoomColor(allRooms[i].GetRoomType()));
				if (roomPrefabPool.Length > 0)
				{
					GameObject chosenPrefab = roomPrefabPool[prng.Next(roomPrefabPool.Length)];
					GameObject roomInstance = (GameObject)Instantiate(chosenPrefab);//, allRooms[i].roomController.transform.position, Quaternion.identity);
					roomInstance.transform.parent = allRooms[i].roomController.transform;
					roomInstance.transform.localPosition = Vector3.zero;
				}
				allRooms[i].roomController.GetComponent<RoomControl>().InitializeRoom();
			}
		}

		private Color GetRoomColor(RoomType roomType)
		{
			Color roomColor;
			switch (roomType)
			{
				case RoomType.none:
					roomColor = Color.white;
					break;
				case RoomType.start:
					roomColor = Color.green;
					break;
				case RoomType.safe:
					roomColor = Color.blue;
					break;
				case RoomType.combat:
					roomColor = Color.magenta;
					break;
				case RoomType.bit:
					roomColor = Color.red;
					break;
				default:
					roomColor = Color.white;
					break;
			}

			return roomColor;
		}

		private GameObject[] EnemyRandomPool(int roomSize, RoomType type)
		{
			GameObject[] enemyRandomPool;
			if (type == RoomType.combat)
			{
				switch (roomSize)
				{
					case 4:
						enemyRandomPool = squads4x4;
						break;
					case 5:
						enemyRandomPool = squads5x5;
						break;
					case 6:
						enemyRandomPool = squads6x6;
						break;
					case 7:
						enemyRandomPool = squads7x7;
						break;
					default:
						enemyRandomPool = new GameObject[0];
						break;
				}
			}
			else if (type == RoomType.bit)
			{
				switch (roomSize)
				{
					case 4:
						enemyRandomPool = bitSquads4x4;
						break;
					case 5:
						enemyRandomPool = bitSquads5x5;
						break;
					case 6:
						enemyRandomPool = bitSquads6x6;
						break;
					case 7:
						enemyRandomPool = bitSquads7x7;
						break;
					default:
						enemyRandomPool = new GameObject[0];
						break;
				}
			}
			else if (type == RoomType.safe)
			{
				switch (roomSize)
				{
					case 4:
						enemyRandomPool = safeRoom4x4;
						break;
					case 5:
						enemyRandomPool = safeRoom5x5;
						break;
					case 6:
						enemyRandomPool = safeRoom6x6;
						break;
					case 7:
						enemyRandomPool = safeRoom7x7;
						break;
					default:
						enemyRandomPool = new GameObject[0];
						break;
				}
			}
			else
			{
				enemyRandomPool = new GameObject[0];
			}

			return enemyRandomPool;
		}

		private Room PrintInRandomRoom(List<Room> rooms, int enemyID, bool center = false)
		{
			if (rooms.Count <= 0)
			{
				Debug.LogError("No rooms to print enemies in this list");
				return null;
			}
			Room chosenRoom = rooms[prng.Next(rooms.Count)];
			//print("available tiles: " + chosenRoom.availableTiles.Count);
			if (center)
			{
				VecInt centerTile = new VecInt(chosenRoom.roomSpace.center);
				SetEnemyTile(centerTile.x, centerTile.y, enemyID);
			}
			else if (chosenRoom.availableTiles.Count > 0)
			{
				VecInt randomTile = chosenRoom.availableTiles[prng.Next(chosenRoom.availableTiles.Count)];
				SetEnemyTile(randomTile.x, randomTile.y, enemyID);
				chosenRoom.availableTiles.Remove(randomTile);
			}
			else
			{
				rooms.Remove(chosenRoom);
				if (rooms.Count > 0)
				{
					chosenRoom = PrintInRandomRoom(rooms, enemyID);
				}
				else
				{
					chosenRoom = null;
					Debug.LogError("Ran out of rooms to add enemies");
				}
			}
			return chosenRoom;
		}

		private void SetRoomLevels(int closestToCenterIndex)
		{
			List<List<Room>> roomLevels = new List<List<Room>>();
			List<Room> currentRoomLevel = new List<Room>();
			int currentLevel = 0;
			allRooms[closestToCenterIndex].roomLevel = currentLevel;
			currentRoomLevel.Add(allRooms[closestToCenterIndex]);
			roomLevels.Add(currentRoomLevel);
			while (true)
			{
				List<Room> nextRoomLevel = new List<Room>();
				currentLevel++;
				foreach (var current in currentRoomLevel)
				{
					foreach (var border in current.connectedRooms)
					{
						if (border.roomLevel == -1)
						{
							border.roomLevel = currentLevel;
							nextRoomLevel.Add(border);
							//highestLevel = currentLevel;
							//
						}
					}
				}
				if (nextRoomLevel.Count > 0)
				{
					roomLevels.Add(nextRoomLevel);
					currentRoomLevel = nextRoomLevel;
				}
				else
				{
					break;
				}
			}
		}

		private int FindClosestToCenter()
		{
			Vector3 center = new Vector3(mapWidth / 2f, mapHeight / 2f);
			float minDistance = float.MaxValue;
			int closestToCenterIndex = -1;
			for (int i = 0; i < allRooms.Count; i++)
			{
				float distanceToCenter = Vector3.Distance(allRooms[i].roomSpace.center, center);
				if (distanceToCenter < minDistance)
				{
					minDistance = distanceToCenter;
					closestToCenterIndex = i;
				}
			}

			return closestToCenterIndex;
		}

		private void PrintCorridors()
		{
			foreach (var corridor in allCorridors)
			{
				PrintCorridorOnMap(corridor);
			}
		}

		private void PrintRooms()
		{
			for (int i = 0; i < allRooms.Count; i++)
			{
				Rect newRoom = allRooms[i].roomSpace;
				//Vector2 center = newRoom.center;// * tileSize;
				Vector2 center = allRooms[i].roomSpace.center - roomOffset;// * tileSize;
				Vector2 size = newRoom.size;// * tileSize;

				//center.x = (int)center.x;
				//center.y = (int)center.y;
				size.x = (int)size.x;
				size.y = (int)size.y;
				//size.x = (int)((newRoom.xMax - newRoom.x) * tileSize);
				//size.y = (int)((newRoom.yMax - newRoom.y) * tileSize);

				//size.x -= (2 * roomWallDepth);
				//size.y -= (2 * roomWallDepth);
				int roomSize = (int)size.x - (roomWallDepth * 2);
				GameObject roomPrefab;
				switch (roomSize)
				{
					case 4:
						roomPrefab = room4x4;
						break;
					case 5:
						roomPrefab = room5x5;
						break;
					case 6:
						roomPrefab = room6x6;
						break;
					case 7:
						roomPrefab = room7x7;
						break;
					default:
						Debug.LogError("Wrong room size: " + roomSize);
						return;
						//break;
				}
				GameObject roomGO = (GameObject)Instantiate(roomPrefab, center * tileSize, Quaternion.identity);
				//int roomType = (int)allRooms[i].GetRoomType();
				//print("roomtype" + roomType);
				//roomGO.name = "Room " + center + " type: " + Enum.GetName(typeof(RoomType), roomType);
				roomGO.GetComponent<RoomControl>().dg = this;
				roomList.Add(roomGO);
				allRooms[i].roomController = roomGO;

				for (int y = (int)newRoom.y; y < (int)newRoom.yMax; y++)
				{
					for (int x = (int)newRoom.x; x < (int)newRoom.xMax; x++)
					{
						int type;

						type = Tiles.floor; //floor
						if (x < (int)newRoom.x + roomWallDepth ||
							x >= (int)newRoom.xMax - roomWallDepth ||
							y < (int)newRoom.y + roomWallDepth ||
							y >= (int)newRoom.yMax - roomWallDepth)
						{
							type = Tiles.empty;
						}
						SetMapTile(x, y, type);
					}
				}
			}
		}

		private void InitializeMap()
		{
			map = new int[mapWidth, mapHeight];
			enemyMap = new int[mapWidth, mapHeight];
			holeMap = new int[mapWidth, mapHeight];
			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{
					SetMapTile(x, y, (int)Tiles.empty);
					SetEnemyTile(x, y, (int)Enemy.none);
					SetHoleTile(x, y, (int)Enemy.none);
				}
			}
		}

		private void PlaceWalls()
		{
			for (int y = 1; y < mapHeight - 1; y++)
			{
				for (int x = 1; x < mapWidth - 1; x++)
				{
					if (GetMapTile(x, y) == (int)Tiles.empty) //empty tile
					{
						bool right, left, top, bottom, topRight, bottomRight, topLeft, bottomLeft;

						bool[] adjacentFloors = new bool[8];

						for (int i = 0; i < Direction.all.Length; i++)
						{
							VecInt direction = Direction.all[i];
							adjacentFloors[i] = GetMapTile(x + direction.x, y + direction.y) == (int)Tiles.floor;
						}

						top = adjacentFloors[0];
						bottom = adjacentFloors[1];
						right = adjacentFloors[2];
						left = adjacentFloors[3];
						topRight = adjacentFloors[4];
						bottomRight = adjacentFloors[5];
						topLeft = adjacentFloors[6];
						bottomLeft = adjacentFloors[7];

						//nothing
						if (!top && !bottom && !right && !left && !topRight && !bottomRight && !topLeft && !bottomLeft)
						{
							continue;
						}

						//ortho
						else if (top)
						{
							if (left)
							{
								SetMapTile(x, y, (int)Tiles.wallTopLeftConcave);
							}
							else if (right)
							{
								SetMapTile(x, y, (int)Tiles.wallTopRightConcave);
							}
							else
							{
								SetMapTile(x, y, (int)Tiles.wallTop);
							}
						}
						else if (bottom)
						{
							if (left)
							{
								SetMapTile(x, y, (int)Tiles.wallBottomLeftConcave);
							}
							else if (right)
							{
								SetMapTile(x, y, (int)Tiles.wallBottomRightConcave);
							}
							else
							{
								SetMapTile(x, y, (int)Tiles.wallBottom);
							}
						}
						else if (right)
						{
							if (top)
							{
								SetMapTile(x, y, (int)Tiles.wallTopRightConcave);
							}
							else if (bottom)
							{
								SetMapTile(x, y, (int)Tiles.wallBottomRightConcave);
							}
							else
							{
								SetMapTile(x, y, (int)Tiles.wallRight);
							}
						}
						else if (left)
						{
							if (top)
							{
								SetMapTile(x, y, (int)Tiles.wallTopLeftConcave);
							}
							else if (bottom)
							{
								SetMapTile(x, y, (int)Tiles.wallBottomLeftConcave);
							}
							else
							{
								SetMapTile(x, y, (int)Tiles.wallLeft);
							}
						}

						////convex
						else if (bottomLeft)
						{
							SetMapTile(x, y, (int)Tiles.wallTopRightConvex);
						}
						else if (bottomRight)
						{
							SetMapTile(x, y, (int)Tiles.wallTopLeftConvex);
						}
						else if (topLeft)
						{
							SetMapTile(x, y, (int)Tiles.wallBottomRightConvex);
						}
						else if (topRight)
						{
							SetMapTile(x, y, (int)Tiles.wallBottomLeftConvex);
						}
					}
				}
			}
		}

		private void RemoveBadTiles()
		{
			int cornerCount;
			do
			{
				cornerCount = 0;
				for (int y = 1; y < mapHeight - 1; y++)
				{
					for (int x = 1; x < mapWidth - 1; x++)
					{
						if (GetMapTile(x, y) == (int)Tiles.empty) //empty tile
						{
							int closeFloors = 0;

							for (int i = 0; i < Direction.all.Length; i++)
							{
								if (GetMapTile(x + Direction.all[i].x, y + Direction.all[i].y) == (int)Tiles.floor)
								{
									closeFloors++;
								}
							}

							if (closeFloors >= 6)
							{
								cornerCount++;
								SetMapTile(x, y, (int)Tiles.floor);
							}
						}
					}
				}
			} while (cornerCount > 0);
		}

		private void CloseGaps()
		{
			List<VecInt> gaps = new List<VecInt>();
			//List<VecInt> diagonals = new List<VecInt>();
			for (int y = 1; y < mapHeight - 1; y++)
			{
				for (int x = 1; x < mapWidth - 1; x++)
				{
					if (GetMapTile(x, y) == (int)Tiles.empty)
					{
						bool isGap =
							(GetMapTile(x - 1, y) == (int)Tiles.floor && GetMapTile(x + 1, y) == (int)Tiles.floor) ||
							(GetMapTile(x, y - 1) == (int)Tiles.floor && GetMapTile(x, y + 1) == (int)Tiles.floor);

						if (isGap)
						{
							gaps.Add(new VecInt(x, y));
						}
						/* TO DO: FIX no problema do gap diagonal de corredores (tentar considerar a condição específica
						if (GetMapTile(x - 1, y - 1) == (int)Tiles.floor && GetMapTile(x + 1, y + 1) == (int)Tiles.floor)
						{
							SetMapTile(x - 1, y, (int)Tiles.floor);
							SetMapTile(x + 1, y, (int)Tiles.floor);
						}
						else if (GetMapTile(x + 1, y - 1) == (int)Tiles.floor && GetMapTile(x - 1, y + 1) == (int)Tiles.floor)
						{
							SetMapTile(x, y - 1, (int)Tiles.floor);
							SetMapTile(x, y + 1, (int)Tiles.floor);
						}
						*/
					}
				}
			}

			foreach (var gap in gaps)
			{
				SetMapTile(gap, (int)Tiles.floor);
				print("gap closed " + gap.x + "," + gap.y);
			}

			//foreach (var diagonal in diagonals)
			//{
			//	//for (int y = -1; y <= 1; y++)
			//	//{
			//	//	for (int x = -1; x <= 1; x++)
			//	//	{
			//	//		SetMapTile(diagonal.x + x, diagonal.y + y, (int)Tiles.floor);
			//	//	}
			//	//}
			//	SetMapTile(diagonal, (int)Tiles.floor);
			//	print("diagonal area painted in " + diagonal);
			//}
		}

		void CreateRooms()
		{
			for (int i = 0; i < roomTries; i++)
			{
				VecInt actualVariance = new VecInt(roomVariance.x + roomWallDepth * 2, roomVariance.y + roomWallDepth * 2 + 1);
				int size = ((prng.Next(actualVariance.x, actualVariance.y)));// + 1) - 1) / 2) * 2 + 1;
				Rect newRoom = new Rect(
					(prng.Next(mapBorder, mapWidth - mapBorder) / 2) * 2, //x
					(prng.Next(mapBorder, mapHeight - mapBorder) / 2) * 2, //y
					size, //sizeX
					size);// ((prng.Next(roomYVariance.x, roomYVariance.y + 1) - 1) / 2) * 2 + 1); //sizeY

				if (newRoom.xMax >= mapWidth - mapBorder || newRoom.yMax >= mapHeight - mapBorder || newRoom.xMin <= mapBorder || newRoom.yMin <= mapBorder)
				{
					continue;
				}

				bool overlaps = false;
				foreach (var room in allRooms)
				{
					if (newRoom.Overlaps(room.roomSpace))
					{
						overlaps = true;
						continue;
					}
				}
				if (overlaps)
				{
					continue;
				}
				allRooms.Add(new Room(newRoom, roomWallDepth));
				//Debug.Log("Room " + allRooms.Count + " size " + (allRooms[allRooms.Count - 1].roomSpace.size.x - 2*roomWallDepth));
				//allRooms[allRooms.Count - 1].SetType(RoomType.combat);
			}
			print("Number of rooms: " + allRooms.Count);
		}

		void PrintRoomOnMap(Room room, bool set = true)
		{

		}

		void ConnectRooms()
		{
			allRooms.Sort();
			allRooms[0].isMainRoom = true;
			allRooms[0].ConnectToMainRoom();
			FindNeighborRooms();
			Connect();
		}

		void Connect()
		{
			for (int i = 0; i < allRooms.Count; i++)
			{
				allRooms[i].roomDistances.Sort();
				for (int j = 0; j < connectionsPerRoom; j++)
				{
					{
						if (!allRooms[i].connectedRooms.Contains(allRooms[i].roomDistances[j].room))
						{
							Corridor corridor = new Corridor(this, allRooms[i], allRooms[i].roomDistances[j].room, (int)Tiles.empty, (int)Tiles.floor);
							corridor.Connect();
							allCorridors.Add(corridor);
						}
					}
				}
			}
		}

		void PrintCorridorOnMap(Corridor corridor)
		{
			int ax = ((Mathf.RoundToInt(corridor.roomA.roomSpace.center.x)) / 2) * 2;
			int ay = ((Mathf.RoundToInt(corridor.roomA.roomSpace.center.y)) / 2) * 2;
			int bx = ((Mathf.RoundToInt(corridor.roomB.roomSpace.center.x)) / 2) * 2;
			int by = ((Mathf.RoundToInt(corridor.roomB.roomSpace.center.y)) / 2) * 2;
			VecInt centerA = new VecInt(ax, ay);
			VecInt centerB = new VecInt(bx, by);

			int halfSizeAX = (int)(corridor.roomA.roomSpace.width / 2 - roomWallDepth);
			int halfSizeAY = (int)(corridor.roomA.roomSpace.height / 2 - roomWallDepth);
			int halfSizeBX = (int)(corridor.roomB.roomSpace.width / 2 - roomWallDepth);
			int halfSizeBY = (int)(corridor.roomB.roomSpace.height / 2 - roomWallDepth);

			float deltaX = corridor.roomB.roomSpace.center.x - corridor.roomA.roomSpace.center.x;
			float deltaY = corridor.roomB.roomSpace.center.y - corridor.roomA.roomSpace.center.y;

			//TODO Arrumar essa parte
			//evitar corredores colados com a sala
			bool xFirst = true;
			//bool fromAToB = true;
			if (Mathf.Abs(deltaX) <= Mathf.Abs(deltaY))
			{
				if (Mathf.Abs(deltaX) > halfSizeAX + 1)
				{
					xFirst = true;
				}
				else if (Mathf.Abs(deltaY) > halfSizeAY + 1)
				{
					xFirst = false;
				}
			}
			else
			{
				if (Mathf.Abs(deltaX) > halfSizeBX + 1)
				{
					xFirst = true;
				}
				else if (Mathf.Abs(deltaY) > halfSizeBY + 1)
				{
					xFirst = false;
				}
			}

			if (xFirst)
			{
				if (centerA.x <= centerB.x)
				{
					for (int x = centerA.x; x <= centerB.x; x++)
					{
						PaintCorridor(corridor, x, centerA.y, false);
					}
				}
				else
				{
					for (int x = centerA.x; x >= centerB.x; x--)
					{
						PaintCorridor(corridor, x, centerA.y, false);
					}
				}

				if (centerA.y <= centerB.y)
				{
					for (int y = centerA.y; y <= centerB.y; y++)
					{
						PaintCorridor(corridor, centerB.x, y, true);
					}
				}
				else
				{
					for (int y = centerA.y; y >= centerB.y; y--)
					{
						PaintCorridor(corridor, centerB.x, y, true);
					}
				}
			}
			else
			{
				if (centerA.y <= centerB.y)
				{
					for (int y = centerA.y; y <= centerB.y; y++)
					{
						PaintCorridor(corridor, centerA.x, y, true);
					}
				}
				else
				{
					for (int y = centerA.y; y >= centerB.y; y--)
					{
						PaintCorridor(corridor, centerA.x, y, true);
					}
				}
				if (centerA.x <= centerB.x)
				{
					for (int x = centerA.x; x <= centerB.x; x++)
					{
						PaintCorridor(corridor, x, centerB.y, false);
					}
				}
				else
				{
					for (int x = centerA.x; x >= centerB.x; x--)
					{
						PaintCorridor(corridor, x, centerB.y, false);
					}
				}
			}
		}

		void PaintCorridor(Corridor corridor, int x, int y, bool addHorizontal)
		{
			SetMapTile(x, y, Tiles.floor);
			corridor.AddTile(new VecInt(x, y));
		}

		bool InBoundaries(VecInt pos)
		{
			return pos.x > 0 && pos.x < mapWidth - 1 && pos.y > 0 && pos.y < mapHeight - 1;
		}

		void FindNeighborRooms()
		{
			for (int i = 0; i < allRooms.Count; i++)
			{
				for (int j = 0; j < allRooms.Count; j++)
				{
					if (i != j) //dont look at the same room
					{
						float distance = Vector2.Distance(allRooms[i].roomSpace.center, allRooms[j].roomSpace.center);
						allRooms[i].roomDistances.Add(new RoomDistance(allRooms[j], distance));
					}
				}
			}
		}

		void PrintMap()
		{
			tileMap.height = mapHeight;
			tileMap.width = mapWidth;
			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{
					int mapTile = GetMapTile(x, y);
					if (mapTile != (int)Tiles.floor || mapTile != (int)Tiles.empty)
					{
						tileMap.SetTile(x, y, wallLayer, mapTile);
					}
					else
					{
						tileMap.SetTile(x, y, floorLayer, mapTile);
					}

					int enemyTile = GetEnemyTile(x, y);
					tileMap.SetTile(x, y, enemyLayer, enemyTile);

					int holeTile = GetHoleTile(x, y);
					tileMap.SetTile(x, y, holeLayer, holeTile);
				}
			}
			tileMap.Build();
		}

		public void SetMapTile(VecInt pos, int type)
		{
			SetMapTile(pos.x, pos.y, type);
		}

		public void SetMapTile(int posX, int posY, int type)
		{
			map[posX, posY] = type;
		}

		public int GetMapTile(VecInt pos)
		{
			return GetMapTile(pos.x, pos.y);
		}

		public int GetMapTile(int posX, int posY)
		{
			return map[posX, posY];
		}

		public int GetEnemyTile(int posX, int posY)
		{
			return enemyMap[posX, posY];
		}

		private void SetEnemyTile(int posX, int posY, int id)
		{
			enemyMap[posX, posY] = id;
		}

		public int GetHoleTile(int posX, int posY)
		{
			return holeMap[posX, posY];
		}

		private void SetHoleTile(int posX, int posY, int id)
		{
			holeMap[posX, posY] = id;
			SetMapTile(posX, posY, (int)Tiles.empty);
		}

	}

	public static class ExtensionMethods
	{
		private static System.Random rng = new System.Random();

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}

	public class Corridor
	{
		public List<VecInt> tiles = new List<VecInt>();
		public Room roomA;
		public Room roomB;
		public bool isConnectedToMainRoom = false;
		public DungeonGenerator dg;
		public int empty;
		public int floor;

		public Corridor(DungeonGenerator dg, Room roomA, Room roomB, int empty, int floor)
		{
			this.dg = dg;
			this.roomA = roomA;
			this.roomB = roomB;
			this.empty = empty;
			this.floor = floor;
		}

		public void Connect()
		{
			roomA.connectedCorridors.Add(this);
			roomB.connectedCorridors.Add(this);
			if (roomA.isConnectedToMainRoom || roomB.isConnectedToMainRoom)
			{
				roomA.ConnectToMainRoom();
				roomB.ConnectToMainRoom();
				isConnectedToMainRoom = true;
			}

			roomA.connectedRooms.Add(roomB);
			roomB.connectedRooms.Add(roomA);
		}

		public void AddTile(VecInt tile)
		{
			tiles.Add(tile);
		}
	}

	public struct RoomDistance : IComparable<RoomDistance>
	{
		public Room room;
		public float distance;

		public RoomDistance(Room otherRoom, float distance) : this()
		{
			this.room = otherRoom;
			this.distance = distance;
		}

		public int CompareTo(RoomDistance other)
		{
			return distance.CompareTo(other.distance);
		}
	}

	public class Room : IComparable<Room>
	{
		private RoomType roomType;// = RoomType.combat;
		public Rect roomSpace;
		public List<Room> connectedRooms = new List<Room>();
		public List<RoomDistance> roomDistances = new List<RoomDistance>();
		public bool isMainRoom = false;
		public bool isConnectedToMainRoom = false;
		public bool hasBeenConnected = false;
		public GameObject roomController;

		public List<Corridor> connectedCorridors = new List<Corridor>();
		public List<VecInt> availableTiles = new List<VecInt>();
		public int roomLevel = -1;

		public Room() { }

		public void SetRoomType(RoomType type)
		{
			Debug.Log("Set Room Type Called: " + type);
			roomType = type;
		}

		public RoomType GetRoomType()
		{
			return roomType;
		}

		public void SetRoomColor(Color roomColor)
		{
			roomController.GetComponent<RoomControl>().minimapSprite.color = roomColor;
		}

		public Room(Rect roomSpace, int wallDepth)
		{
			this.roomSpace = roomSpace;
			for (int y = (int)roomSpace.yMin + wallDepth; y < (int)roomSpace.yMax - wallDepth; y++)
			{
				for (int x = (int)roomSpace.xMin + wallDepth; x < (int)roomSpace.xMax - wallDepth; x++)
				{
					availableTiles.Add(new VecInt(x, y));
				}
			}
			SetRoomType(RoomType.none);
		}

		public void ConnectToMainRoom()
		{
			isConnectedToMainRoom = true;
			foreach (var room in connectedRooms)
			{
				if (!room.isConnectedToMainRoom)
				{
					room.isConnectedToMainRoom = true;
					room.ConnectToMainRoom();
				}
			}
			foreach (var corridor in connectedCorridors)
			{
				if (!corridor.isConnectedToMainRoom)
				{
					corridor.isConnectedToMainRoom = true;
				}
			}
		}

		public void Connect(Room other)
		{
			hasBeenConnected = true;
			other.hasBeenConnected = true;
			if (other.isConnectedToMainRoom)
			{
				isConnectedToMainRoom = true;
				for (int i = 0; i < connectedRooms.Count; i++)
				{
					connectedRooms[i].isConnectedToMainRoom = true;
				}
			}
			connectedRooms.Add(other);
		}

		public float Size()
		{
			return roomSpace.size.x * roomSpace.size.y;
		}

		public bool Contains(int x, int y)
		{
			return roomSpace.Contains(new Vector2(x, y));
		}

		public void AddClosest(Room otherRoom, float distance)
		{
			roomDistances.Add(new RoomDistance(otherRoom, distance));
			roomDistances.Sort();
		}

		public bool IsConnected(Room otherRoom)
		{
			return connectedRooms.Contains(otherRoom);
		}

		public int CompareTo(Room otherRoom)
		{
			return otherRoom.Size().CompareTo(Size());
		}

		public void SpreadType(int type)
		{
			if (roomLevel == 0)
			{
				roomLevel = type;
				foreach (var room in connectedRooms)
				{
					if (room.roomLevel == 0)
					{
						room.SpreadType(roomLevel + 1);
					}
				}
			}
		}
	}

	public enum RoomType
	{
		none,
		start,
		safe,
		combat,
		bit,
	}

	[System.Serializable]
	public struct VecInt
	{
		public int x;
		public int y;

		public VecInt(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public VecInt(Vector2 vector2)
		{
			x = Mathf.RoundToInt(vector2.x);
			y = Mathf.RoundToInt(vector2.y);
		}

		public VecInt Add(VecInt other)
		{
			return new VecInt(x + other.x, y + other.y);
		}

		public bool Equals(VecInt other)
		{
			return x == other.x && y == other.y;
		}

		public static VecInt Vector2ToVecInt(Vector2 vector2)
		{
			return new VecInt(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
		}
	}

	public class Direction
	{
		public static VecInt N = new VecInt(0, 1);
		public static VecInt S = new VecInt(0, -1);
		public static VecInt E = new VecInt(1, 0);
		public static VecInt W = new VecInt(-1, 0);
		public static VecInt NE = new VecInt(1, 1);
		public static VecInt SE = new VecInt(1, -1);
		public static VecInt NW = new VecInt(-1, 1);
		public static VecInt SW = new VecInt(-1, -1);
		public static VecInt[] all = { N, S, E, W, NE, SE, NW, SW };
		public static VecInt[] diagonal = { NE, SE, NW, SW };
		public static VecInt[] orthogonal = { N, S, E, W };
	}

	[System.Serializable]
	public class Tiles
	{
		public static int
			empty = -1,
			filled = -2,

			floor = 5,

			wallTop = 9,
			wallRight = 4,
			wallBottom = 1,
			wallLeft = 6,

			wallTopRightConcave = 13,
			wallBottomRightConcave = 17,
			wallBottomLeftConcave = 16,
			wallTopLeftConcave = 12,

			wallTopRightConvex = 2,
			wallBottomRightConvex = 10,
			wallBottomLeftConvex = 8,
			wallTopLeftConvex = 0,

			holeCenter = 25,

			holeTop = 21,
			holeRight = 26,
			holeBottom = 57,
			holeLeft = 24,

			holeTopRight = 22,
			holeBottomRight = 58,
			holeBottomLeft = 56,
			holeTopLeft = 20,

			nim = 3,

			simpleApple = 28,
			simplePineapple = 29,
			simpleWatermelon = 30,
			simpleBanana = 31,

			superApple = 32,
			superPineapple = 33,
			superWatermelon = 34,
			superBanana = 35;

		public static int[] normalEnemies = { simpleApple, simpleBanana, simplePineapple, simpleWatermelon };
		public static int[] superEnemies = { superApple, superBanana, superPineapple, superWatermelon };
	}
}
