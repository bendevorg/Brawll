using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour {

	[Range(1, 1000)]
	public int life = 1;
	public int speed = 10;

	void TakeDamage(int damage){

		life -= damage;

		//	Aplicar efeito visual de tomar o dano aqui

		if (life <= 0) Death();

	}

	void Death(){
		
		//	Aplicar efeito visual de morrer aqui

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
