using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {

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
	}

	void OnCollisionEnter(Collision collision) {
		string tag = collision.gameObject.tag;
		if (tag == "Enemy" || tag == "Player") {
			Vector3 knockback = collision.contacts[0].normal * collision.relativeVelocity.magnitude;
			Vector3 knockbackClamped = Vector3.ClampMagnitude(knockback, 10);
			rb.AddForce(knockbackClamped, ForceMode.Impulse);
		}
	}
}
