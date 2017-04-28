using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulk : MonoBehaviour {

	public float bulkMass = 4f;
	public float bulkSize = 4f;
	Vector3 bulkScale;
	public float timeToBulk = .2f;
	public float bulkDuration = 5f;
	[RangeAttribute(0, 1)]
	public float bulkSpeedPercentage = 0.2f;

	Rigidbody rb;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		bulkScale = new Vector3(bulkSize, bulkSize, bulkSize);
	}

	public void ActivateBulk(){
		StartCoroutine(StartBulk());
	}
 
	IEnumerator StartBulk(){

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
		//float bulkBoostForce = bulkMass / oldMass;
		//forceMultiplier *= (bulkBoostForce * bulkSpeedPercentage);

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
		//forceMultiplier /= (bulkBoostForce * bulkSpeedPercentage);

		yield return null;

	}

}
