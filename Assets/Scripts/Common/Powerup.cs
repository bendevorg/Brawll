using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Zhonya))]
public class Powerup : MonoBehaviour {

	//	Jogar isso pro game controller
	public enum Powerups {None = -1, Zhonya = 0, Reflection = 1};

	Powerups actualPowerup = Powerups.None;

	/*public float bulkMass = 4f;
	public float bulkSize = 4f;
	Vector3 bulkScale;
	public float timeToBulk = .2f;
	public float bulkDuration = 5f;
	[RangeAttribute(0, 1)]
	public float bulkSpeedPercentage = 0.2f;
	*/

	LivingEntity ownerEntity;

	//	Zhonya Special
	public Zhonya zhonya;

	//	Reflection Special
	public GameObject reflectionSpecial;

	string collectableTag = "Collectable";

	void Start(){

		ownerEntity = GetComponent<LivingEntity>();

		zhonya = GetComponent<Zhonya>();

		//bulkScale = new Vector3(bulkSize, bulkSize, bulkSize);

	}

	void Update(){

		ownerEntity.actualState = zhonya.IsZhonyaActive()?LivingEntity.State.Zhonya:LivingEntity.State.None;

	}

	public void UsePowerup(){
		
		switch (actualPowerup){

			/*case Powerups.Bulk:
				
				StartCoroutine(Bulk());

				actualPowerup = Powerups.None;
				break;
			*/

			case Powerups.Zhonya:

				ActivateZhonya();
				break;

			case Powerups.Reflection:
				GameObject specialInstance = Instantiate(reflectionSpecial, transform.position, Quaternion.identity) as GameObject;
				SpecialController specialController = specialInstance.GetComponent<SpecialController>();
				specialController.caster = this.gameObject;
				AudioManager.instance.PlaySound("Force");
				ResetPowerup();
				break;

			case Powerups.None:
				break;

		}

	}

	public void ActivateZhonya(){
		
		ResetPowerup();
		zhonya.ActivateZhonya();

	}

	public int GetPowerup(){
		return (int)actualPowerup;
	}

	void ResetPowerup(){
		actualPowerup = Powerups.None;
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
}