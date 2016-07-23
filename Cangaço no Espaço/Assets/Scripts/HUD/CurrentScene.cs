using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrentScene : MonoBehaviour {
    
    void OnEnable () {
        GetComponent<Text>().text = GameSave.instance.CurrentSceneNumber().ToString();
	}
	
}
