using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {

	public ParticleSystem hitEffect;
	public ParticleSystem dashEffect;
	private Rigidbody rb;

	public float speed = 25;
	public float dashForce = 25;
	public float dashCooldown = 2f;

	float timeToDash = 0f;
	bool dashOffCooldown = true;

	void Awake () {

		rb = GetComponent<Rigidbody>();

	}

	void Update(){

		if (rb.velocity.magnitude > 0.1f) {

			transform.forward = rb.velocity.normalized;

		}
	}

	public void Move(float inputHorizontal, float inputVertical, float bonusMultiplier) {
		
		Vector3 movement = new Vector3(inputHorizontal, 0.0f, inputVertical).normalized;

		rb.AddForce(movement * (speed * bonusMultiplier));

	}

	public void Dash(float inputHorizontal, float inputVertical, float bonusMultiplier){
		
		if (CanDash()){

			StartCoroutine(UseDash());

			Vector3 dashOffset = new Vector3(0, 0, 0.6f);

			GameObject dashGameObject = Instantiate(dashEffect.gameObject, transform.position, Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y - 180, transform.localEulerAngles.z)) as GameObject;

			dashGameObject.transform.parent = transform;
			dashGameObject.transform.localScale = Vector3.one;
			dashGameObject.transform.localPosition = dashOffset;

			Destroy(dashGameObject, 2f);
			
			Vector3 force = new Vector3(inputHorizontal, 0.0f, inputVertical).normalized * (dashForce * bonusMultiplier);
			rb.AddForce(force, ForceMode.Impulse);
			AudioManager.instance.PlaySound("Dash");

		}
		
	}

	public bool CanDash(){

		//	TODO: implementar eventos que bloqueiam o dash
		return dashOffCooldown;

	}

	void Knockback(Collision col) {

		CameraShaker.Shake(0.2f, 0.2f);
		Vector3 knockback = col.contacts[0].normal * col.relativeVelocity.magnitude;
		Vector3 knockbackClamped = Vector3.ClampMagnitude(knockback, 10);
		rb.AddForce(knockbackClamped, ForceMode.Impulse);

		Destroy(Instantiate(hitEffect, col.contacts[0].point, Quaternion.FromToRotation(Vector3.forward, col.relativeVelocity)), 2f);

	}

	IEnumerator UseDash(){

		timeToDash = Time.time + dashCooldown;
		dashOffCooldown = false;

		while (timeToDash > Time.time) {

			GameController.gameController.SetDashText(timeToDash - Time.time, this.tag);
			yield return null;

		}

		GameController.gameController.SetDashText(0, this.tag);
		dashOffCooldown = true;

	}

	void OnCollisionEnter(Collision collision) {

		string tag = collision.gameObject.tag;

		if (tag == "Enemy" || tag == "Player") {

			Knockback(collision);
			AudioManager.instance.PlaySound("Impact");

		} else if (tag == "Obstacle") {

			Knockback(collision);
			AudioManager.instance.PlaySound("Impact Wall");

		}
	}

}
