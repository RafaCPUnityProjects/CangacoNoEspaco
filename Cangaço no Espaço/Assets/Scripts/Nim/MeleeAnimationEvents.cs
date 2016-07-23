using UnityEngine;
using System.Collections;

public class MeleeAnimationEvents : MonoBehaviour {

    // Called by an Animation Event
    public void HideMe(float theValue)
    {
        this.gameObject.SetActive(false);
    }
}
