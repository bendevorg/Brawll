using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	public float minTimeToRespawn = 5f;
	public float maxTimeToRespawn = 10f;

	BoxCollider collider;
	MeshRenderer renderer;
	Renderer rend;

	int powerupAmount = 2;
	int actualPowerup;

	public Color[] powerupColors;

	// Use this for initialization
	void Start () {

		collider = GetComponent<BoxCollider>();
		renderer = GetComponent<MeshRenderer>();
		rend = GetComponent<Renderer>();

		collider.enabled = false;
		renderer.enabled = false;

		StartCoroutine(Spawn());

	}
	
	public int PickUp(){

		collider.enabled = false;
		renderer.enabled = false;

		StartCoroutine(Spawn());

		return actualPowerup;

	}

	IEnumerator Spawn(){

		float respawnTime = Random.Range(minTimeToRespawn, maxTimeToRespawn);
		float nextRespawnTime = Time.time + respawnTime;

		while (Time.time < nextRespawnTime){

			yield return null;

		}

		Vector3 spawnPosition = new Vector3(Random.Range(-10.5f, 10.5f), 1f, Random.Range(-10.5f, 10.5f));

		transform.position = Vector3.ClampMagnitude(spawnPosition, new Vector3(7.5f, 0, 7.5f).magnitude);

		actualPowerup = Random.Range(0, powerupAmount);
		rend.material.color = actualPowerup <= powerupColors.Length?powerupColors[actualPowerup]:Color.black;

		renderer.enabled = true;
		collider.enabled = true;

	}
}
