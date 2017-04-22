using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int speed;
	private Movement movementController;

	void Start(){
		movementController = GetComponent<Movement>();
	}
	
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxisRaw("Horizontal");
    float moveVertical = Input.GetAxisRaw("Vertical");

		movementController.Move(moveHorizontal, moveVertical, speed);
	}
}
