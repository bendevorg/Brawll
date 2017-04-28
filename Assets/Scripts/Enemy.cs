using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Powerup))]
public class Enemy : LivingEntity {

	Movement movementController;
	Powerup powerupController;

	public LayerMask obstacleMask;
	public LayerMask playerMask;
	Transform ground;

	Target target;

	SphereCollider sphereCollider;
	Rigidbody rb;
	Renderer rend;

	//	Difficulty parameter variables
	[RangeAttribute(1, 5)]
	int difficulty = 1;
	int minDifficultyToPredict = 4;
	bool decisionHandicap = false;

	float zhonyasChance = 10;
	float forceChance = 10;
	float instakillChance = 10;
	float dashChance = 10;
	float handicap;

	public Color[] enemyColors;

	//	Behavior variables
	enum Behavior {Engage, Dash, Desengage, Powerup, PickPowerup};
	Behavior enemyBehavior;

	//	Decision variables
	float maxYDistanceToHit;
	public float minDistanceToDash = 2.5f;
	float impossibleDodgeDistanceDash = 0.25f;

	Vector3 futurePosition;
	float safeDistance = 2.7f;

	Vector2 input = Vector2.zero;
	Vector2 inputPrediction = Vector2.zero;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();
		sphereCollider = GetComponent<SphereCollider>();
		ground = GameObject.FindGameObjectWithTag("Ground").transform;
		movementController = GetComponent<Movement>();
		powerupController = GetComponent<Powerup>();
		rend = GetComponent<Renderer>();

		difficulty = GameController.gameController.difficulty;
		decisionHandicap = GameController.gameController.trueMode;

		handicap = 5/difficulty;

		//	20% incrementando na dificuldade
		if (!decisionHandicap){
			zhonyasChance /= handicap;
			forceChance /= handicap;
			instakillChance /= handicap;
			dashChance /= handicap;
		}
	
		minDistanceToDash += handicap;

		if (difficulty - 1 <= enemyColors.Length - 1){
			rend.material.SetColor("_Color", enemyColors[difficulty - 1]);
		} else {
			rend.material.SetColor("_Color", enemyColors[enemyColors.Length - 1]);	
		}

		//	Se a sphere mudar de tamanho online mudar isso para um update
		maxYDistanceToHit = sphereCollider.radius;

	}

	void FixedUpdate(){

		DecideNextMovement();

		switch (enemyBehavior){

			case Behavior.Desengage:

				Desengage();
				break;

			case Behavior.Dash:

				movementController.Dash(input.x, input.y, 1/handicap);
				break;

			case Behavior.Engage:

				// Debug.DrawLine (transform.position, (Vector2)transform.position + input * 10, Color.red, Mathf.Infinity);
				movementController.Move(inputPrediction.x, inputPrediction.y, 1/handicap);
				break;

			case Behavior.Powerup:

				powerupController.UsePowerup();
				break;

			case Behavior.PickPowerup:

				movementController.Move(input.x, input.y, 1/handicap);
				break;

		}

	}

	public void DecideNextMovement(){

		DecideTarget();
		DecideBehavior();

		//	Decidindo a direção que vamos
		Vector3 direction = (target.position - transform.position);

		Vector3 relativeVelocityToTarget = Vector3.Project(rb.velocity, direction);
		float enemyVelocityToTarget = relativeVelocityToTarget.sqrMagnitude==0?0.1f:relativeVelocityToTarget.sqrMagnitude;
		float enemyTimeToCoverDistance = target.distance/enemyVelocityToTarget;
		Debug.Log(enemyTimeToCoverDistance);

		Vector3 finalTargetPos = target.position + (target.velocity/1000);

		Vector3 directionPredicted = difficulty < minDifficultyToPredict?(target.position - transform.position):(finalTargetPos - transform.position);

		input = new Vector2(direction.x, direction.z);
		inputPrediction = new Vector2(directionPredicted.x, directionPredicted.z);

	}

	void DecideTarget(){

		foreach (GameController.Player player in GameController.gameController.players) {

			if (player.GetLivingEntity() != this){

				target.distance = int.MaxValue;

				float playerDistance = Vector3.Distance(transform.position, player.GetPosition());

				if (playerDistance < target.distance) {

					target.distance = playerDistance;
					target.position = player.GetPosition();
					target.velocity = player.GetVelocity();
					target.state = player.GetState();

				}
			}		
		}
	}
	
	public void DecideBehavior(){
		
		//	Kill certo
		if (IsInstakillAvaiable()){

			enemyBehavior = Behavior.Dash;
			return;

		}

		//	Verifica se o inimigo mais próximo está vindo na minha direção
		if (IsUnderAttack()) {

			//	Zhonyas
			if (IsDefensivePowerupAvaiable()) {

				enemyBehavior = Behavior.Powerup;
				return;

			}
		}

		if (IsGoingOffGround()){

			enemyBehavior = Behavior.Desengage;
			return;

		}

		if (IsDashAvaiable()){

			enemyBehavior = Behavior.Dash;
			return;
			
		}

		enemyBehavior = Behavior.Engage;
		return;

	}

	bool IsUnderAttack(){

		//	Add velocity check
		return Physics.Raycast(target.position, target.velocity, target.distance, playerMask);
	
	}

	bool IsGoingOffGround(){

		futurePosition = (transform.position + (rb.velocity/4));
		return !Physics.Raycast(futurePosition, -Vector3.up, Mathf.Infinity, obstacleMask);

	}

	bool IsInstakillAvaiable(){

		return target.distance <= impossibleDodgeDistanceDash && movementController.CanDash() && Random.Range(1, 11) <= instakillChance && target.state != (int)LivingEntity.State.Zhonya;
	
	}

	bool IsDefensivePowerupAvaiable(){

		return target.velocity.sqrMagnitude > rb.velocity.sqrMagnitude && ((powerupController.GetPowerup() == (int)Powerup.Powerups.Zhonya && Random.Range(1, 11) <= zhonyasChance) || (powerupController.GetPowerup() == (int)Powerup.Powerups.Reflection && Random.Range(1, 11) <= forceChance));
	
	}

	bool IsDashAvaiable(){

		return target.distance <= minDistanceToDash && Mathf.Abs(target.position.y - transform.position.y) <= maxYDistanceToHit && movementController.CanDash() && Random.Range(1, 11) <= dashChance && target.state != (int)LivingEntity.State.Zhonya;
	
	}

	void Desengage(){

		Vector3 direction = (ground.position - transform.position);
		input = new Vector2(direction.x, direction.z);

		if (Vector3.Distance(transform.position, futurePosition) > safeDistance){

			if (powerupController.GetPowerup() == (int)Powerup.Powerups.Zhonya){

				powerupController.UsePowerup();

			} else if (movementController.CanDash()){

				movementController.Dash(input.x, input.y, 1/handicap);

			} else {

				movementController.Move(input.x, input.y, 1/handicap);

			}

		} else {

			movementController.Move(input.x, input.y, 1/handicap);

		}	
	}

	public struct Target{

		public float distance;
		public Vector3 position;
		public Vector3 velocity;
		public int state;

	}
}
