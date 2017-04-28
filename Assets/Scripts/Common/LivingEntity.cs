using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour {

	[Range(1, 1000)]
	public int life = 1;

	public enum State {None = -1, Zhonya = 0};
	public State actualState = State.None;

	public event Action<LivingEntity> OnEntityDeath;
	public ParticleSystem deathEffect;

	public int GetState(){
		return (int)actualState;
	}

	void TakeDamage(int damage){

		life -= damage;
		//	Aplicar efeito visual de tomar o dano aqui

		if (life <= 0) Death();

	}

	void Death(){

		CameraShaker.Shake(0.3f, 0.3f);
		AudioManager.instance.PlaySound("Death");

		Destroy(Instantiate(deathEffect, transform.position, Quaternion.identity).gameObject, 2f);

		if (OnEntityDeath != null){
			OnEntityDeath(this);
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
