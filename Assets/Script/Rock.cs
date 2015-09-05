using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {
	public bool isPushing = false, isPulling = false;

	// Update is called once per frame
	void Update () {
		if(isPushing){
			PushMove();
		}
		else if(isPulling){
			PullMove();
		}
	}

	public void Push(float playerX, float playerZ){
		int pX = Mathf.FloorToInt(playerX), pZ = Mathf.FloorToInt(playerZ),
		rX = Mathf.FloorToInt(transform.position.x), rZ = Mathf.FloorToInt(transform.position.z);

		isPushing = true;
	}

	public void Pull(){
		isPulling = true;
	}

	void PushMove(){

	
	}

	void PullMove(){
	
	}
}
