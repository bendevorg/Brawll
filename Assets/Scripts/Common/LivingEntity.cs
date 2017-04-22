using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour {

	[Range(1, 1000)]
	public int life = 1;
	public int speed = 10;
	public int dashForce = 10;

	public event Action<GameObject> OnEntityDeath;

	void TakeDamage(int damage){

		life -= damage;

		//	Aplicar efeito visual de tomar o dano aqui

		if (life <= 0) Death();

	}

	void Death(){
		
		//	Aplicar efeito visual de morrer aqui

		if (OnEntityDeath != null){
			OnEntityDeath(gameObject);
		}

		GameObject.Destroy(gameObject);

	}

	void OnTriggerEnter(Collider other) {

		Debug.Log("Olar");

		if (other.tag == "Instakill"){

			Debug.Log("Olar");
			Death();

		}
		
	}

}
