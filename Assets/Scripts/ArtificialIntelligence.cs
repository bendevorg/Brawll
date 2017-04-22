using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour {

	private List<Player> players = new List<Player>();
	Target target;

	Vector2 input = Vector2.zero;

	// Use this for initialization
	void Start () {

		List<GameObject> livingEntities = new List<GameObject>();
		livingEntities.AddRange(GameObject.FindGameObjectsWithTag("Player"));
		livingEntities.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

		foreach (GameObject livingEntity in livingEntities){

			if (livingEntity != this.gameObject) players.Add(new Player(livingEntity, livingEntity.transform.position));

		}
		
	}

	public Vector2 DecideNextMovement(){

		DecideTarget();

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

			}

		}

	}
	
	// Update is called once per frame
	void Update () {

		foreach (Player player in players){

			player.UpdatePlayerPosition();

		}
		
	}

	public struct Player{

		GameObject entity;
		Vector3 position;

		public Player(GameObject _entity, Vector3 _position){

			entity = _entity;
			position = _position;

		}

		public void UpdatePlayerPosition(){
			position = entity.transform.position;
		}

		public Vector3 GetPosition(){
			return position;
		}

	}

	public struct Target{

		public float distance;
		public Vector3 position;

	}

}
