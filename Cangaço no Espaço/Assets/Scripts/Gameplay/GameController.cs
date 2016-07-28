using UnityEngine;
using System.Collections;
using System;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    public int bitsCollectedInThisScene = 0;

    private int bitsToCollectInThisScene; // = 8;

    int monsterKilled;
    public int monstersToKill = 256 - 8;

    public GameObject boss;

    public Canvas bossCanvas;
    public Animator bossTitleAnim;

    public CameraFollow cameraFollow;
    
    public GameObject deathScreen;
    public Transform canvasGlitch;
    public GameObject winScreen;
    
    public bool startWin = false;

    public GameObject canvasSplash;
    public Animator panelSplashAnimator;
    public float delaySplashPanel = 4f;
    
    public bool gamePaused = false;
    public GameObject panelPause;
    public GameObject canvasStartScreen;

    GameObject stageUI;

    public bool gameStarted = false;

    
    void Awake()
    {
        if (instance == null)
            instance = this;

        canvasSplash.SetActive(false);
        deathScreen.SetActive(false);
        winScreen.SetActive(false);

        GameplayEvents.nimOnStage.Subscribe(OnNimOnStage);

        if (startWin)
        {
            FindObjectOfType<NimHealth>().PlayerWon();
            winScreen.SetActive(true);
        }

        if (boss != null)
            boss.SetActive(false);

        Retroboy.DungeonGenerator dg = FindObjectOfType<Retroboy.DungeonGenerator>();

        if (dg)
        {
            bitsToCollectInThisScene = dg.bitNumber;
        }
    }

    void OnLevelWasLoaded()
    {
        if (GameSave.instance.gameStarted)
        {
            //jump start screen
            GameStart();
        }
            
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameplayEvents.enemyDied.Subscribe(EnemyDiedEvent);

        Debug.Log("go to sleep");
        //Nim sleeping
        NimMove.instance.nimAnimator.SetTrigger("sleep");
    }

    private void OnNimOnStage(NimHealth nimHealth)
    {
        stageUI = FindObjectOfType<HudController>().gameObject;

        if (GameSave.instance.gameStarted)
        {
            WakeNim();
            return;
        }

        canvasStartScreen.SetActive(true);
        NimHealth.instance.transform.FindChild("Sleep_balloon").gameObject.SetActive(true);

        NimMove.instance.enabled = false;
        NimHealth.instance.nimHud.SetActive(false);
        stageUI.SetActive(false);

        FindObjectOfType<CameraFollow>().verticalOffset = 1f;
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (!GameSave.instance.gameStarted && canvasStartScreen.activeSelf && Input.anyKeyDown)
        {
            GameSave.instance.StartGame();
            GameStart();
            WakeNim();
        }
    }

    void GameStart()
    {
        canvasSplash.SetActive(true);
        panelSplashAnimator.SetTrigger("init");
        Invoke("HideSplashPanel", delaySplashPanel);
    }



    void WakeNim()
    {
        NimHealth.instance.transform.FindChild("Sleep_balloon").gameObject.SetActive(false);
        NimHealth.instance.nimAnimator.SetTrigger("awake");

        NimMove.instance.enabled = true;
        NimHealth.instance.nimHud.SetActive(true);
        stageUI.SetActive(true);

        FindObjectOfType<CameraFollow>().verticalOffset = 0f;

        canvasStartScreen.SetActive(false);
    }

    


    void OnDestroy()
    {
        GameplayEvents.enemyDied.Unsubscribe();
        GameplayEvents.nimDied.Unsubscribe();
        GameplayEvents.nimOnStage.Unsubscribe();
    }

    void HideSplashPanel()
    {
        canvasSplash.SetActive (false);
    }

    public bool IsGamePaused()
    {
        return gamePaused;
    }

    private void EnemyDiedEvent(EnemyHealth eHealth)
    {
        CancelInvoke("ReturnTimeScale");
        
        Time.timeScale = 0.25f;
        Invoke("ReturnTimeScale", 0.007f);
    }

    public void PickBit()
    {
        bitsCollectedInThisScene++;
		GameSave.totalBitsCollected++;

        if(RetroJukebox.control != null)
            RetroJukebox.control.PlayOneShot("PickupBit", NimMove.instance.transform.position);
        //JukeboxPlayer.control.Play("Effects", "PickupBit", true);

        if (bitsCollectedInThisScene >= bitsToCollectInThisScene)
        {
            //if (changeScene)
            //{

            GameSave.instance.NextScene();

            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex + 1);
            //}
            //else
            //{
            //	Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            //	Instantiate(teleportPrefab, playerPos, Quaternion.identity);
            //}
            //TODO implement boss
            //if (boss != null)
            //{
            //	boss.SetActive(true);
            //}

            ////Show boss camera, show title animation, then return to player
            //if (!showedTitle)
            //{
            //	ShowBoss();
            //}
        }
    }
    
    public void MonsterKilled()
    {
        monsterKilled++;
        if (monsterKilled >= monstersToKill)
        {
            if (boss != null)
                boss.GetComponent<EnemyHealth>().invincible = false;
        }
    }

    void ShowBoss()
    {

        ChangeCamera();
        Invoke("ShowBossTitle", 1f);
        Invoke("ShowPlayer", 4f);
    }

    void ChangeCamera()
    {
        //if (cameraFollow != null)
            //cameraFollow.SetTarget("Boss");
    }

    void ShowBossTitle()
    {

        //Canvas title
        //showedTitle = true;

        if (bossCanvas != null)
            bossCanvas.enabled = true;
        if (bossTitleAnim != null)
            bossTitleAnim.SetTrigger("show");
    }

    public void SetDeath(bool active)
    {
        deathScreen.SetActive(active);
    }

    public void WinGame()
    {
        winScreen.SetActive(true);
    }

    void ShowPlayer()
    {

        if (cameraFollow != null)
            //cameraFollow.SetTarget("Player");

        if (bossCanvas != null)
            bossCanvas.enabled = false;
    }

    void ReturnTimeScale()
    {
        Time.timeScale = 1f;
    }




    //PAUSE SCRIPT

    
    

    

    public void TogglePause()
    {
        gamePaused = !gamePaused;
        panelPause.SetActive(gamePaused);

        if (gamePaused)
        {
            Time.timeScale = 0;
            NimHealth.instance.nimAnimator.SetTrigger("sleep");
        }
        else {
            Time.timeScale = 1;
            //WakeNim();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }



}
