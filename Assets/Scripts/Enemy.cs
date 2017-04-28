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

				Debug.DrawRay (transform.position, input, Color.red, .1f);
				movementController.Move(input.x, input.y, 1/handicap);
				break;

		}

	}

	public void DecideNextMovement(){

		DecideTarget();
		DecideBehavior();

		//	Decidindo a direção que vamos
		Vector3 finalTargetPos = target.position + (target.velocity/10);

		Vector3 direction = (target.position - transform.position);
		Vector3 directionPredicted = difficulty < minDifficultyToPredict?(target.position - transform.position):(finalTargetPos - transform.position);

		input = new Vector2(direction.x, direction.z);
		inputPrediction = new Vector2(directionPredicted.x, directionPredicted.z);

	}

	void DecideTarget(){

		foreach (GameController.Player player in GameController.gameController.players) {

			if (player.GetLivingEntity() != this){

				target.distance = int.MaxValue;

				player.UpdatePlayerPosition();

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

		int instakillValue = Random.Range(1, 11);
		int zhonyasValue = Random.Range(1, 11);
		int forcevalue = Random.Range(1, 11);
		int dashValue = Random.Range(1, 11);
		
		//	Kill certo
		if (target.distance <= impossibleDodgeDistanceDash && movementController.CanDash() && instakillValue <= instakillChance && target.state != (int)LivingEntity.State.Zhonya){

			enemyBehavior = Behavior.Dash;
			return;

		}

		//	Verifica se o inimigo mais próximo está vindo na minha direção
		if (Physics.Raycast(target.position, target.velocity, target.distance, playerMask)) {

			//	Zhonyas
			if (target.velocity.sqrMagnitude > rb.velocity.sqrMagnitude && ((powerupController.GetPowerup() == (int)Powerup.Powerups.Zhonya && zhonyasValue <= zhonyasChance) || (powerupController.GetPowerup() == (int)Powerup.Powerups.Reflection && forcevalue <= forceChance))) {

				enemyBehavior = Behavior.Powerup;
				return;

			}

		}

		/*//	Verifica se existe um pickup force disponível
		foreach (Collectable collectable in collectables){
			if (collectable.pickupAvaiable && collectable.actualPowerup == (int)Powerup.Powerups.Reflection){

				// Caso haja um force ele muda o target para a force
				target.position = collectable.transform.position;
				target.velocity = Vector3.zero;
				target.state = (int)Powerup.Powerups.None;
				enemyState = State.PickPowerup;
				return;

			}
		}*/

		
		futurePosition = (transform.position + (rb.velocity/4));

		if (!Physics.Raycast(futurePosition, -Vector3.up, Mathf.Infinity, obstacleMask)){

			enemyBehavior = Behavior.Desengage;
			return;

		}

		if (target.distance <= minDistanceToDash && dashValue <= dashChance){

			if (Mathf.Abs(target.position.y - transform.position.y) <= maxYDistanceToHit && movementController.CanDash() && dashValue <= dashChance && target.state != (int)LivingEntity.State.Zhonya) {

				enemyBehavior = Behavior.Dash;
				return;

			}
		}

		enemyBehavior = Behavior.Engage;
		return;

	}

	public struct Target{

		public float distance;
		public Vector3 position;
		public Vector3 velocity;
		public int state;

	}

}
