﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NDJGameController : MonoBehaviour
{
	public float timeToShowBoss = 5f;
	public float timeToPlayer = 5f;
	public HUDController hudController;
	public GameObject grayscaleCamera;
	public Vector3 bossOffset;
	public GameObject splashScreen;
	public GameObject winScreen;
	public GameObject loseScreen;
	public Text barrilYears;

	private Transform player;
	private Transform boss;
	private Transform cameraTarget;
	private NPCController playerController;
	private NPCController bossController;

	bool gameStarted = false;
	private bool gameReady = false;
	private bool canRestart = false;
	private bool bossDefeated = false;

	public int deathCount
	{
		get { return PlayerPrefs.GetInt("DeathCount", 0); }
		set { PlayerPrefs.SetInt("DeathCount", value); }
	}

	void Start()
	{
		grayscaleCamera.SetActive(false);
		winScreen.SetActive(false);
		loseScreen.SetActive(false);
		StartCoroutine(LookAtBoss());
	}

	IEnumerator LookAtBoss()
	{
		yield return null;
		yield return null;
		while (boss == null)
		{
			boss = GameObject.FindGameObjectWithTag("Boss").transform;
			yield return null;
		}
		while (player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player").transform;
			yield return null;
		}
		while (cameraTarget == null)
		{
			cameraTarget = GameObject.FindGameObjectWithTag("CameraTarget").transform;
			yield return null;
		}
		bossController = boss.GetComponent<NPCController>();
		bossController.gameController = this;
		playerController = player.GetComponent<NPCController>();
		playerController.gameController = this;
		playerController.canMove = false;
		ChangeHealth(playerController.myBodyInfo.maxLife, playerController.myBodyInfo.maxLife);
		ChangeStrength(playerController.myBodyInfo.strength);
		ChangeSpeed(playerController.myBodyInfo.speed);
		splashScreen.SetActive(true);
		barrilYears.text = ((1 + deathCount) * 10).ToString();
		hudController.gameObject.SetActive(false);
		cameraTarget.position = boss.position + bossOffset;
		gameReady = true;
		yield return null;
		//yield return new WaitForSeconds(timeToShowBoss);
	}

	IEnumerator LookAtPlayer()
	{
		splashScreen.SetActive(false);
		hudController.gameObject.SetActive(true);
		float elapsedTime = 0.0f;
		while (Vector3.Distance(cameraTarget.position, player.position) > 0.1f)
		{
			cameraTarget.position = Vector3.Lerp(boss.position, player.position, elapsedTime / timeToPlayer);
			yield return null;
			elapsedTime += Time.deltaTime;
		}
		cameraTarget.localPosition = Vector3.zero;
		playerController.canMove = true;
	}


	void Update()
	{
		if (!gameStarted && gameReady && Input.anyKey)
		{
			gameStarted = true;
			StartCoroutine(LookAtPlayer());
		}
		if (canRestart && Input.anyKey)
		{
			ReloadScene();
		}
	}

	public void ChangeHealth(int currentHealth, int maxHealth)
	{
		hudController.SetLife(currentHealth, maxHealth);
	}

	public void ChangeSpeed(int currentSpeed)
	{
		hudController.SetSpeed(currentSpeed);
	}

	public void ChangeStrength(int currentStrength)
	{
		hudController.SetStrength(currentStrength);
	}

	public void BossDefeated()
	{
		deathCount = 0;
		playerController.myBodyInfo.SaveBody();
		winScreen.SetActive(true);
		ActivateGrayscale();
		Invoke("RestartScene", 5f);
		bossDefeated = true;
	}

	public void PlayerDefeated()
	{
		if (bossDefeated)
		{
			return;
		}
		deathCount++;
		loseScreen.SetActive(true);
		ActivateGrayscale();
		//Invoke("RestartScene", 5f);
	}

	void ActivateGrayscale()
	{
		hudController.gameObject.SetActive(false);
		grayscaleCamera.SetActive(true);
	}

	void RestartScene()
	{
		canRestart = true;
	}

	public void ResetBoss()
	{
		BodyInfo bodyInfo = new BodyInfo(0, 0, 0, 0, 0, 0, 0);
		bodyInfo.SaveBody();
		deathCount = 0;
		ReloadScene();
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(1);
	}
}
