using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessUI : MonoBehaviour {

	public Text waveText;
	public Text enemiesRemainingText;

	public void ChangeWaveText(int wave){

		waveText.text = "Wave: " + wave;

	}

	public void ChangeEnemiesRemainingText(int enemiesRemaining){

		enemiesRemainingText.text = "Enemies to next wave: " + enemiesRemaining;

	}

}
