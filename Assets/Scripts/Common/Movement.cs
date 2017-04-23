using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {

	public ParticleSystem hitEffect;
	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	public void Move(float inputHorizontal, float inputVertical, float speed) {
		Vector3 movement = new Vector3(inputHorizontal, 0.0f, inputVertical).normalized;

		rb.AddForce(movement * speed);
	}

	public void Dash(float inputHorizontal, float inputVertical, float force){
		Vector3 dashForce = new Vector3(inputHorizontal, 0.0f, inputVertical).normalized * force;
		rb.AddForce(dashForce, ForceMode.Impulse);
		AudioManager.instance.PlaySound("Dash");
	}

	void Knockback(Collision col) {
		CameraShaker.Shake(0.2f, 0.2f);
		Vector3 knockback = col.contacts[0].normal * col.relativeVelocity.magnitude;
		Vector3 knockbackClamped = Vector3.ClampMagnitude(knockback, 10);
		rb.AddForce(knockbackClamped, ForceMode.Impulse);

		Instantiate(hitEffect, col.contacts[0].point, Quaternion.FromToRotation(Vector3.forward, col.relativeVelocity));
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
