using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour {

	[Range(1, 1000)]
	public int life = 1;
	public float speed = 25;
	public float dashForce = 25;

	public float dashCooldown = 2f;
	public float enemyRandomCooldown = .2f;
	float randomCooldown = 0f;

	float timeToDash = 0f;
	bool canDash = true;

	public event Action<GameObject> OnEntityDeath;
	public ParticleSystem deathEffect;

	public virtual void Start(){
		if (this.tag == "Enemy") randomCooldown = enemyRandomCooldown;
	}

	public virtual void Update(){

		if (!canDash){

			canDash = Time.time > timeToDash;
			GameController.gameController.SetDashText(timeToDash - Time.time, this.tag);

		}
	}

	public bool IsOffCooldown(){

		return canDash;

	}

	public void UseDash(){

		timeToDash = Time.time + dashCooldown + UnityEngine.Random.Range(-randomCooldown, randomCooldown);

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
