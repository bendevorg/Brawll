using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	public bool pickupAvaiable = false;

	public float minTimeToRespawn = 5f;
	public float maxTimeToRespawn = 10f;

	BoxCollider col;
	MeshRenderer meshRenderer;
	Renderer rend;

	int powerupAmount = 2;
	[HideInInspector]
	public int actualPowerup;

	public Color[] powerupColors;

	// Use this for initialization
	void Start () {

		col = GetComponent<BoxCollider>();
		meshRenderer = GetComponent<MeshRenderer>();
		rend = GetComponent<Renderer>();

		col.enabled = false;
		meshRenderer.enabled = false;

		StartCoroutine(Spawn());

	}

	void Update() {
		transform.Rotate(0, 1f, 0);
	}
	
	public int PickUp(){

		col.enabled = false;
		meshRenderer.enabled = false;
		pickupAvaiable = false;

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

		meshRenderer.enabled = true;
		col.enabled = true;
		pickupAvaiable = true;

	}
}
