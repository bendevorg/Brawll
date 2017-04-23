using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject enemy;
	Vector3 enemyStart = new Vector3(0.5f, 1f, -9.74f);

	public void Start(){

		if (GameController.gameController.gameMode == GameController.GameMode.Endless){
			StartCoroutine(SpawnEnemies());
		} else if (GameController.gameController.gameMode == GameController.GameMode.Campaign){
			SpawnNormalGame();
		} else {
			StartCoroutine(SpawnEnemies());
		}

	}

	public void SpawnNormalGame(){
		Instantiate(enemy, enemyStart, Quaternion.identity);
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

				Instantiate(enemy, new Vector3(Random.Range(-10.5f, 10.5f), 1f, Random.Range(-10.5f, 10.5f)), Quaternion.identity);

			}

			GameController.gameController.amountOfEnemiesToSpawn--;

			yield return null;

		}

	}

}
