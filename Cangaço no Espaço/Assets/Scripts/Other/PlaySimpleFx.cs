//--------------------------------------------------------------------
//
using System;
using UnityEngine;

class PlaySimpleFx : MonoBehaviour
{
	[FMODUnity.EventRef]
	public string PlayerStateEvent;

	FMOD.Studio.EventInstance playerState;

	[FMODUnity.EventRef]
	public string DamageEvent;
	[FMODUnity.EventRef]
	public string HealEvent;

	[FMODUnity.EventRef]
	public string PlayerIntroEvent;
	FMOD.Studio.EventInstance playerIntro;


	public int StartingHealth = 100;
	int health;

	Rigidbody cachedRigidBody;

	void Start()
	{
		cachedRigidBody = GetComponent<Rigidbody>();
		health = StartingHealth;

		playerState = FMODUnity.RuntimeManager.CreateInstance(PlayerStateEvent);
		//playerState.start();

		playerIntro = FMODUnity.RuntimeManager.CreateInstance(PlayerIntroEvent);
		//playerIntro.start();
	}

	void OnDestroy()
	{
		StopAllPlayerEvents();

		//playerState.release();
	}

	void SpawnIntoWorld()
	{
		health = StartingHealth;

		//playerState.start();
	}

	void Update()
	{
		//playerState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, cachedRigidBody));

		//playerState.setParameterValue("health", (float)health);

		/*if (playerIntro != null)
		{
			FMOD.Studio.PLAYBACK_STATE playbackState;
			playerIntro.getPlaybackState(out playbackState);
			if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
			{
				playerIntro.release();
				playerIntro = null;
				SendMessage("PlayerIntroFinished");
			}
		}*/

		if (Input.GetButtonDown ("Fire1")) {
			TakeDamage ();
		}
	}

	void TakeDamage()
	{
		health -= 1;

		FMODUnity.RuntimeManager.PlayOneShot(DamageEvent, transform.position);

		if (health == 0)
		{
			playerState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}


	void ReceiveHealth(bool restoreAll)
	{
		if (restoreAll)
		{
			health = StartingHealth;
		}
		else
		{
			health = Math.Min(health + 3, StartingHealth);
		}        

		FMOD.Studio.EventInstance heal = FMODUnity.RuntimeManager.CreateInstance(HealEvent);
		heal.setParameterValue("FullHealth", restoreAll ? 1.0f : 0.0f);
		heal.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
		heal.start();
		heal.release();
	}

	void StopAllPlayerEvents()
	{
		FMOD.Studio.Bus playerBus = FMODUnity.RuntimeManager.GetBus("bus:/player");
		playerBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}
}