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

	void Start(){

		movementController = GetComponent<Movement>();
		powerupController = GetComponent<Powerup>();

	}

	public override void Update() {

		base.Update();
		moveHorizontal = Input.GetAxisRaw("Horizontal");
		moveVertical = Input.GetAxisRaw("Vertical");

		willDash = Input.GetButtonDown("Fire3") && CanDash();
		usePowerup = Input.GetKeyDown(KeyCode.Z);

		if (usePowerup) {

			powerupController.UsePowerup();

		}

	}
	
	void FixedUpdate () {

		movementController.Move(moveHorizontal, moveVertical, speed * powerupController.forceMultiplier);
		if (willDash && CanDash()) {
			UseDash();
			movementController.Dash(moveHorizontal, moveVertical, dashForce * powerupController.forceMultiplier);
		}
	}
}
