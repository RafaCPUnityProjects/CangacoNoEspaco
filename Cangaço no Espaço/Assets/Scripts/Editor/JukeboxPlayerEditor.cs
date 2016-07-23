using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(JukeboxPlayer))]
public class JukeboxPlayerEditor : Editor {

	JukeboxPlayer jukeboxPlayer;
	StyleEditor styleEditor;

	SerializedProperty property;
	Rect rect;

	public override void OnInspectorGUI()
	{
		jukeboxPlayer = (JukeboxPlayer)target;
		styleEditor = new StyleEditor();

		serializedObject.Update();

		DisplayPlayerObjectList();
		JukeboxButtons();
		GUILayout.Space(10);
		DisplayLibrary();
		LibraryButtons();
	}

	void DisplayPlayerObjectList()
	{

		rect = EditorGUILayout.BeginVertical();
		GUI.Box(rect, GUIContent.none);

		EditorGUILayout.LabelField("Players Created", styleEditor.TitleStyle());

		EditorGUILayout.Separator();
		for (int i = 0; i < jukeboxPlayer.PlayerObjects.Count; i++) {
			if(jukeboxPlayer.PlayerObjects[i] != null)
				EditorGUILayout.LabelField(i + " - " + jukeboxPlayer.PlayerObjects[i].name);	
			else
				EditorGUILayout.LabelField("Null");
		}

		EditorGUILayout.EndVertical();
	}

	void JukeboxButtons()
	{
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Create New Player"))
		{
			GameObject tempPlayerObj = new GameObject();
			tempPlayerObj.transform.position = Vector3.zero;
			tempPlayerObj.transform.parent = jukeboxPlayer.transform;
			tempPlayerObj.AddComponent<AudioSource>();
			tempPlayerObj.GetComponent<AudioSource>().playOnAwake = false;	

			jukeboxPlayer.PlayerObjects.Add(tempPlayerObj);
		}

		if(GUILayout.Button("Refresh Players"))
		{
			int playerObjectsSize = jukeboxPlayer.PlayerObjects.Count;
			for (int i = 0; i < playerObjectsSize; i++) {
				for (int x = 0; x < jukeboxPlayer.PlayerObjects.Count; x++) {
					if(jukeboxPlayer.PlayerObjects[x] == null)
						jukeboxPlayer.PlayerObjects.RemoveAt(x);
				}
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	void DisplayLibrary()
	{
		rect = EditorGUILayout.BeginVertical();
		GUI.Box(rect, GUIContent.none);

		EditorGUILayout.LabelField("Audio Clip Library", styleEditor.TitleStyle());

		for (int i = 0; i < jukeboxPlayer.audioClipLibrary.Count; i++) {
			GUILayout.Space(5);
			EditorGUILayout.BeginHorizontal();

			property = serializedObject.FindProperty("audioClipLibrary");
			SerializedProperty audioClipProperty = property.GetArrayElementAtIndex(i);
			EditorGUI.BeginChangeCheck();
			EditorGUI.indentLevel = 1;

			EditorGUILayout.PropertyField(audioClipProperty, true);
			EditorGUI.indentLevel = 0;
			if(EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();

			if(GUILayout.Button("X"))
				jukeboxPlayer.audioClipLibrary.RemoveAt(i);

			EditorGUILayout.EndHorizontal();
			GUILayout.Space(5);
		}

		EditorGUILayout.EndVertical();
	}
	
	void LibraryButtons()
	{
		if(GUILayout.Button("Create New Library"))
		{
			jukeboxPlayer.audioClipLibrary.Add(new JukeboxPlayer.AudioClipLibrary());
		}
	}
}
