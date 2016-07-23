using UnityEngine;
using System.Collections;

public class LevelTextSelector : MonoBehaviour {

    public GameObject textStage;
    public GameObject textBoss;

	void OnEnable () {
        textBoss.SetActive(GameSave.instance.isBossStage());
        textStage.SetActive(!GameSave.instance.isBossStage());
    }
	
}
