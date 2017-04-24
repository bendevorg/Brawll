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
	public ParticleSystem deathEffect;

	public virtual void Update(){

		if (!canDash){

			canDash = Time.time >= timeToDash;

		}
	}

	public bool IsOffCooldown(){

		return canDash;

	}

	public void UseDash(){

		timeToDash = Time.time + dashCooldown;

		canDash = false;

	}

	void TakeDamage(int damage){

		life -= damage;

		//	Aplicar efeito visual de tomar o dano aqui

		if (life <= 0) Death();

	}

	void Death(){
		CameraShaker.Shake(0.3f, 0.3f);
		AudioManager.instance.PlaySound("Death");

		Instantiate(deathEffect, transform.position, Quaternion.identity);

		if (OnEntityDeath != null){
			OnEntityDeath(gameObject);
		}

		GameController.gameController.PlayerDied(gameObject.tag);
		GameObject.Destroy(gameObject);

	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Instakill"){

			Death();

		}	
	}
}
