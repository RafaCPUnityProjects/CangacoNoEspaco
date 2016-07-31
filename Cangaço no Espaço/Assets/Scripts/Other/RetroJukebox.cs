using UnityEngine;
using System.Collections;
using FMOD;

public class RetroJukebox : MonoBehaviour {
    
    public static RetroJukebox control;

    [FMODUnity.EventRef]
    public string Music;
    [FMODUnity.EventRef]
    public string PAtaque;
    [FMODUnity.EventRef]
    public string PDano;
    [FMODUnity.EventRef]
    public string PMorte;
    [FMODUnity.EventRef]
    public string PPVMais;
    [FMODUnity.EventRef]
    public string PPVMenos;
    [FMODUnity.EventRef]
    public string PPFMais;
    [FMODUnity.EventRef]
    public string PPFMenos;
    [FMODUnity.EventRef]
    public string PPLMais;
    [FMODUnity.EventRef]
    public string PPLMenos;
    [FMODUnity.EventRef]
    public string IDano;
    [FMODUnity.EventRef]
    public string IAlerta;
    [FMODUnity.EventRef]
    public string IMorte;
    [FMODUnity.EventRef]
    public string IAtaque;
    [FMODUnity.EventRef]
    public string BDano;
    [FMODUnity.EventRef]
    public string BAlerta;
    [FMODUnity.EventRef]
    public string BMorte;
    [FMODUnity.EventRef]
    public string BAtaque;

    FMOD.Studio.EventInstance music;
    FMOD.Studio.EventInstance pickBit;

    //save explosion position
    Vector3 explosionPosition;

 


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

        //pickBit = FMODUnity.RuntimeManager.CreateInstance(PickBit);
  
    }

    void OnDestroy()
    {
        if(music != null)
            music.release();
        if (pickBit != null)
            pickBit.release();

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
            case "PDano":
                FMODUnity.RuntimeManager.PlayOneShot(PDano, position);
                break;
            case "PMorte":
                FMODUnity.RuntimeManager.PlayOneShot(PMorte, position);
                break;
            case "PAtaque":
                FMODUnity.RuntimeManager.PlayOneShot(PAtaque, position);
                break;
            case "PPVMais":
                FMODUnity.RuntimeManager.PlayOneShot(PPVMais, position);
                break;
            case "PPVMenos":
                FMODUnity.RuntimeManager.PlayOneShot(PPVMenos, position);
                break;
            case "PPFMais":
                FMODUnity.RuntimeManager.PlayOneShot(PPFMais, position);
                break;
            case "PPFMenos":
                FMODUnity.RuntimeManager.PlayOneShot(PPFMenos, position);
                break;
            case "PPLMais":
                FMODUnity.RuntimeManager.PlayOneShot(PPLMais, position);
                break;      
            case "PPLMenos":
                FMODUnity.RuntimeManager.PlayOneShot(PPLMenos, position);
                break;
            case "IDano":
                FMODUnity.RuntimeManager.PlayOneShot(IDano, position);
                break;
            case "IAlerta":
                FMODUnity.RuntimeManager.PlayOneShot(IAlerta, position);
                break;
            case "IAtaque":
                FMODUnity.RuntimeManager.PlayOneShot(IAtaque, position);
                break;
            case "IMorte":
                FMODUnity.RuntimeManager.PlayOneShot(IMorte, position);
                break;
            case "BDano":
                FMODUnity.RuntimeManager.PlayOneShot(BDano, position);
                break;
            case "BAlerta":
                FMODUnity.RuntimeManager.PlayOneShot(BAlerta, position);
                break;
            case "BAtaque":
                FMODUnity.RuntimeManager.PlayOneShot(BAtaque, position);
                break;
            case "BMorte":
                FMODUnity.RuntimeManager.PlayOneShot(BMorte, position);
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
