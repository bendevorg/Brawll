using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {

	public ParticleSystem hitEffect;
	public ParticleSystem dashEffect;
	private Rigidbody rb;

	void Awake () {
		rb = GetComponent<Rigidbody>();
	}

	void Update(){
		if (rb.velocity.magnitude > 0.1f) {
			transform.forward = rb.velocity.normalized;
		}
	}

	public void Move(float inputHorizontal, float inputVertical, float speed) {
		Vector3 movement = new Vector3(inputHorizontal, 0.0f, inputVertical).normalized;

		rb.AddForce(movement * speed);
	}

	public void Dash(float inputHorizontal, float inputVertical, float force){
		Vector3 dashOffset = new Vector3(0, 0, 0.6f);

		GameObject dashGameObject = Instantiate(dashEffect.gameObject, transform.position, Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y - 180, transform.localEulerAngles.z)) as GameObject;

		dashGameObject.transform.parent = transform;
		dashGameObject.transform.localScale = Vector3.one;
		dashGameObject.transform.localPosition = dashOffset;

		Destroy(dashGameObject, 2f);
		
		Vector3 dashForce = new Vector3(inputHorizontal, 0.0f, inputVertical).normalized * force;
		rb.AddForce(dashForce, ForceMode.Impulse);
		AudioManager.instance.PlaySound("Dash");
	}

	void Knockback(Collision col) {
		CameraShaker.Shake(0.2f, 0.2f);
		Vector3 knockback = col.contacts[0].normal * col.relativeVelocity.magnitude;
		Vector3 knockbackClamped = Vector3.ClampMagnitude(knockback, 10);
		rb.AddForce(knockbackClamped, ForceMode.Impulse);

		Destroy(Instantiate(hitEffect, col.contacts[0].point, Quaternion.FromToRotation(Vector3.forward, col.relativeVelocity)), 2f);
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
