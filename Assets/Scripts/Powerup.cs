using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

	enum Powerups {None = -1, Jump = 0, Zhonya = 1};
	Powerups actualPowerup = Powerups.None;

	string collectableTag = "Collectable";

	public void UsePowerup(){
		Debug.Log(actualPowerup);
	}

	void OnTriggerEnter(Collider other){

		if (other.tag == collectableTag){

			actualPowerup = (Powerups)other.GetComponent<Collectable>().PickUp();

		}
	}
}