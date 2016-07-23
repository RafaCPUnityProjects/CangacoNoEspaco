using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class JukeboxPlayer : MonoBehaviour
{

    public static JukeboxPlayer control;

    #region [ Preperties ]
    // Reference to the objects that are created to add the AudioSources
    public List<GameObject> PlayerObjects = new List<GameObject>();

    // The list that keep the amount of AudioClipLibrary
    public List<AudioClipLibrary> audioClipLibrary = new List<AudioClipLibrary>();
    #endregion

    // Create a dictionary using the the PlayerSources list
    Dictionary<string, AudioSource> playerSourceDictionary = new Dictionary<string, AudioSource>();
    Dictionary<string, float> playerVolume = new Dictionary<string, float>();
    Dictionary<GameObject, AudioSource> customPlayerSourceDictionary = new Dictionary<GameObject, AudioSource>();
    Dictionary<GameObject, bool> customPlayerStartSmoothly = new Dictionary<GameObject, bool>();
    Dictionary<GameObject, bool> customPlayerStopSmoothly = new Dictionary<GameObject, bool>();
    Dictionary<GameObject, float> customPlayerStartTime = new Dictionary<GameObject, float>();
    Dictionary<GameObject, float> customPlayerStopTime = new Dictionary<GameObject, float>();
    List<GameObject> customPlayerObjs = new List<GameObject>();
    List<GameObject> customPlayer_Source = new List<GameObject>();
    // Create a dictionary searching for each clip inside the AudioClipLibrary
    Dictionary<string, AudioClip> AudioClipDictionary = new Dictionary<string, AudioClip>();


    // Used to keep the current player that is playing smoothly or pausing smoothly
    string playerName;

    // ----- Play ------
    AudioSource source;

    // ----- New -----
    Dictionary<string, AudioSource> customAudio = new Dictionary<string, AudioSource>();
    List<AudioSource> customAudioList = new List<AudioSource>();

    void Awake()
    {
        if (!control)
        {
            control = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (control != this)
            Destroy(gameObject);

        // Setup the audio sources and their properties
        for (int i = 0; i < PlayerObjects.Count; i++)
        {
            playerSourceDictionary.Add(PlayerObjects[i].name, PlayerObjects[i].GetComponent<AudioSource>());
            playerVolume.Add(PlayerObjects[i].name, PlayerObjects[i].GetComponent<AudioSource>().volume);
        }

        // Load the libraries and create a unique list with all of them
        for (int i = 0; i < audioClipLibrary.Count; i++)
        {
            for (int y = 0; y < audioClipLibrary[i].LibraryClips.Count; y++)
            {
                if (audioClipLibrary[i].LibraryClips[y])
                    AudioClipDictionary.Add(audioClipLibrary[i].LibraryClips[y].name, audioClipLibrary[i].LibraryClips[y]);
            }
        }
    }


    void Update()
    {
        if (customPlayerObjs.Count > 0)
        {
            for (int i = 0; i < customPlayerObjs.Count; i++)
            {

                if (customPlayerStartSmoothly[customPlayerObjs[i]])
                    PlaySmoothly(customPlayerObjs[i]);

                if (customPlayerStopSmoothly[customPlayerObjs[i]])
                    StopSmoothly(customPlayerObjs[i]);
            }
        }

        //Destroi os audios que nao estao mais tocando
        for (int i = 0; i < customAudioList.Count; i++)
        {				
			if (customAudioList[i] != null && !customAudioList[i].isPlaying)
            {
				GameObject temp = customAudioList[i].gameObject;
				customAudioList.Remove(customAudioList[i]);
				Destroy(temp);
            }
        }
    }

    void PlaySmoothly(GameObject gameObj)
    {
        customPlayerSourceDictionary[gameObj].volume = Mathf.MoveTowards(customPlayerSourceDictionary[gameObj].volume, 1, Time.deltaTime / customPlayerStartTime[gameObj]);
        if (customPlayerSourceDictionary[gameObj].volume >= 1)
        {
            customPlayerStartSmoothly[gameObj] = false;
            customPlayerStartTime[gameObj] = 0;
        }
    }

    void StopSmoothly(GameObject gameObj)
    {
        customPlayerSourceDictionary[gameObj].volume = Mathf.MoveTowards(customPlayerSourceDictionary[gameObj].volume, 0, Time.deltaTime / customPlayerStopTime[gameObj]);
        if (customPlayerSourceDictionary[gameObj].volume <= 0)
        {
            customPlayerStopSmoothly[gameObj] = false;
            customPlayerStopTime[gameObj] = 0;
            customPlayerSourceDictionary[gameObj].Stop();
			customPlayerSourceDictionary[gameObj].loop = false;
        }
    }

    #region [ PLAY ]	
    public void Play(string playerName, string clipName, bool playTogether, bool createObj)
    {
        if (!playerSourceDictionary[playerName].mute)
        {
            customAudio.Add(clipName, new GameObject(clipName).AddComponent<AudioSource>());
            customAudio[clipName].volume = playerVolume[playerName];
            customAudio[clipName].clip = AudioClipDictionary[clipName];
            customAudio[clipName].Play();
        }
    }

	//Updated
    public void Play(string playerName, string clipName, bool playTogether)
    {
        if (!playerSourceDictionary[playerName].mute)
        {
            if (playTogether)
            {
                if (!playerSourceDictionary[playerName].isPlaying)
                {
                    playerSourceDictionary[playerName].clip = AudioClipDictionary[clipName];

                    //rnd pitch
                    
                    //playerSourceDictionary[playerName].pitch = 1 + Random.value;
					playerSourceDictionary[playerName].Play();
                }
                else
                {
                    AudioSource obj = new GameObject(clipName).AddComponent<AudioSource>();

                    //rnd pitch
                    
					//obj.GetComponent<AudioSource> ().pitch = 1 + Random.value;

					customAudioList.Add(obj);
                    obj.spatialBlend = 0;
                    obj.clip = AudioClipDictionary[clipName];
                    obj.volume = playerVolume[playerName];
                    obj.Play();
                }

            }
            else
            {
                playerSourceDictionary[playerName].Stop();
                playerSourceDictionary[playerName].clip = AudioClipDictionary[clipName];

				//rnd pitch
				//playerSourceDictionary[playerName].pitch = 1 + Random.value;

                playerSourceDictionary[playerName].Play();
            }
        }
    }

    public void Play(string playerName, AudioClip audioClip, bool playTogether)
    {
		if (!playerSourceDictionary[playerName].mute)
		{
			if (playTogether)
			{
				if (!playerSourceDictionary[playerName].isPlaying)
				{
					playerSourceDictionary[playerName].clip = audioClip;
					playerSourceDictionary[playerName].Play();
				}
				else
				{
					AudioSource obj = new GameObject(audioClip.name).AddComponent<AudioSource>();
					customAudioList.Add(obj);
					obj.spatialBlend = 0;
					obj.clip = audioClip;
					obj.volume = playerVolume[playerName];
					obj.Play();
				}

			}
			else
			{
				playerSourceDictionary[playerName].Stop();
				playerSourceDictionary[playerName].clip = audioClip;
				playerSourceDictionary[playerName].Play();
			}
		}
    }

	//Updated
	public void Play(string playerName, string clipName, bool loop, float smoothTime, GameObject gameObj, bool variablePitch = true)
    {
        if (!playerSourceDictionary[playerName].mute)
        {
            this.playerName = playerName;

            source = GetCustomAudioSource(gameObj);

            if (customPlayerStopSmoothly[gameObj])
            {
                customPlayerStopSmoothly[gameObj] = false;
                customPlayerStopTime[gameObj] = 0;
            }

            source.clip = AudioClipDictionary[clipName];
            source.loop = loop;

			//rnd pitch
			if (variablePitch) {
				//source.pitch = 1 + Random.value;
				source.spatialize = true;
				source.spatialBlend = 0.8f;
				source.transform.position = gameObj.transform.position;
			}

            customPlayerStartSmoothly[gameObj] = true;
            customPlayerStartTime[gameObj] = smoothTime;

            source.volume = 0;
            this.playerName = playerName;

            source.Play();
        }
    }

	public void Play(string playerName, AudioClip audioClip, bool loop, float smoothTime, GameObject gameObj)
	{
		if (!playerSourceDictionary[playerName].mute)
		{
			this.playerName = playerName;

			source = GetCustomAudioSource(gameObj);

			if (customPlayerStopSmoothly[gameObj])
			{
				customPlayerStopSmoothly[gameObj] = false;
				customPlayerStopTime[gameObj] = 0;
			}

			source.clip = audioClip;
			source.loop = loop;

			customPlayerStartSmoothly[gameObj] = true;
			customPlayerStartTime[gameObj] = smoothTime;

			source.volume = 0;
			this.playerName = playerName;

			source.Play();
		}
	}
    #endregion

    #region [ Stop ]
    public void Stop(string playerName)
    {
        playerSourceDictionary[playerName].Stop();
    }

    public void Stop(string playerName, float smoothTime, GameObject gameObj)
    {
		if(!playerSourceDictionary.ContainsKey(playerName) || !customPlayerStartSmoothly.ContainsKey(gameObj))
			return;

		if (!playerSourceDictionary[playerName].mute)
        {
            this.playerName = playerName;

            if (customPlayerStartSmoothly[gameObj])
            {
                customPlayerStartSmoothly[gameObj] = false;
                customPlayerStartTime[gameObj] = 0;
            }

            source = customPlayerSourceDictionary[gameObj];

            customPlayerStopSmoothly[gameObj] = true;
            customPlayerStopTime[gameObj] = smoothTime;
        }
    }

    public void Stop(string playerName, string clipName)
    {
        GameObject temp = customAudio[clipName].gameObject;
        customAudio.Remove(clipName);
        Destroy(temp);
    }
    #endregion

    public void PauseAudio(string PlayerName) { playerSourceDictionary[PlayerName].Pause(); }

    public void DeleteAditionalPlayers()
    {
        if (customPlayer_Source.Count > 0)
        {
            for (int i = 0; i < customPlayer_Source.Count; i++)
            {
                Destroy(customPlayer_Source[i]);
            }
            customPlayer_Source.Clear();
            customPlayerObjs.Clear();
            customPlayerSourceDictionary.Clear();
            customPlayerStartSmoothly.Clear();
            customPlayerStartTime.Clear();
            customPlayerStopSmoothly.Clear();
            customPlayerStopTime.Clear();
        }
    }

    #region [ SETTERS ]
    public void SetAudioTime(string playerName, float time) { playerSourceDictionary[playerName].time = time; }
    public void SetPlayerMute(string playerName, bool mute)
    {
        //if (mute)
        //    playerSourceDictionary[playerName].Stop();
        playerSourceDictionary[playerName].mute = mute;
        if (playerSourceDictionary[playerName].gameObject.transform.childCount > 0)
        {
            for (int i = 0; i < customPlayerObjs.Count; i++)
            {
                //if (mute)
                //    customPlayerSourceDictionary[customPlayerObjs[i]].Stop();
                customPlayerSourceDictionary[customPlayerObjs[i]].mute = mute;
            }
        }
        foreach (string s in customAudio.Keys)
        {
            customAudio[s].mute = mute;
        }
    }
    #endregion

    #region [ GETTERS ]
    public bool GetPlayerIsPlaying(string playerName) { return playerSourceDictionary[playerName].isPlaying; }
    public float GetPlayerTime(string playerName) { return playerSourceDictionary[playerName].time; }
    public bool GetPlayerMute(string playerName) { return playerSourceDictionary[playerName].mute; }
    AudioSource GetCustomAudioSource(GameObject gameObj)
    {
        bool create = false;

        if (customPlayerSourceDictionary.Count <= 0 || !customPlayerSourceDictionary.ContainsKey(gameObj))
            create = true;

        if (create)
        {
            GameObject tempCustomAudioSource = new GameObject();
            tempCustomAudioSource.AddComponent<AudioSource>();
            tempCustomAudioSource.GetComponent<AudioSource>().playOnAwake = false;
            tempCustomAudioSource.name = "Custom Audio Source " + playerSourceDictionary[playerName].gameObject.transform.childCount;
            tempCustomAudioSource.transform.parent = playerSourceDictionary[playerName].gameObject.transform;

            customPlayer_Source.Add(tempCustomAudioSource);
            customPlayerObjs.Add(gameObj);
            customPlayerStartSmoothly[gameObj] = false;
            customPlayerStopSmoothly[gameObj] = false;
            customPlayerSourceDictionary.Add(gameObj, tempCustomAudioSource.GetComponent<AudioSource>());
        }

        return customPlayerSourceDictionary[gameObj];
    }
    #endregion


    [System.Serializable]
    public class PlayerAudioSources
    {
        public List<AudioSource> PlayerAudioSource;
    }

    [System.Serializable]
    public class AudioClipLibrary
    {
        public string LibraryName;
        public List<AudioClip> LibraryClips;
    }
}