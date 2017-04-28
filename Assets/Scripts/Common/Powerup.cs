using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Zhonya))]
[RequireComponent(typeof(Bulk))]
public class Powerup : MonoBehaviour {

	//	Jogar isso pro game controller
	public enum Powerups {None = -1, Zhonya = 0, Reflection = 1, Bulk = 2};
	Powerups actualPowerup = Powerups.None;

	LivingEntity ownerEntity;

	//	Zhonya Special
	public Zhonya zhonya;
	public Bulk bulk;

	//	Reflection Special
	public GameObject reflectionSpecial;

	string collectableTag = "Collectable";

	void Start(){

		ownerEntity = GetComponent<LivingEntity>();

		zhonya = GetComponent<Zhonya>();
		bulk = GetComponent<Bulk>();

	}

	void Update(){

		ownerEntity.actualState = zhonya.IsZhonyaActive()?LivingEntity.State.Zhonya:LivingEntity.State.None;

	}

	public void UsePowerup(){
		
		switch (actualPowerup){

			case Powerups.Zhonya:

				ActivateZhonya();
				break;

			case Powerups.Reflection:

				ActivateReflection();
				break;

			case Powerups.Bulk:
				
				ActivateBulk();
				break;

			case Powerups.None:
				break;

		}

	}

	public void ActivateZhonya(){
		
		zhonya.ActivateZhonya();
		ResetPowerup();

	}

	public void ActivateReflection(){

		GameObject specialInstance = Instantiate(reflectionSpecial, transform.position, Quaternion.identity) as GameObject;
		SpecialController specialController = specialInstance.GetComponent<SpecialController>();
		specialController.caster = this.gameObject;
		AudioManager.instance.PlaySound("Force");
		ResetPowerup();

	}

	public void ActivateBulk(){
		
		bulk.ActivateBulk();
		ResetPowerup();

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
}