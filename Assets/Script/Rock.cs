using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {
	public bool isPushing = false, isPulling = false;
	int dirX, dirZ, targetX, targetZ, forceCoef;

	// Update is called once per frame
	void Update () {
		if(isPushing){
			PushMove();
		}
		else if(isPulling){
			PullMove();
		}
	}

	public void Push(float playerX, float playerZ, int force = 1){
		int pX = Mathf.FloorToInt(playerX), pZ = Mathf.FloorToInt(playerZ),
		rX = Mathf.FloorToInt(transform.position.x), rZ = Mathf.FloorToInt(transform.position.z);

		dirX = rX - pX;
		dirZ = rZ - pZ;
		if(dirX != 0 ^ dirZ != 0){
			targetX = dirX * force + rX;
			targetZ = dirZ * force + rZ;
			forceCoef = force;
			isPushing = true;
		}
	}

	public void Pull(){
		isPulling = true;
	}

	void PushMove(){
		if(Mathf.FloorToInt(transform.position.x) != targetX){
			transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, targetX, Time.deltaTime * forceCoef), transform.position.y, transform.position.z);
		}
		else if(Mathf.FloorToInt(transform.position.z) != targetZ){
			transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.MoveTowards(transform.position.z, targetZ, Time.deltaTime * forceCoef));
		}
	}

	void PullMove(){
	
	}
}
