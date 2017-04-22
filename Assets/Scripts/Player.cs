using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity {

	private Movement movementController;
	private float moveHorizontal;
	private float moveVertical;
	private bool willDash;

	void Start(){
		movementController = GetComponent<Movement>();
	}

	void Update() {
		moveHorizontal = Input.GetAxisRaw("Horizontal");
		moveVertical = Input.GetAxisRaw("Vertical");

		willDash = Input.GetButtonDown("Fire1");

	}
	
	void FixedUpdate () {

		movementController.Move(moveHorizontal, moveVertical, speed);
		if (willDash) {
			movementController.Dash(moveHorizontal, moveVertical, dashForce);
		}
	}
}
