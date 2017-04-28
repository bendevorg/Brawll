using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

	public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	public enum GameMode {Campaign = 0, Endless = 1, SingleMatch = 2};
	public GameMode gameMode = GameMode.Campaign;

	public int difficulty = 0;
	[HideInInspector]
	public bool trueMode = false;
	int maxDifficulty = 5;

	//	UI variables
	public GameObject restartUI;
	public GameObject nextUI;
	public GameObject endUI;
	public GameObject endlessUI;
	EndlessUI endlessUIController;
	public GameObject gameUI;
	GameUI gameUIController; 

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
	int singleMatchEnemiesAmount;

	//	Players and enemies management
	public event Action<LivingEntity> OnEntitySpawn;
	public List<Player> players = new List<Player>();

	int playersActiveAmount = 0;
	int enemiesActiveAmount = 0;

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
		gameUIController = gameUI.GetComponent<GameUI>();

	}

	// Update is called once per frame
	void Update () {

		//	TODO: REMOVER ISSO
		if (Input.GetKeyDown(KeyCode.Escape)){
			RestartGame();
		}

		if (Input.GetButton("Submit")){
			PauseGame();
		}
		
	}

	public void InsertEntity(LivingEntity livingEntity){

			players.Add(new Player(livingEntity.GetComponent<LivingEntity>(), livingEntity.transform.position, livingEntity.GetComponent<Rigidbody>()));
			livingEntity.GetComponent<LivingEntity>().OnEntityDeath += RemoveEntity;

			if (livingEntity.tag == "Player") playersActiveAmount++;
			if (livingEntity.tag == "Enemy") enemiesActiveAmount++;

	}

	void RemoveEntity(LivingEntity livingEntity){

		//	Melhorar isso
		foreach(Player player in players){

			if (player.GetLivingEntity() == livingEntity){

				players.Remove(player);
				break;

			}
		}

		if (livingEntity.tag == "Player") playersActiveAmount--;
		if (livingEntity.tag == "Enemy") enemiesActiveAmount--;

		if (!gameOver) CheckGameStatus();

	}

	void CheckGameStatus(){

		if (playersActiveAmount <= 0){

			GameOver(false);
			return;

		} 
		
		if (enemiesActiveAmount <= 0){

			if (gameMode == GameMode.Endless) {

				NextWave();

			} else {

				GameOver(true);

			}

		} else {

			if (gameMode == GameMode.Endless){

				enemiesRemaining--;
				endlessUIController.ChangeEnemiesRemainingText(enemiesRemaining);

			}
		}

	}

	public void StartGame(bool firstStart){

		ResetGameInfo();

		if (gameMode == GameMode.Endless) SetEndlessMatch();
		else if (gameMode == GameMode.SingleMatch) SetSingleMatch();
		else if (firstStart) difficulty = 1; 

		gameUI.SetActive(true);
		SetDashText(0, "Player");
		SetPowerupText((int)Powerup.Powerups.None, "Player");

		Application.LoadLevel(1);
	}

	public void RestartGame(){

		ResetGameInfo();
		Application.LoadLevel(0);

		//	Nao deveria deletar isso para score
		Destroy(gameObject);
	}

	void ResetGameInfo(){

		DeactivateAllUI();
		gameOver = false;

		players.Clear();
		playersActiveAmount = 0;
		enemiesActiveAmount = 0;

	}

	void GameOver(bool won){

		gameOver = true;

		//	If player loses
		if (!won){

			restartUI.SetActive(true);
			return;

		}
		
		//	Check if there`s still game to go on
		if (gameMode == GameMode.Campaign){

			if (difficulty < maxDifficulty){

				StartCoroutine(NextLevel());
				return; 

			}
		}

		//	Play won, end of game
		endUI.SetActive(true);
		return;

	}

	void PauseGame(){

		Time.timeScale = 1 - Time.timeScale;

	}

	void DeactivateAllUI(){

		restartUI.SetActive(false);
		nextUI.SetActive(false);
		endUI.SetActive(false);
		endlessUI.SetActive(false);
		gameUI.SetActive(false);

	}

	void SetEndlessMatch(){

		wave = 0;
		NextWave();
		endlessUI.SetActive(true);

	}

	void SetSingleMatch(){
		amountOfEnemiesToSpawn = singleMatchEnemiesAmount;
	}

	//	Retirar a verificação de tag
	public void SetDashText(float timeRemainingToDash, string tag){
		if (tag == "Player") gameUIController.SetDashText(timeRemainingToDash);
	}

	public void SetPowerupText(int powerup, string tag){
		if (tag == "Player") gameUIController.SetPowerupText(powerup);
	}

	public void SetGamemode(int _gameMode){

		gameMode = (GameMode)_gameMode;

	}

	public void SetDifficulty(int _difficulty){

		_difficulty += 1;

		difficulty = (_difficulty <= maxDifficulty) && _difficulty > 0?_difficulty:1;

	}

	public void SetEnemies(int _amountOfEnemiesToSpawn){
		singleMatchEnemiesAmount = amountOfEnemiesToSpawn = _amountOfEnemiesToSpawn;
	}
	
	void NextWave(){

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

		difficulty++;
		nextUI.SetActive(true);

		float nextLevelTime = 7f + Time.time;

		while (nextLevelTime > Time.time){

			yield return null;

		}

		StartGame(false);

	}

	[System.SerializableAttribute]
	public struct Wave{

		public int amountOfEnemiesToSpawn;
		public int difficulty;
		public float minSpawnTime;
		public float maxSpawnTime;

	}

	public struct Player{

		LivingEntity entity;
		Vector3 position;
		Rigidbody rb;

		public Player(LivingEntity _entity, Vector3 _position, Rigidbody _rb){

			entity = _entity;
			position = _position;
			rb = _rb;

		}

		public void UpdatePlayerPosition(){
			position = entity.transform.position;
		}

		public LivingEntity GetLivingEntity(){
			return entity;
		}

		public Vector3 GetPosition(){
			return position;
		}

		public Vector3 GetVelocity(){
			return rb.velocity;
		}

		public int GetState(){
			return (int)entity.GetState();
		}

	}

}
