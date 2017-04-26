using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Powerup))]
public class Player : LivingEntity {

	private Movement movementController;
	private Powerup powerupController;

	private float moveHorizontal;
	private float moveVertical;
	
	private bool willDash;
	private bool usePowerup;

	public bool player1;

	string[] player1Inputs = {"Horizontal", "Vertical", "Fire2", "Fire1"};
	string[] player2Inputs = {"HorizontalP2", "VerticalP2", "Fire2P2", "Fire1P2"};

	string[] actualPlayerInputs;

	void Start(){

		actualPlayerInputs = player1?player1Inputs:player2Inputs;

		movementController = GetComponent<Movement>();
		powerupController = GetComponent<Powerup>();

	}

	void Update() {

		moveHorizontal = Input.GetAxisRaw(actualPlayerInputs[0]);
		moveVertical = Input.GetAxisRaw(actualPlayerInputs[1]);

		willDash = Input.GetButtonDown(actualPlayerInputs[2]);
		usePowerup = Input.GetButtonDown(actualPlayerInputs[3]);

		if (usePowerup) {

			powerupController.UsePowerup();
			GameController.gameController.SetPowerupText((int)Powerup.Powerups.None, this.tag);

		}
	}
	
	void FixedUpdate () {

		movementController.Move(moveHorizontal, moveVertical, 1f);

		if (willDash) {

			movementController.Dash(moveHorizontal, moveVertical, 1f);

		}
	}
}
