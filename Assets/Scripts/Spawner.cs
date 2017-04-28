using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject enemy;
	public GameObject player;
	Vector3 enemyStart = new Vector3(0.5f, 1f, -9.74f);
	Vector3 playerStart = new Vector3(0.48f, 0.99f, 10.01f);

	public void Awake(){

		SpawnPlayer(playerStart);

		if (GameController.gameController.gameMode == GameController.GameMode.Endless){
			StartCoroutine(SpawnEnemies());
		} else if (GameController.gameController.gameMode == GameController.GameMode.Campaign){
			SpawnNormalGame();
		} else {
			StartCoroutine(SpawnEnemies());
		}
	}

	public void SpawnNormalGame(){
		SpawnEnemy(enemyStart);
	}

	public void SpawnPlayer(Vector3 spawnPosition){
		GameController.gameController.InsertEntity (Instantiate(player, spawnPosition, Quaternion.identity).gameObject.GetComponent<LivingEntity>());
	}

	public void SpawnEnemy(Vector3 spawnPosition){
		GameController.gameController.InsertEntity (Instantiate(enemy, spawnPosition, Quaternion.identity).gameObject.GetComponent<LivingEntity>());
	}

	IEnumerator SpawnEnemies(){

		float nextEnemySpawn;
		float nextTimeSpawn;

		while (!GameController.gameController.gameOver){

			if (GameController.gameController.amountOfEnemiesToSpawn > 0){

				if (GameController.gameController.newWaveBegun){

					nextEnemySpawn = 3f;
					GameController.gameController.newWaveBegun = false;

				} else {

					nextEnemySpawn = Random.Range(GameController.gameController.minSpawnTime, GameController.gameController.maxSpawnTime);

				}

				nextTimeSpawn = Time.time + nextEnemySpawn;

				while (nextTimeSpawn > Time.time){

					yield return null;

				}

				SpawnEnemy(new Vector3(Random.Range(-10.5f, 10.5f), 1f, Random.Range(-10.5f, 10.5f)));

			}

			GameController.gameController.amountOfEnemiesToSpawn--;

			yield return null;

		}

	}

}
