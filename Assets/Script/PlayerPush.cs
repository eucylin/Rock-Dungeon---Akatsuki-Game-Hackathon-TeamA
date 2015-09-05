using UnityEngine;
using System.Collections;

public class PlayerPush : MonoBehaviour {
	public int pushForce = 1;
	Rock rockScript;
	bool isTouchingRock = false;

	void Update(){
		if(Input.GetKey(KeyCode.W)){
			transform.position += Vector3.forward * Time.deltaTime * 3f;
		}
		if(Input.GetKey(KeyCode.A)){
			transform.position += Vector3.left * Time.deltaTime * 3f;
		}
		if(Input.GetKey(KeyCode.S)){
			transform.position += Vector3.back * Time.deltaTime * 3f;
		}
		if(Input.GetKey(KeyCode.D)){
			transform.position += Vector3.right * Time.deltaTime * 3f;
		}

		if(Input.GetKeyDown(KeyCode.Space)){
			if(isTouchingRock && rockScript != null){
				rockScript.Push(transform.position.x, transform.position.z, pushForce);
			}
		}
	}
	
	void OnTriggerStay(Collider col){
		if(col.gameObject.tag == "Rock"){
			if(rockScript == null){
				rockScript = col.gameObject.GetComponent<Rock>();
			}
			isTouchingRock = true;
		}
	}

	void OnTriggerExit(Collider col){
		if(col.gameObject.tag == "Rock"){
			rockScript = null;
			isTouchingRock = false;
		}
	}
}
