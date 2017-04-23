using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	public int difficulty = 1;
	public bool trueMode = true;
	int maxDifficulty = 5;

	public GameObject UI;

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

	public void PlayerDied(){

		GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
		
		if (player.Length <= 1){

			UI.SetActive(true);

		}
	}

	public void PauseGame(){
		Time.timeScale = 1 - Time.timeScale;
	}

	public void StartGame(){
		Application.LoadLevel(1);
	}

	public void RestartGame(){

		UI.SetActive(false);
		Time.timeScale = 1;
		Application.LoadLevel(0);
		Destroy(gameObject);
	}

	public void SetDifficulty(int _difficulty){
		_difficulty  += 1;
		difficulty = (_difficulty <= maxDifficulty) && _difficulty > 0?_difficulty:1;
	}

	public void SetTrueMode(){
		trueMode = !trueMode;
	}

}
