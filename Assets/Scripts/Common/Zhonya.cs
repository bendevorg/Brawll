using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zhonya : MonoBehaviour {

	public float zhonyaDuration = 2f;
	[HideInInspector]
	public float forceMultiplier = 1f;

	Rigidbody rb;
	Renderer rend;
	Collider col;

	bool onZhonya = false;

	// Use this for initialization
	void Start () {
		
		rb = GetComponent<Rigidbody>();
		rend = GetComponent<Renderer>();
		col = GetComponent<SphereCollider>();

	}
	
	public void ActivateZhonya(){

		StartCoroutine(StartZhonya());

	}

	public bool IsZhonyaActive(){
		return onZhonya;
	}

	IEnumerator StartZhonya(){

		float zhonyaTime = zhonyaDuration + Time.time;

		onZhonya = true;
		rb.isKinematic = true;
		col.enabled = false;

		Color oldColor = rend.material.color;

        rend.material.SetColor("_Color", Color.blue);

		AudioManager.instance.PlaySound("Zhonyas");

		while (zhonyaTime > Time.time){

			yield return null;

		}

		rend.material.SetColor("_Color", oldColor);

		onZhonya = false;
		rb.isKinematic = false;
		col.enabled = true;

	}

}
