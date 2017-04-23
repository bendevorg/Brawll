using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text sliderValue;
	public Slider slider;

	public GameObject difficulty;
	public GameObject enemies;
	
	// Update is called once per frame
	void Update () {

		sliderValue.text = slider.value.ToString();

		GameController.gameController.SetEnemies((int)slider.value);
		
	}

	public void onGameModeChange(int gameMode){

		switch (gameMode){

			case 0:
				onCampaign();
				break;

			case 1:
				onEndless();
				break;
			
			case 2:
				onSingleMatch();
				break;

		}

	}

	void onCampaign(){

		difficulty.SetActive(false);
		enemies.SetActive(false);

		//	Texto falando sobre como é a campanha?

	}

	void onEndless(){

		difficulty.SetActive(false);
		enemies.SetActive(false);

		//	Texto falando sobre como é o endless?

	}

	void onSingleMatch(){

		difficulty.SetActive(true);
		enemies.SetActive(true);

	}

}
