using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour {

	private List<Player> players = new List<Player>();
	Target target;

	Vector2 input = Vector2.zero;

	Rigidbody rb;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();

		List<GameObject> livingEntities = new List<GameObject>();
		livingEntities.AddRange(GameObject.FindGameObjectsWithTag("Player"));
		livingEntities.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

		foreach (GameObject livingEntity in livingEntities){

			if (livingEntity != this.gameObject) players.Add(new Player(livingEntity, livingEntity.transform.position, livingEntity.GetComponent<Rigidbody>()));
			livingEntity.GetComponent<LivingEntity>().OnEntityDeath += RemoveEntity;

		}
		
	}

	public Vector2 DecideNextMovement(float speed){

		DecideTarget();

		Vector3 finalTargetPos = target.position + (target.velocity);

		//Vector3 direction = (finalTargetPos - transform.position).normalized;
		Vector3 direction = (target.position - transform.position).normalized;
		Debug.DrawLine (transform.position, transform.position + direction * 10, Color.red, Mathf.Infinity);
		return new Vector2(direction.x, direction.z);


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
	
	// Update is called once per frame
	void Update () {

		foreach (Player player in players){

			player.UpdatePlayerPosition();

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
