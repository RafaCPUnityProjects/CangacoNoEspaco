using UnityEngine;
using System.Collections;
using FMOD;

public class RetroJukebox : MonoBehaviour {
    
    public static RetroJukebox control;

    [FMODUnity.EventRef]
    public string Music;
    [FMODUnity.EventRef]
    public string NimMelee;
    [FMODUnity.EventRef]
    public string NimShoot;
    [FMODUnity.EventRef]
    public string PineappleExplosion;
    [FMODUnity.EventRef]
    public string NimGranade;
    [FMODUnity.EventRef]
    public string PickWatermelon;
    [FMODUnity.EventRef]
    public string PickPineapple;
    [FMODUnity.EventRef]
    public string PickApple;
    [FMODUnity.EventRef]
    public string NimHurt;
    [FMODUnity.EventRef]
    public string PickBit;
    [FMODUnity.EventRef]
    public string PickLife;
    [FMODUnity.EventRef]
    public string EnemyDeath;

    FMOD.Studio.EventInstance music;
    FMOD.Studio.EventInstance pickBit;

    //save explosion position
    Vector3 explosionPosition;

    bool isBitRoom;


    void Awake()
    {
        RetroJukebox[] myInstances = FindObjectsOfType<RetroJukebox>();
        if (myInstances.Length > 1)
            Destroy(this.gameObject);

        if (control == null)
            control = this;

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance(Music);
        music.start();

        pickBit = FMODUnity.RuntimeManager.CreateInstance(PickBit);
  
    }

    void OnDestroy()
    {
        if(music != null)
            music.release();
        if (pickBit != null)
            pickBit.release();

    }
    
    public void BitRoom(bool isActive)
    {
        float parameter;

        if (isActive)
            parameter = 1f;
        else
            parameter = 0f;

		if(music != null)
			music.setParameterValue("sala_bit", parameter);
    }

    void Update()
    {
        //Test bit room music
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    isBitRoom = !isBitRoom;
        //    BitRoom(isBitRoom);
        //}
            
    }

    public void PlayOneShot(string effectName, Vector3 position)
    {
        switch (effectName)
        {
            case "NimMelee":
                FMODUnity.RuntimeManager.PlayOneShot(NimMelee, position);
                break;
            case "NimShoot":
                FMODUnity.RuntimeManager.PlayOneShot(NimShoot, position);
                break;
            case "PineappleExplosion":
                FMODUnity.RuntimeManager.PlayOneShot(PineappleExplosion, position);
                break;
            case "NimGranade":
                FMODUnity.RuntimeManager.PlayOneShot(NimGranade, position);
                break;
            case "PickWartermeloon":
                FMODUnity.RuntimeManager.PlayOneShot(PickWatermelon, position);
                break;
            case "PickPineapple":
                FMODUnity.RuntimeManager.PlayOneShot(PickPineapple, position);
                break;
            case "PickApple":
                FMODUnity.RuntimeManager.PlayOneShot(PickApple, position);
                break;
            case "NimHurt":
                FMODUnity.RuntimeManager.PlayOneShot(NimHurt, position);
                break;      
            case "PickLife":
                FMODUnity.RuntimeManager.PlayOneShot(PickLife, position);
                break;
            case "EnemyDeath":
                FMODUnity.RuntimeManager.PlayOneShot(EnemyDeath, position);
                break;
            case "PickBit":
                FMODUnity.RuntimeManager.PlayOneShot(PickBit, position);
                break;
        }
        
    }

    public void PlayExplosion(Vector3 position, float delay = 0f)
    {
        explosionPosition = position;
        Invoke("FxExplosion", delay);
    }

    void FxExplosion()
    {
        FMODUnity.RuntimeManager.PlayOneShot(PineappleExplosion, explosionPosition);
    }

    public void Play(string effectName, Vector3 position)
    {
        switch (effectName)
        {
            case "PickBit":
                pickBit.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                pickBit.start();
              break;
        }

    }



}
