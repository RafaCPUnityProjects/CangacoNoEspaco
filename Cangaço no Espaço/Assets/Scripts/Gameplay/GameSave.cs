using UnityEngine;
using System.Collections;
using System;

public class GameSave : MonoBehaviour {

	public static GameSave instance;

	public int coins;
	public int tries;
	public static int nimHealth;

	public static int totalBitsCollected = 0;
    public static CurrentWeapon nimWeapon;
	public static NimWeapon nimGunWeapon;

    const int MAXCOINS = 999999;

    public bool gameStarted = false;

    public int currentScene = 0;

    //TODO make a scene manager
    public RetroScene[] scenes;

    //public static void SetNimHealth(int value)
    //{
    //	nimHealth = value;
    //}

    //public static int GetNimHealth()
    //{
    //	return nimHealth;
    //}

    void Awake(){

		//Check if other GameSave is on scene
		GameSave[] myInstances = FindObjectsOfType<GameSave> ();
		if (myInstances.Length > 1)
			Destroy (this.gameObject);

		if (instance == null)
			instance = this;

		DontDestroyOnLoad (this.gameObject);
	}

	public void Reset()
	{
		coins = 0;
		//tries = 0;
		nimHealth = 3;
		totalBitsCollected = 0;
		nimWeapon = CurrentWeapon.None;
		nimGunWeapon = null;
	}

	public void CoinCollect(int newCoins){
		if(coins - newCoins <= MAXCOINS)
			coins += newCoins;
	}

	public void NewTry(){
		tries++;
	}

    public void StartGame()
    {
        gameStarted = true;
    }

    public void RestartGame()
    {
        gameStarted = false;
    }

    public void NextScene()
    {
        currentScene++;
        if (currentScene > scenes.Length)
            currentScene = 0;
    }

    public int CurrentSceneNumber()
    {
        return (currentScene + 1);
    }

    public bool isBossStage()
    {
        return scenes[currentScene].boss;
    }
}
