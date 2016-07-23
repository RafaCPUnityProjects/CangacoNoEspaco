using UnityEngine;
using System.Collections;

public class NimBlackScreen : MonoBehaviour {


    void Update () {

        if (GameSave.instance.gameStarted)
        {
            //gameObject.SetActive(false);
            Destroy(this.gameObject);
        }

	}
	
}
