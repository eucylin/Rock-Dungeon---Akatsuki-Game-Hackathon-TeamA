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

			//limit target position
			if(targetX < 0){
				targetX = 0;
			}
			else if(targetX >= GameManager.instance.gridSizeX){
				targetX = GameManager.instance.gridSizeX - 1;
			}
			if(targetZ < 0){
				targetZ = 0;
			}
			else if(targetZ >= GameManager.instance.gridSizeZ){
				targetZ = GameManager.instance.gridSizeZ - 1;
			}

			//Obstacle block
			if(dirX == 0){
				foreach(GameObject rock in GameObject.FindGameObjectsWithTag("Rock")){
					if(rock != gameObject && Mathf.FloorToInt(rock.transform.position.x) == targetX){
						if(dirZ > 0){
							if(rock.transform.position.z - rZ >= targetZ - rZ){
								targetZ = Mathf.FloorToInt(rock.transform.position.z) - 1;
							}
						}
						else{
							if(rZ - rock.transform.position.z >= rZ - targetZ){
								targetZ = Mathf.FloorToInt(rock.transform.position.z) + 1;
							}
						}
					}
				}
			}
			else if(dirZ == 0){
				foreach(GameObject rock in GameObject.FindGameObjectsWithTag("Rock")){
					if(rock != gameObject && Mathf.FloorToInt(rock.transform.position.z) == targetZ){
						if(dirX > 0){
							if(rock.transform.position.x - rX >= targetX - rX){
								targetX = Mathf.FloorToInt(rock.transform.position.x) - 1;
							}
						}
						else{
							if(rX - rock.transform.position.x >= rX - targetX){
								targetX = Mathf.FloorToInt(rock.transform.position.x) + 1;
							}
						}
					}
				}
			}

			forceCoef = force;
			isPushing = true;
		}
		DebugLogger.Log("Pushing rock! " + "px = " + pX + ", pz = " + pZ + ", targetX = " + targetX + ", targetZ = " + targetZ);
	}

	public void Pull(float playerX, float playerZ){
		int force = 1;

		int pX = Mathf.FloorToInt(playerX + offset), pZ = Mathf.FloorToInt(playerZ + offset),
		rX = Mathf.FloorToInt(transform.position.x + offset), rZ = Mathf.FloorToInt(transform.position.z + offset);
		
		dirX = rX - pX;
		dirZ = rZ - pZ;
		if(dirX != 0 ^ dirZ != 0){
			targetX = dirX * -force + rX;
			targetZ = dirZ * -force + rZ;
			
			//limit target position
			if(targetX < 1){
				if(pX == 0 && rX != 0)
					targetX = 1;
			}
			else if(targetX > GameManager.instance.gridSizeX - 2){
				if(pX == GameManager.instance.gridSizeX - 1 && rX != GameManager.instance.gridSizeX - 1)
					targetX = GameManager.instance.gridSizeX - 2;
			}
			if(targetZ < 1){
				if(pZ == 0 && rZ != 0)
					targetZ = 1;
			}
			else if(targetZ > GameManager.instance.gridSizeZ - 2){
				if(pZ == GameManager.instance.gridSizeZ - 1 && rZ != GameManager.instance.gridSizeZ - 1)
					targetZ = GameManager.instance.gridSizeZ - 2;
			}
			
			//Obstacle block
			if(dirX == 0){
				foreach(GameObject rock in GameObject.FindGameObjectsWithTag("Rock")){
					if(rock != gameObject && Mathf.FloorToInt(rock.transform.position.x) == targetX){
						if(dirZ > 0){
							if(rock.transform.position.z < targetZ && (targetZ - Mathf.FloorToInt(rock.transform.position.z)== 1)){
								targetZ += 1;
							}
						}
						else{
							if(rock.transform.position.z > targetZ && (Mathf.FloorToInt(rock.transform.position.z) - targetZ == 1)){
								targetZ -= 1;
							}
						}
					}
				}
			}
			else if(dirZ == 0){
				foreach(GameObject rock in GameObject.FindGameObjectsWithTag("Rock")){
					if(rock != gameObject && Mathf.FloorToInt(rock.transform.position.z) == targetZ){
						if(dirX > 0){
							if(rock.transform.position.x < targetX && (targetX - Mathf.FloorToInt(rock.transform.position.x) == 1)){
								targetX += 1;
							}
						}
						else{
							if(rock.transform.position.x > targetX && (Mathf.FloorToInt(rock.transform.position.x) - targetX == 1)){
								targetX -= 1;
							}
						}
					}
				}
			}
			
			forceCoef = force;
			isPulling = true;
		}
		DebugLogger.Log("Pulling rock! " + "px = " + pX + ", pz = " + pZ + ", targetX = " + targetX + ", targetZ = " + targetZ);

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
		if(Mathf.Abs(transform.position.x - targetX) > Mathf.Epsilon){
			transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, targetX, Time.deltaTime * forceCoef * speedCoef), transform.position.y, transform.position.z);
		}
		else if(Mathf.Abs(transform.position.z - targetZ) > Mathf.Epsilon){
			transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.MoveTowards(transform.position.z, targetZ, Time.deltaTime * forceCoef * speedCoef));
		}
		else{
			isPulling = false;
		}
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Enemy"){
			if(isPushing){
				col.gameObject.GetComponent<Enemy>().TakeDamage();
				col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(dirX * forceCoef, 0.0f, dirZ * forceCoef), ForceMode.Impulse);
			}
		}
	}
}
