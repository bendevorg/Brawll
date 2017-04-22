using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	public float minTimeToRespawn = 5f;
	public float maxTimeToRespawn = 10f;

	BoxCollider collider;
	MeshRenderer renderer;

	// Use this for initialization
	void Start () {

		collider = GetComponent<BoxCollider>();
		renderer = GetComponent<MeshRenderer>();

		collider.enabled = false;
		renderer.enabled = false;

		StartCoroutine(Spawn());

	}
	
	public int PickUp(){

		collider.enabled = false;
		renderer.enabled = false;

		StartCoroutine(Spawn());

		return Random.Range(0, 1);

	}

	IEnumerator Spawn(){

		float respawnTime = Random.Range(minTimeToRespawn, maxTimeToRespawn);
		float nextRespawnTime = Time.time + respawnTime;

		while (Time.time < nextRespawnTime){

			yield return null;

		}

		renderer.enabled = true;
		collider.enabled = true;

	}
}
