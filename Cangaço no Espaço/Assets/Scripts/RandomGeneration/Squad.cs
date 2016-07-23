using System;

[System.Serializable]
public class Squad : IComparable<Squad>
{
	[System.Serializable]
	public struct Row
	{
		public Enemy[] row;
	}
	public Row[] collum;
	public int width;
	public int height;

	public Squad(int sizeX, int sizeY)
	{
		width = sizeX;
		height = sizeY;
		collum = new Row[sizeY];
		for (int y = 0; y < sizeY; y++)
		{
			collum[y].row = new Enemy[sizeX];
			for (int x = 0; x < sizeX; x++)
			{
				collum[y].row[x] = Enemy.none;
			}
		}
	}

	public int CompareTo(Squad other)
	{
		return width.CompareTo(other.width);
	}
}

[System.Serializable]
public enum Enemy
{
	none = -1,
	redApple = 28,
	pineApple = 29,
	waterMelon = 30,
	banana = 31,

	topLeftHole = 56,
	topHole = 57,
	topRightHole = 58,
	leftHole = 24,
	centerHole = 25,
	rightHole = 26,
	bottomLeftHole = 20,
	bottomHole = 21,
	bottomRightHole = 22,
}