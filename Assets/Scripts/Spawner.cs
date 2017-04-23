using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject enemy;

	public void StartSpawning(){
		StartCoroutine(SpawnEnemies());
	}

	public void StopSpawning(){
		StopCoroutine(SpawnEnemies());
	}

	IEnumerator SpawnEnemies(){

		while (GameController.gameController.playEndless){

			float nextEnemySpawn = Random.Range(GameController.gameController.minSpawnTime, GameController.gameController.maxSpawnTime);
			float nextTimeSpawn = Time.time + nextEnemySpawn;

			while (nextTimeSpawn > Time.time){

				yield return null;
				Instantiate(enemy, new Vector3(Random.Range(-10.5f, 10.5f), 1f, Random.Range(-10.5f, 10.5f)), Quaternion.identity);

			}

			yield return null;

		}

	}

}
