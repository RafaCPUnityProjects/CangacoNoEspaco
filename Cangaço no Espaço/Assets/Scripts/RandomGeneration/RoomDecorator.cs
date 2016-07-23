//using UnityEngine;
//using System.Collections;

//namespace Retroboy
//{
//	[RequireComponent(typeof(DungeonGenerator))]
//	public class RoomDecorator : MonoBehaviour
//	{
//		public RoomDecoration[] bossDecorationPatterns;
//		public RoomDecoration[] decorationPatterns;
//		public int numberOfBits;
//		public GameObject bitPrefab;
//		private DungeonGenerator dg;
//		private Room boosRoom;
//		public void DecorateRooms(ref DungeonGenerator dg)
//		{
//			this.dg = dg;
//			dg.allRooms.Sort();
//			Room bossRoom = dg.allRooms[0];
//			for (int i = 0; i < numberOfBits; i++)
//			{
//				//add bit at dg.allRooms[i+1];
//			}
//			for (int i = 1; i < dg.allRooms.Count-1; i++)
//			{
//				//decorate room dg.allRooms
//			}
//		}
//	}

//	[System.Serializable]
//	public class RoomDecoration
//	{
//		public GameObject[] monsterPrefabs;
//		public VecInt numberOfMonstersRange;
//		public DistributionPattern monsterDistributionPattern;

//		public GameObject[] obstacles;
//		public VecInt numberOfObstaclesRange;
//		public DistributionPattern obstacleDistributionPattern;

//		public enum DistributionPattern
//		{
//			spaced,
//			circular,
//			singleOneInCenter,
//		}
//	}
//}
