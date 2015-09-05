using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {
	public bool isPushing = false, isPulling = false;
	public float offset = 0.5f, speedCoef;
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
		int pX = Mathf.FloorToInt(playerX + offset), pZ = Mathf.FloorToInt(playerZ + offset),
		rX = Mathf.FloorToInt(transform.position.x + offset), rZ = Mathf.FloorToInt(transform.position.z + offset);

		dirX = rX - pX;
		dirZ = rZ - pZ;
		if(dirX != 0 ^ dirZ != 0){
			targetX = dirX * force + rX;
			targetZ = dirZ * force + rZ;
			forceCoef = force;
			isPushing = true;
		}
		DebugLogger.Log("Pushing rock! " + "px = " + pX + ", pz = " + pZ + ", targetX = " + targetX + ", targetZ = " + targetZ);
	}

	public void Pull(){
		isPulling = true;
	}

	void PushMove(){
		if(Mathf.Abs(transform.position.x - targetX) > Mathf.Epsilon){
			transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, targetX, Time.deltaTime * forceCoef * speedCoef), transform.position.y, transform.position.z);
		}
		else if(Mathf.Abs(transform.position.z - targetZ) > Mathf.Epsilon){
			transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.MoveTowards(transform.position.z, targetZ, Time.deltaTime * forceCoef * speedCoef));
		}
		else{
			isPushing = false;
		}
	}

	void PullMove(){
	
	}
}
