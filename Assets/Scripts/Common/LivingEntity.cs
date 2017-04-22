using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour {

	[Range(1, 1000)]
	public int life = 1;
	public float speed = 10;
	public float dashForce = 10;

	public float dashCooldown = 3f;
	float timeToDash = 0f;
	bool canDash = true;

	public event Action<GameObject> OnEntityDeath;

	public virtual void Update(){

		if (!canDash){

			//Debug.Log(timeToDash);

			canDash = Time.time >= timeToDash;

		}

	}

	public bool CanDash(){

		return canDash;

	}

	public void UseDash(){

		timeToDash = Time.time + dashCooldown;
		//Debug.Log("Reset: " + timeToDash);

		canDash = false;

	}

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

		if (other.tag == "Instakill"){

			Death();

		}	
	}
}
