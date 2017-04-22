using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity {

	ArtificialIntelligence artificialIntelligence;
	Movement movementController;

	void Start(){

		movementController = GetComponent<Movement>();
		artificialIntelligence = GetComponent<ArtificialIntelligence>();

	}

	void Update(){

		Vector2 direction = artificialIntelligence.DecideNextMovement(speed);
		movementController.Move(direction.x, direction.y, speed);

	}

}
