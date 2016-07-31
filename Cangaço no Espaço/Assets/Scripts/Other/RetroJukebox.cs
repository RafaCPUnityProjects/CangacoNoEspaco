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
    [FMODUnity.EventRef]
    public string MortePU;
    

    FMOD.Studio.EventInstance music;
    FMOD.Studio.EventInstance Pataque;
    FMOD.Studio.EventInstance Pdano;
    FMOD.Studio.EventInstance Pmorte;
    FMOD.Studio.EventInstance Iataque;
    FMOD.Studio.EventInstance Ialerta;
    FMOD.Studio.EventInstance Idano;
    FMOD.Studio.EventInstance Imorte;
    FMOD.Studio.EventInstance Balerta;
    FMOD.Studio.EventInstance Bdano;
    FMOD.Studio.EventInstance Bmorte;
    FMOD.Studio.EventInstance Bataque;
    FMOD.Studio.EventInstance MortePu;

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
        Pataque = FMODUnity.RuntimeManager.CreateInstance(PAtaque);
        Pdano = FMODUnity.RuntimeManager.CreateInstance(PDano);
        Pmorte = FMODUnity.RuntimeManager.CreateInstance(PMorte);
        Iataque = FMODUnity.RuntimeManager.CreateInstance(IAtaque);
        Idano = FMODUnity.RuntimeManager.CreateInstance(IDano);
        Ialerta = FMODUnity.RuntimeManager.CreateInstance(IAlerta);
        Bataque = FMODUnity.RuntimeManager.CreateInstance(BAtaque);
        MortePu = FMODUnity.RuntimeManager.CreateInstance(MortePU);


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
        //FMODUnity.RuntimeManager.PlayOneShot(PineappleExplosion, explosionPosition);
    }

    public void Play(string effectName, Vector3 position)
    {
        switch (effectName)
        {
            case "PAtaque":
                Pataque.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Pataque.start();
                break;
            case "IAtaque":
                Iataque.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Iataque.start();
                break;
            case "BAtaque":
                Bataque.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Bataque.start();
                break;
            case "IAlerta":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start();
                break;
                //AQUI
            case "PDano":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "PMorte":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "PPVMais":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "PPVMenos":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "PPFMais":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "PPFMenos":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "PPLMais":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "PPLMenos":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "IDano":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "IMorte":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "BDano":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "BAlerta":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
            case "BMorte":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.start(); break;
        }

    }

    public void Stop(string effectName, Vector3 position)
    {
        switch (effectName)
        {
            case "IAlerta":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.stop(0);
                break;
        }
    }



}
