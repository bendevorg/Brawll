using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	public int difficulty = 1;
	public bool trueMode = true;
	int maxDifficulty = 5;

	public GameObject restartUI;
	public GameObject nextUI;
	public GameObject endUI;

	bool storyMode = false;
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

					if (storyMode){

						if (difficulty >= maxDifficulty){

							endUI.SetActive(true);
							difficulty = 1;

							return;

						}

						difficulty++;
						nextUI.SetActive(true);
						Invoke("StartGame", 10);
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

	public void StartGame(){

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

	public void SetDifficulty(int _difficulty){

		Debug.Log(_difficulty);

		if (_difficulty == 0){

			storyMode = true;
			difficulty = 1;

		} else {

			storyMode = false;
			difficulty = (_difficulty <= maxDifficulty) && _difficulty > 0?_difficulty:1;

		}

		Debug.Log(storyMode);

	}

	public void SetTrueMode(){
		trueMode = !trueMode;
	}

}
