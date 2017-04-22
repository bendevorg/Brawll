﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {

	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	public void Move(float inputHorizontal, float inputVertical, int speed) {
		Vector3 movement = new Vector3(inputHorizontal, 0.0f, inputVertical);

		rb.AddForce(movement * speed);
	}
}
