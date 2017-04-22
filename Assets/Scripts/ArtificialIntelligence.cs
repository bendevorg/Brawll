using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour {

	public void DecideNextMovement(){

		float minDistance = int.MaxValue;

		foreach (GameController.Player player in GameController.gameController.players) {

			float playerDistance = Vector3.Distance(transform.position, player.GetPosition());
			Debug.Log(playerDistance);

			if (playerDistance < minDistance) {

				minDistance = playerDistance;

			}

		}

	}

}
