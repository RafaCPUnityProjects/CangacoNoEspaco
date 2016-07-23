using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Retroboy;
using System;

public class SquadCustomWindow : EditorWindow
{
	private const int minSquadSize = 4;
	private const int maxSquadSize = 10;
	public SquadList mySquadList;
	private int viewIndex = 1;
	private int size = 4;

	[MenuItem("Window/Squad List Editor %#e")]
	static void Init()
	{
		EditorWindow.GetWindow(typeof(SquadCustomWindow));
	}

	void OnEnable()
	{
		OpenSquadList();
	}

	void OnGUI()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Squad List Editor", EditorStyles.boldLabel);
		if (mySquadList != null)
		{
			if (GUILayout.Button("Show Squad List", GUILayout.ExpandWidth(false)))
			{
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = mySquadList;
			}
		}

		GUILayout.EndHorizontal();

		if (mySquadList == null)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(10);
			if (GUILayout.Button("Create New Squad List", GUILayout.ExpandWidth(false)))
			{
				CreateNewSquadList();
			}
			if (GUILayout.Button("Open Existing Squad List", GUILayout.ExpandWidth(false)))
			{
				OpenSquadList();
			}
			GUILayout.EndHorizontal();
		}

		GUILayout.Space(20);

		if (mySquadList != null)
		{
			GUILayout.BeginHorizontal();

			GUILayout.Space(10);

			if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
			{
				if (viewIndex > 1)
					viewIndex--;
			}
			GUILayout.Space(5);
			if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
			{
				if (viewIndex < mySquadList.squadList.Count)
				{
					viewIndex++;
				}
			}

			GUILayout.Space(60);
			size = (int)EditorGUILayout.IntField("Size:", size);//, minSquadSize, maxSquadSize);
			size = Mathf.Clamp(size, 3, 9);
			if(size % 2 != 1)
			{
				size -= 1;
			}

			GUILayout.Space(10);

			if (GUILayout.Button("Add Squad", GUILayout.ExpandWidth(false)))
			{
				AddSquad();
			}
			if (GUILayout.Button("Sort Squad", GUILayout.ExpandWidth(false)))
			{
				SortSquad();
			}
			if (GUILayout.Button("Delete Squad", GUILayout.ExpandWidth(false)))
			{
				DeleteSquad(viewIndex - 1);
			}
			GUILayout.EndHorizontal();
			if (mySquadList.squadList == null)
				Debug.Log("wtf");
			if (mySquadList.squadList.Count > 0)
			{
				GUILayout.BeginHorizontal();
				viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Squad", viewIndex, GUILayout.ExpandWidth(false)), 1, mySquadList.squadList.Count);
				EditorGUILayout.LabelField("of   " + mySquadList.squadList.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
				EditorGUILayout.LabelField("size: " + mySquadList.squadList[viewIndex-1].collum.Length.ToString(), "", GUILayout.ExpandWidth(false));
				GUILayout.EndHorizontal();
				if (mySquadList.squadList[viewIndex - 1] != null && mySquadList.squadList[viewIndex - 1].width > 0 && mySquadList.squadList[viewIndex - 1].height > 0)
				{
					for (int y = 0; y < mySquadList.squadList[viewIndex - 1].height; y++)
					{
						EditorGUILayout.BeginHorizontal();
						for (int x = 0; x < mySquadList.squadList[viewIndex - 1].width; x++)
						{
							mySquadList.squadList[viewIndex - 1].collum[y].row[x] = (Enemy)EditorGUILayout.EnumPopup(mySquadList.squadList[viewIndex - 1].collum[y].row[x]);
						}
						EditorGUILayout.EndHorizontal();
					}
				}
				GUILayout.Space(10);
			}
			else
			{
				GUILayout.Label("This Inventory List is Empty.");
			}
		}
		if (GUI.changed)
		{
			EditorUtility.SetDirty(mySquadList);
		}
	}

	private void SortSquad()
	{
		mySquadList.squadList.Sort();
	}

	void CreateNewSquadList()
	{
		viewIndex = 1;
		mySquadList = CreateSquadList.Create();
		if (mySquadList)
		{
			mySquadList.squadList = new List<Squad>();
		}
	}

	void OpenSquadList()
	{
		mySquadList = AssetDatabase.LoadAssetAtPath("Assets/Resources/SquadList.asset", typeof(SquadList)) as SquadList;

		if(mySquadList == null)
		{
			CreateNewSquadList();
		}
	}

	void AddSquad()
	{
		Squad newItem = new Squad(size, size);
		mySquadList.squadList.Add(newItem);
		viewIndex = mySquadList.squadList.Count;
	}

	void DeleteSquad(int index)
	{
		mySquadList.squadList.RemoveAt(index);
	}
}

public class CreateSquadList
{
	public static SquadList Create()
	{
		SquadList asset = ScriptableObject.CreateInstance<SquadList>();

		AssetDatabase.CreateAsset(asset, "Assets/Resources/SquadList.asset");
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		return asset;
	}
}
