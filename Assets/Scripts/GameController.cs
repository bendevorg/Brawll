using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	public enum GameMode {Campaign = 0, Endless = 1, SingleMatch = 2};
	public GameMode gameMode = GameMode.Campaign;

	public int difficulty = 0;
	public bool trueMode = false;
	int maxDifficulty = 5;

	public GameObject restartUI;
	public GameObject nextUI;
	public GameObject endUI;

	bool gameOver = false;

	void Awake(){

		if(gameController != null){

			Destroy(gameObject);

		} else {

			gameController = this;
			DontDestroyOnLoad(gameObject);

		}

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

				if (entity.Length <= 1){

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

	public void PauseGame(){
		Time.timeScale = 1 - Time.timeScale;
	}

	public void StartGame(bool firstStart){

		if (firstStart){

			if (gameMode == GameMode.Campaign || gameMode == GameMode.Endless){

				difficulty = 1;

			} else if (gameMode == GameMode.SingleMatch){

				//	Espera aí 

			}

		}

		gameOver = false;
		restartUI.SetActive(false);
		nextUI.SetActive(false);
		endUI.SetActive(false);

		Application.LoadLevel(1);
	}

	public void RestartGame(){

		gameOver = false;
		restartUI.SetActive(false);
		nextUI.SetActive(false);
		endUI.SetActive(false);

		Time.timeScale = 1;
		Application.LoadLevel(0);
		Destroy(gameObject);
	}

	public void SetGamemode(int _gameMode){

		gameMode = (GameMode)_gameMode;

	}

	public void SetDifficulty(int _difficulty){

		_difficulty += 1;

		difficulty = (_difficulty <= maxDifficulty) && _difficulty > 0?_difficulty:1;

	}

	IEnumerator NextLevel(){

		float nextLevelTime = 7f + Time.time;

		while (nextLevelTime > Time.time){

			yield return null;

		}

		StartGame(false);

	}

}
