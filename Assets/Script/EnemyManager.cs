using UnityEngine;
using System.Collections;
public class EnemyManager : MonoBehaviour
{
	 
	public GameObject enemy;                // The enemy prefab to be spawned.
	public float spawnTime = 3.0f;            // How long between each spawn.
	//public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
	public bool[] blocked;
	public Vector3[] spawnPoints;  
	public float currentTime;

	public Vector3[] boxPosittions;
	public int[,] obstacles;
	public int sizeX;
	public int sizeZ;



	void Start ()
	{
		sizeX = 6;
		sizeZ = 6;
		obstacles = new int[sizeX, sizeZ];
	
		currentTime = 2.0f;
		for (int i = 0; i<blocked.Length; i++)
			blocked [i] = false;
		//Spawn();

		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		//InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	
	void Update()
	{
		currentTime += Time.deltaTime;
		if (currentTime >= spawnTime) {
			currentTime = 0.0f;
			updateBoxPosition();
			Spawn();

		}
	}


	void Spawn ()
	{
		for (int i = 0; i<spawnPoints.Length; i++) {
			int X = (int)spawnPoints[i].x;
			int Z = (int)spawnPoints[i].z;
			if(obstacles[X,Z]==1)
			{
				blocked[i] = true;
			}
		}
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		Instantiate (enemy, new Vector3(0,1.0f,0), Quaternion.identity);

	}

	void updateBoxPosition()
	{
		for (int i = 0; i<sizeX; i++)
			for (int j = 0; j<sizeZ; j++)
				obstacles [i, j] = 0;
		for (int i = 0; i<boxPosittions.Length; i++) {
			int X = (int)boxPosittions[i].x;
			int Z = (int)boxPosittions[i].z;
			obstacles[X,Z] = 1;

		}


	}
}