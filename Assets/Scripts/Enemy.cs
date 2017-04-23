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

	private List<Player> players = new List<Player>();
	Target target;

	Vector2 input = Vector2.zero;
	Vector2 inputPrediction = Vector2.zero;

	SphereCollider sphereCollider;
	Rigidbody rb;
	Renderer rend;

	[RangeAttribute(1, 5)]
	int difficulty = 1;
	int minDifficultyToPredict = 4;
	bool decisionHandicap = false;

	//	Parametros de dificuldades
	float zhonyasChance = 10;
	float instakillChance = 10;
	float dashChance = 10;
	float handicap;

	public Color[] enemyColors;

	enum State {Engage, Dash, Desengage, Powerup};
	State enemyState;

	float maxYDistanceToHit;
	public float minDistanceToDash = 3f;
	float impossibleDodgeDistanceDash = 0.3f;

	// Use this for initialization
	void Start () {

		difficulty = GameController.gameController.difficulty;
		decisionHandicap = GameController.gameController.trueMode;

		handicap = 5/difficulty;

		//	20% incrementando na dificuldade
		if (!decisionHandicap){
			zhonyasChance /= handicap;
			instakillChance /= handicap;
			dashChance /= handicap;
		}
		
		speed /=  handicap;
		dashForce /= handicap;
		minDistanceToDash += handicap;

		rb = GetComponent<Rigidbody>();
		sphereCollider = GetComponent<SphereCollider>();
		ground = GameObject.FindGameObjectWithTag("Ground").transform;
		movementController = GetComponent<Movement>();
		powerupController = GetComponent<Powerup>();
		rend = GetComponent<Renderer>();

		if (difficulty - 1 <= enemyColors.Length - 1){
			rend.material.SetColor("_Color", enemyColors[difficulty - 1]);
		} else {
			rend.material.SetColor("_Color", enemyColors[enemyColors.Length - 1]);	
		}

		//	Se a sphere mudar de tamanho online mudar isso para um update
		maxYDistanceToHit = sphereCollider.radius;

		List<GameObject> livingEntities = new List<GameObject>();
		livingEntities.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
		livingEntities.AddRange(GameObject.FindGameObjectsWithTag("Player"));

		foreach (GameObject livingEntity in livingEntities){

			if (livingEntity != this.gameObject) players.Add(new Player(livingEntity, livingEntity.transform.position, livingEntity.GetComponent<Rigidbody>(), livingEntity.GetComponent<Powerup>()));
			livingEntity.GetComponent<LivingEntity>().OnEntityDeath += RemoveEntity;

		}
	}

	public override void Update(){

		base.Update();

	}

	void FixedUpdate(){

		DecideNextMovement();

		switch (enemyState){

			case State.Desengage:

				Vector3 direction = (ground.position - transform.position);
				input = new Vector2(direction.x, direction.z);
				movementController.Move(input.x, input.y, speed);
				break;

			case State.Dash:

				movementController.Dash(input.x, input.y, dashForce);
				break;

			case State.Engage:

				Debug.DrawLine (transform.position, (Vector2)transform.position + input * 10, Color.red, Mathf.Infinity);
				movementController.Move(inputPrediction.x, inputPrediction.y, speed);
				break;

			case State.Powerup:

				powerupController.UsePowerup();
				break;

		}

	}

	public void DecideNextMovement(){

		DecideTarget();

		DecideState();

		//	Decidindo a direção que vamos
		Vector3 finalTargetPos = target.position + (target.velocity);

		Vector3 direction = (target.position - transform.position);
		Vector3 directionPredicted = difficulty < minDifficultyToPredict?(target.position - transform.position):(finalTargetPos - transform.position);

		input = new Vector2(direction.x, direction.z);
		inputPrediction = new Vector2(directionPredicted.x, directionPredicted.z);

	}
	
	public void DecideState(){

		int instakillValue = Random.Range(1, 11);
		int zhonyasValue = Random.Range(1, 11);
		int dashValue = Random.Range(1, 11);
		
		//	Kill certo
		if (target.distance <= impossibleDodgeDistanceDash && CanDash() && instakillValue <= instakillChance && target.state != (int)Powerup.States.Zhonya){

			enemyState = State.Dash;
			UseDash();
			return;

		}

		//	Verifica se o inimigo mais próximo está vindo na minha direção
		if (Physics.Raycast(target.position, target.velocity, target.distance, playerMask)) {

			//	Zhonyas
			if (target.velocity.sqrMagnitude > rb.velocity.sqrMagnitude && powerupController.GetPowerup() == (int)Powerup.Powerups.Zhonya && zhonyasValue <= zhonyasChance) {

				enemyState = State.Powerup;
				return;

			}

		}

		Vector3 oneSecondFuturePosition = (transform.position + rb.velocity);

		if (!Physics.Raycast(oneSecondFuturePosition, -Vector3.up, Mathf.Infinity, obstacleMask)){

			enemyState = State.Desengage;
			return;

		}

		if (target.distance <= minDistanceToDash){

			if (Mathf.Abs(target.position.y - transform.position.y) <= maxYDistanceToHit && CanDash() && dashValue <= dashChance && target.state != (int)Powerup.States.Zhonya) {

				enemyState = State.Dash;
				UseDash();
				return;

			}
		}

		enemyState = State.Engage;
		return;

	}

	void DecideTarget(){

		foreach (Player player in players) {

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

	public void RemoveEntity(GameObject livingEntity){

		foreach(Player player in players){

			if (player.GetGameObject() == livingEntity){

				players.Remove(player);
				return;

			}
		}
	}

	public struct Player{

		GameObject entity;
		Vector3 position;
		Rigidbody rb;
		Powerup powerup;

		public Player(GameObject _entity, Vector3 _position, Rigidbody _rb, Powerup _powerup){

			entity = _entity;
			position = _position;
			rb = _rb;
			powerup = _powerup;

		}

		public void UpdatePlayerPosition(){
			position = entity.transform.position;
		}

		public GameObject GetGameObject(){
			return entity;
		}

		public Vector3 GetPosition(){
			return position;
		}

		public Vector3 GetVelocity(){
			return rb.velocity;
		}

		public int GetState(){
			return powerup.GetState();
		}

	}

	public struct Target{

		public float distance;
		public Vector3 position;
		public Vector3 velocity;
		public int state;

	}

}
