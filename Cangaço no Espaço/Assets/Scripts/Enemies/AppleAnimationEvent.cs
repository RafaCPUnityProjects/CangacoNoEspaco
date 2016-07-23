using UnityEngine;
using System.Collections;

public class AppleAnimationEvent : MonoBehaviour {

    FollowTargetTiled follow;

    void Start()
    {
        follow = transform.parent.gameObject.GetComponent<FollowTargetTiled>();
    }

    //Animation event
    public void Jump()
    {
        if(follow != null)
            follow.timeToJump = true;
    }
}
