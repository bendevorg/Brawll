using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	//private List<Player> players = new List<Player>();

	void Awake(){

		if(gameController != null && gameController != this){

			Destroy(this.gameObject);

		} else {

			gameController = this;
			DontDestroyOnLoad(gameObject);

		}

	}

	// Use this for initialization
	void Start () {

		/*List<GameObject> livingEntities = new List<GameObject>();
		livingEntities.AddRange(GameObject.FindGameObjectsWithTag("Player"));
		livingEntities.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

		foreach (GameObject livingEntity in livingEntities){

			players.Add(new Player(livingEntity, livingEntity.transform.position));

		}*/
		
	}
	
	// Update is called once per frame
	void Update () {

		/*foreach (Player player in players){

			player.UpdatePlayerPosition();

		} */
		
	}

	/*public List<Player> GetPlayers(){
		return players;
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

	}*/

}
