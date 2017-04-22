using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity {

	Movement movementController;

	private List<Player> players = new List<Player>();
	Target target;

	Vector2 input = Vector2.zero;

	SphereCollider sphereCollider;

	Rigidbody rb;

	enum State {Engage, Dash};
	State enemyState;

	float maxYDistanceToHit;
	public float minDistanceToDash = 10f;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();
		sphereCollider = GetComponent<SphereCollider>();
		movementController = GetComponent<Movement>();

		//	Se a sphere mudar de tamanho online mudar isso para um update
		maxYDistanceToHit = sphereCollider.radius;

		List<GameObject> livingEntities = new List<GameObject>();
		livingEntities.AddRange(GameObject.FindGameObjectsWithTag("Player"));
		livingEntities.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

		foreach (GameObject livingEntity in livingEntities){

			if (livingEntity != this.gameObject) players.Add(new Player(livingEntity, livingEntity.transform.position, livingEntity.GetComponent<Rigidbody>()));
			livingEntity.GetComponent<LivingEntity>().OnEntityDeath += RemoveEntity;

		}
	}

	public override void Update(){

		base.Update();

	}

	void FixedUpdate(){

		DecideNextMovement();

		switch (enemyState){

			case State.Dash:

				movementController.Dash(input.x, input.y, dashForce);
				break;

			case State.Engage:

				Debug.DrawLine (transform.position, (Vector2)transform.position + input * 10, Color.red, Mathf.Infinity);
				movementController.Move(input.x, input.y, speed);
				break;

		}

	}

	public void DecideNextMovement(){

		DecideTarget();

		DecideState();

		//	Decidindo a direção que vamos
		Vector3 finalTargetPos = target.position + (target.velocity);
		//Vector3 direction = (finalTargetPos - transform.position);

		Vector3 direction = (target.position - transform.position);
		input = new Vector2(direction.x, direction.z);


	}
	
	public void DecideState(){

		if (target.distance <= minDistanceToDash){

			if (Mathf.Abs(target.position.y - transform.position.y) <= maxYDistanceToHit && CanDash()) {

				enemyState = State.Dash;
				Debug.Log("Use dash");
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

		public Player(GameObject _entity, Vector3 _position, Rigidbody _rb){

			entity = _entity;
			position = _position;
			rb = _rb;

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

	}

	public struct Target{

		public float distance;
		public Vector3 position;
		public Vector3 velocity;

	}

}
