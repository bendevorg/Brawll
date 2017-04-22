using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity {

	ArtificialIntelligence artificialIntelligence;

	void Start(){

		artificialIntelligence = GetComponent<ArtificialIntelligence>();

	}

	void Update(){

		artificialIntelligence.DecideNextMovement();

	}

}
