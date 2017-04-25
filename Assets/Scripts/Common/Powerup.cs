using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

	//	Jogar isso pro game controller
	public enum Powerups {None = -1, Zhonya = 0, Reflection = 1};
	public enum States {None = -1, Zhonya = 0};

	Powerups actualPowerup = Powerups.None;
	States actualState = States.None;

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

	//	Reflection Special
	public GameObject reflectionSpecial;

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

			case Powerups.Reflection:
				GameObject specialInstance = Instantiate(reflectionSpecial, transform.position, Quaternion.identity) as GameObject;
				SpecialController specialController = specialInstance.GetComponent<SpecialController>();
				specialController.caster = this.gameObject;
				AudioManager.instance.PlaySound("Force");
				actualPowerup = Powerups.None;
				break;

			case Powerups.None:
				break;

		}

	}

	public int GetPowerup(){
		return (int)actualPowerup;
	}

	public int GetState(){
		return (int)actualState;
	}

	void OnTriggerEnter(Collider other){

		if (other.tag == collectableTag){

			actualPowerup = (Powerups)other.GetComponent<Collectable>().PickUp();
			GameController.gameController.SetPowerupText((int)actualPowerup, this.tag);

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

		actualState = States.Zhonya;
		rb.isKinematic = true;
		col.enabled = false;

		Color oldColor = rend.material.color;

        rend.material.SetColor("_Color", Color.blue);

		AudioManager.instance.PlaySound("Zhonyas");

		while (zhonyaTime > Time.time){

			yield return null;

		}

		rend.material.SetColor("_Color", oldColor);

		rb.isKinematic = false;
		col.enabled = true;
		actualState = States.None;

	}

}