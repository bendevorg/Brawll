using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	public enum GameMode {Campaign = 0, Endless = 1, SingleMatch = 2};
	public GameMode gameMode = GameMode.Campaign;

	[HideInInspector]
	public int difficulty = 0;
	[HideInInspector]
	public bool trueMode = false;
	int maxDifficulty = 5;

	public GameObject restartUI;
	public GameObject nextUI;
	public GameObject endUI;
	public GameObject endlessUI;
	EndlessUI endlessUIController;

	//	Endless variables
	public Wave[] waves;
	[HideInInspector]
	public int amountOfEnemiesToSpawn = 0;
	[HideInInspector]
	public float minSpawnTime = 0;
	[HideInInspector]
	public float maxSpawnTime = 0;
	[HideInInspector]
	public bool newWaveBegun = false;
	int enemiesRemaining = 0;
	int wave = 0;

	//	Single match variables
	int amountOfEnemiesSingleMatch = 0;

	[HideInInspector]
	public bool gameOver = false;

	void Awake(){

		if(gameController != null){

			Destroy(gameObject);

		} else {

			gameController = this;
			DontDestroyOnLoad(gameObject);

		}

		endlessUIController = endlessUI.GetComponent<EndlessUI>();

	}

	// Update is called once per frame
	void Update () {

		//	TODO: REMOVER ISSO
		if (Input.GetKeyDown(KeyCode.Space)){
			RestartGame();
		}

		if (Input.GetButton("Submit")){
			PauseGame();
		}
		
	}

	public void PlayerDied(string tag){

		if (!gameOver){

			GameObject[] entity = GameObject.FindGameObjectsWithTag(tag);

			if (tag == "Player"){

				if (entity.Length <= 1){

					gameOver = true;
					restartUI.SetActive(true);
					return;

				}

			} else {

				if (gameMode == GameMode.Endless){

					enemiesRemaining--;
					endlessUIController.ChangeEnemiesRemainingText(enemiesRemaining);

				}

				if (entity.Length <= 1){

					if (gameMode == GameMode.Endless){

						if (enemiesRemaining <= 0){
							NextWave();
						}		

					} else {

						gameOver = true;

						if (gameMode == GameMode.Campaign){

							if (difficulty >= maxDifficulty){

								endUI.SetActive(true);
								difficulty = 1;

								return;

							}

							difficulty++;
							nextUI.SetActive(true);
							StartCoroutine(NextLevel());
							return; 

						} else {

							endUI.SetActive(true);
							return;

						}
					}
				}
			}
		}
	}

	public void PauseGame(){

		Time.timeScale = 1 - Time.timeScale;

	}

	public void StartGame(bool firstStart){

		DeactivateAllUI();
		gameOver = false;

		if (!(gameMode == GameMode.Campaign)){

			if (gameMode == GameMode.Endless){

				difficulty = 1;
				wave = 0;

				NextWave();

				endlessUI.SetActive(true);

			} else if (gameMode == GameMode.SingleMatch){

				SetSingleMatch();

			}

		} else if (firstStart){

			difficulty = 1;

		}

		Application.LoadLevel(1);
	}

	public void RestartGame(){

		gameOver = false;
		DeactivateAllUI();

		Time.timeScale = 1;
		Application.LoadLevel(0);
		Destroy(gameObject);
	}

	public void DeactivateAllUI(){

		restartUI.SetActive(false);
		nextUI.SetActive(false);
		endUI.SetActive(false);
		endlessUI.SetActive(false);

	}

	public void SetGamemode(int _gameMode){

		gameMode = (GameMode)_gameMode;

	}

	public void SetDifficulty(int _difficulty){

		_difficulty += 1;

		difficulty = (_difficulty <= maxDifficulty) && _difficulty > 0?_difficulty:1;

	}

	public void SetEnemies(int _amountOfEnemiesSingleMatch){
		amountOfEnemiesSingleMatch = _amountOfEnemiesSingleMatch;
	}
	
	public void NextWave(){

		if (wave >= waves.Length){

			wave = waves.Length - 1;

		}

		amountOfEnemiesToSpawn = waves[wave].amountOfEnemiesToSpawn;
		difficulty = waves[wave].difficulty;
		minSpawnTime = waves[wave].minSpawnTime;
		maxSpawnTime = waves[wave].maxSpawnTime;

		wave++;
		newWaveBegun = true;

		enemiesRemaining = amountOfEnemiesToSpawn;
		endlessUIController.ChangeWaveText(wave);
		endlessUIController.ChangeEnemiesRemainingText(amountOfEnemiesToSpawn);

	}
	
	IEnumerator NextLevel(){

		float nextLevelTime = 7f + Time.time;

		while (nextLevelTime > Time.time){

			yield return null;

		}

		StartGame(false);

	}

	public void SetSingleMatch(){

		amountOfEnemiesToSpawn = amountOfEnemiesSingleMatch;

	}

	[System.SerializableAttribute]
	public struct Wave{

		public int amountOfEnemiesToSpawn;
		public int difficulty;
		public float minSpawnTime;
		public float maxSpawnTime;

	}

}
