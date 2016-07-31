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
    [FMODUnity.EventRef]
    public string MortePLayer;
    [FMODUnity.EventRef]
    public string Texto;
    [FMODUnity.EventRef]
    public string BateBarril;
    [FMODUnity.EventRef]
    public string Grama;
    [FMODUnity.EventRef]
    public string Dobrado;


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
    FMOD.Studio.EventInstance MortePlayer;
    FMOD.Studio.EventInstance texto;

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
        Pataque = FMODUnity.RuntimeManager.CreateInstance(PAtaque);
        Pdano = FMODUnity.RuntimeManager.CreateInstance(PDano);
        Pmorte = FMODUnity.RuntimeManager.CreateInstance(PMorte);
        Iataque = FMODUnity.RuntimeManager.CreateInstance(IAtaque);
        Idano = FMODUnity.RuntimeManager.CreateInstance(IDano);
        Imorte = FMODUnity.RuntimeManager.CreateInstance(IMorte);
        Ialerta = FMODUnity.RuntimeManager.CreateInstance(IAlerta);
        Bataque = FMODUnity.RuntimeManager.CreateInstance(BAtaque);
        MortePu = FMODUnity.RuntimeManager.CreateInstance(MortePU);
        MortePlayer = FMODUnity.RuntimeManager.CreateInstance(MortePLayer);
        texto = FMODUnity.RuntimeManager.CreateInstance(Texto);
        music = FMODUnity.RuntimeManager.CreateInstance(Music);
        music.start();
        


        //pickBit = FMODUnity.RuntimeManager.CreateInstance(PickBit);

    }

    void OnDestroy()
    {
        if(music != null)
            music.release();
        

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
            case "Texto":
                FMODUnity.RuntimeManager.PlayOneShot(Texto, position);
                break;
            case "BateBarril":
                FMODUnity.RuntimeManager.PlayOneShot(BateBarril, position);
                break;
            case "Grama":
                FMODUnity.RuntimeManager.PlayOneShot(Grama, position);
                break;
            case "Dobrado":
                FMODUnity.RuntimeManager.PlayOneShot(Dobrado, position);
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
                Pdano.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Pdano.start(); break;
            case "PMorte":
                Pmorte.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Pmorte.start(); break;
            case "MortePLayer":
                MortePlayer.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                MortePlayer.start(); break;
            case "Texto":
                texto.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                texto.start(); break;
            /* case "PPVMais":
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
                 Ialerta.start(); break;*/
            case "IDano":
                Idano.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Idano.start(); break;
            case "IMorte":
                Imorte.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Imorte.start(); break;
            case "BDano":
                Bdano.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Bdano.start(); break;
            case "BAlerta":
                Balerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Balerta.start(); break;
            case "BMorte":
                Bmorte.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Bmorte.start(); break;
            case "MortePU":
                MortePu.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                MortePu.start(); break;
        }

    }

    public void Stop(string effectName, Vector3 position)
    {
        switch (effectName)
        {
            case "IAtaque":
                Iataque.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Iataque.stop(0);
                break;
            case "PAtaque":
                Pataque.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Pataque.stop(0);
                break;
            case "BAtaque":
                Bataque.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Bataque.stop(0);
                break;
            case "IAlerta":
                Ialerta.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                Ialerta.stop(0);
                break;
            case "MortePLayer":
                MortePlayer.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
                MortePlayer.stop(0);
                break;
        }
    }



}
