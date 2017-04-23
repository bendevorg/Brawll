using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	Renderer rend;
	Collider col;

	string playerTag = "Player";
	string enemyTag = "Enemy";

	public Color[] obstacleStates;
	int wallState = 0;

	public GameObject breakEffect;

	// Use this for initialization
	void Start () {

		rend = GetComponent<Renderer>();
		col = GetComponent<BoxCollider>();

	}

	void DegradadeObstacle(){

		if (wallState < obstacleStates.Length && wallState > -1){
			rend.material.SetColor("_Color", obstacleStates[wallState]);
		}
		wallState++;

	}

	void DestroyObstacle(GameObject hitter){
		
		Destroy(Instantiate(breakEffect, hitter.transform.position, Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + 108f, transform.localEulerAngles.z)), 2);
		GameObject.Destroy(gameObject);

	}
	
	void OnCollisionEnter(Collision other){

		string colTag = other.gameObject.tag;

		if (colTag == playerTag || colTag == enemyTag){

			DegradadeObstacle();

		}	
	}
	
	void OnCollisionExit(Collision other){

		if (wallState > obstacleStates.Length - 1){

			col.isTrigger = true;

		}

	}

	void OnTriggerEnter(Collider other){

		AudioManager.instance.PlaySound("Impact Wall", Vector3.zero);
		DestroyObstacle(other.gameObject);
		return;

	}

}
