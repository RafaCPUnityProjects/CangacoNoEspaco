using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomSubtitle : MonoBehaviour {
    
    Text subtitle;
    public string[] subtitles;

	void Start () {

        subtitle = GetComponent<Text>();

        if (subtitles != null)
        {
            subtitle.text = subtitles[Random.Range(0, subtitles.Length - 1)];
        }
        
    }
	

}
