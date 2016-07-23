using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CheatCodes : MonoBehaviour {

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
#endif

}
