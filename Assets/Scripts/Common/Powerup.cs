using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

	//	Jogar isso pro game controller
	enum Powerups {None = -1, Zhonya = 0};
	Powerups actualPowerup = Powerups.None;

	/*public float bulkMass = 4f;
	public float bulkSize = 4f;
	Vector3 bulkScale;
	public float timeToBulk = .2f;
	public float bulkDuration = 5f;
	[RangeAttribute(0, 1)]
	public float bulkSpeedPercentage = 0.2f;
	*/
	public float zhonyaDuration = 2f;
	[HideInInspector]
	public float forceMultiplier = 1f;

	Rigidbody rb;
	Renderer rend;
	Collider col;

	string collectableTag = "Collectable";

	void Start(){

		rb = GetComponent<Rigidbody>();
		rend = GetComponent<Renderer>();
		col = GetComponent<SphereCollider>();
		//bulkScale = new Vector3(bulkSize, bulkSize, bulkSize);

	}

	public void UsePowerup(){
		
		switch (actualPowerup){

			/*case Powerups.Bulk:
				
				StartCoroutine(Bulk());

				actualPowerup = Powerups.None;
				break;
			*/

			case Powerups.Zhonya:

				StartCoroutine(Zhonya());
				actualPowerup = Powerups.None;
				break;

			case Powerups.None:
				break;

		}

	}

	void OnTriggerEnter(Collider other){

		if (other.tag == collectableTag){

			actualPowerup = (Powerups)other.GetComponent<Collectable>().PickUp();

		}
	}
/* 
	IEnumerator Bulk(){

		float oldMass = rb.mass;
		Vector3 oldScale = transform.localScale;
		float timeToBulkUp = timeToBulk + Time.time;

		rb.isKinematic = true;

		while (rb.mass < bulkMass && transform.localScale.x < bulkScale.x){

			rb.mass = Mathf.Lerp(rb.mass, bulkMass, timeToBulkUp);
			transform.localScale = Vector3.Lerp(transform.localScale, bulkScale, timeToBulkUp);
			yield return null;

		}

		rb.isKinematic = false;

		rb.mass = Mathf.Clamp(rb.mass, oldMass, bulkMass);
		transform.localScale = Vector3.ClampMagnitude(transform.localScale, bulkScale.magnitude);
		float bulkBoostForce = bulkMass / oldMass;
		forceMultiplier *= (bulkBoostForce * bulkSpeedPercentage);

		float bulkDurationRemaining = bulkDuration + Time.time;

		while (bulkDurationRemaining > Time.time){
			yield return null;
		}
		
		rb.isKinematic = true;

		while (rb.mass > oldMass && transform.localScale.x > oldScale.x){

			rb.mass = Mathf.Lerp(rb.mass, oldMass, timeToBulkUp);
			transform.localScale = Vector3.Lerp(transform.localScale, oldScale, timeToBulkUp);
			yield return null;

		}

		rb.isKinematic = false;

		rb.mass = Mathf.Clamp(rb.mass, oldMass, oldMass);
		transform.localScale = Vector3.ClampMagnitude(oldScale, oldScale.magnitude);
		forceMultiplier /= (bulkBoostForce * bulkSpeedPercentage);

		yield return null;

	}
*/

	IEnumerator Zhonya(){

		float zhonyaTime = zhonyaDuration + Time.time;

		rb.isKinematic = true;
		col.enabled = false;

        rend.material.SetColor("_Color", Color.blue);

		AudioManager.instance.PlaySound("Zhonyas", transform.position);

		while (zhonyaTime > Time.time){

			yield return null;

		}

		rend.material.SetColor("_Color", Color.green);

		rb.isKinematic = false;
		col.enabled = true;

	}

}