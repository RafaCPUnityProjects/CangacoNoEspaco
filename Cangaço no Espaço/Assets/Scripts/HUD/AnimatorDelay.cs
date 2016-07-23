using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class AnimatorDelay : MonoBehaviour {

    public float logoDelay = 2f;
    public float delay;
    Text myText;

	void Start () {

        myText = GetComponent<Text>();
        if (myText != null)
            myText.enabled = false;

        Invoke("EnableAnimator", logoDelay + delay);
	}

    void EnableAnimator()
    {
        if(myText != null)
            myText.enabled = true;

        GetComponent<Animator>().enabled = true;
    }
	
}
