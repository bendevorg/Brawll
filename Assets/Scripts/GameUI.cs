using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameUI : MonoBehaviour {

	public Text dashText;
	public Text powerupText;

	string[] powerups = {"", "Intagible", "Force"};

	public void SetDashText(float timeRemainingToDash){

		dashText.text = "Z: Dash";

		if (timeRemainingToDash > 0){

			dashText.text += " (" + Math.Round(timeRemainingToDash, 2) + ")";

		}
	}

	public void SetPowerupText(int powerup){
		powerupText.text = "X: " + powerups[powerup + 1];
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
