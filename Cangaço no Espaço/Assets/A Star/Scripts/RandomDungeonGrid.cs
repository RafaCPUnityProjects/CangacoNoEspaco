using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Retroboy;

public class RandomDungeonGrid : MonoBehaviour
{
	public DungeonGenerator dg;
	public int[] walkableTiles;
	public bool includeDiagonals = false;
	[HideInInspector]
	public bool gridReady = false;
	Node[,] grid;

	VecInt tilesPerMapTile = new VecInt(2, 2);
	float tileSize = 30f / 70f / 2f;
	int gridSizeX, gridSizeY;

	void Awake()
	{
		if (walkableTiles.Length <= 0)
		{
			Debug.LogError("At least one walkable tile required");
			return;
		}

		StartCoroutine(WaitForMapReady());
	}

	IEnumerator WaitForMapReady()
	{
		while (!dg.mapPrinted)
		{
			yield return null;
		}
		print("Map ready");

		print("tilesize: " + tileSize);

		gridSizeX = dg.mapWidth * tilesPerMapTile.x;
		gridSizeY = dg.mapHeight * tilesPerMapTile.y;

		CreateGrid();
		gridReady = true;
		print("grid ready");
	}

	public int MaxGridSize
	{
		get
		{
			return gridSizeX * gridSizeY;
		}
	}


	private void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - (Vector3.right + Vector3.up) * (tileSize / 4*3);
		for (int x = 0; x < gridSizeX; x += tilesPerMapTile.x)
		{
			for (int y = 0; y < gridSizeY; y += tilesPerMapTile.y)
			{
				Vector3 worldPoint00 = worldBottomLeft + Vector3.right * (x * tileSize) + Vector3.up * (y * tileSize);
				Vector3 worldPoint10 = worldBottomLeft + Vector3.right * ((x + 1) * tileSize) + Vector3.up * (y * tileSize);
				Vector3 worldPoint01 = worldBottomLeft + Vector3.right * (x * tileSize) + Vector3.up * ((y + 1) * tileSize);
				Vector3 worldPoint11 = worldBottomLeft + Vector3.right * ((x + 1) * tileSize) + Vector3.up * ((y + 1) * tileSize);

				bool walkable = false;
				for (int i = 0; i < walkableTiles.Length; i++)
				{
					if (dg.GetMapTile(x / tilesPerMapTile.x, y / tilesPerMapTile.y) == (int)walkableTiles[i])
					{
						walkable = true;
						break;
					}
				}

				grid[x, y] = new Node(walkable, worldPoint00, x, y);
				grid[x + 1, y] = new Node(walkable, worldPoint10, x + 1, y);
				grid[x, y + 1] = new Node(walkable, worldPoint01, x, y + 1);
				grid[x + 1, y + 1] = new Node(walkable, worldPoint11, x + 1, y + 1);
			}
		}
	}

	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
				{
					continue;
				}

				if (!includeDiagonals)
				{
					if (x == y || x == -y || y == -x)
					{
						continue;
					}
				}

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
				{
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}
		return neighbours;
	}

	public Node WorldPoint2Node(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x - (tileSize / 4 * 3)) / ((gridSizeX) * tileSize);
		float percentY = (worldPosition.y - (tileSize / 4 * 3)) / ((gridSizeY) * tileSize);
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		print("WorldPoint2Node "+ worldPosition + " = " + x + "," + y);
		return grid[x, y];
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX * tileSize, gridSizeY * tileSize, 2));

		if (grid != null)
		{
			foreach (Node n in grid)
			{
				Gizmos.color = n.walkable ? Color.white : Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (tileSize - 0.1f));
			}
		}
	}
#endif

	//Used to define penalties for multiple types of terrain
	[System.Serializable]
	public class TerrainType
	{
		public LayerMask terrainMask;
		public int terrainPenalty;
	}
}
