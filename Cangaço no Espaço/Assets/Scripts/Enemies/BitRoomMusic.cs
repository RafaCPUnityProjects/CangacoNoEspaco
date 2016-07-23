using UnityEngine;
using System.Collections;

public class BitRoomMusic : MonoBehaviour {

	void OnEnable () {
        if(RetroJukebox.control != null)
            RetroJukebox.control.BitRoom(true);
    }

    void OnDisable()
    {
        if (RetroJukebox.control != null)
            RetroJukebox.control.BitRoom(false);
    }

    void OnDestroy () {
        if (RetroJukebox.control != null)
            RetroJukebox.control.BitRoom(false);
    }
}
