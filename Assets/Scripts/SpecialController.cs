using UnityEngine;
using System.Collections;

public class SpecialController : MonoBehaviour {

	public float specialMaxSize;
	public float specialSpeed;
	public float specialCooldown;
	public float power;
	public GameObject caster;

	void Start () {

		StartSpecial();

	}

	IEnumerator SpecialAttack(){
		float timer = 0;


		while(specialMaxSize > transform.localScale.x){

			transform.localScale += new Vector3(1, 1, 1) * Time.fixedDeltaTime * specialSpeed;
			timer += Time.fixedDeltaTime;
			yield return null;
		}

		Destroy(gameObject);
		
	}

	public void StartSpecial(){
		StartCoroutine(SpecialAttack());
	}

	
	void OnTriggerEnter(Collider collider){

		string tag = collider.gameObject.tag;

		if (tag == "Enemy" || tag == "Player"){

			if(collider.gameObject != caster) {
				CameraShaker.Shake(0.2f, 0.2f);
				Rigidbody rb = collider.GetComponent<Rigidbody>();
				Vector3 direction = (collider.transform.position - transform.position).normalized;
				Vector3 knockback = direction * power;
				rb.AddForce(knockback, ForceMode.Impulse);
			}
		}
	}
}
