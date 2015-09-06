using UnityEngine;
using System.Collections;
public class EnemyManager : MonoBehaviour
{
	public GameObject spawner; 
	public GameObject enemy;                // The enemy prefab to be spawned.
	public float spawnTime = 5.0f;            // How long between each spawn.
	//public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
	public bool[] blocked;
	public Vector3[] spawnPoints;  
	public float currentTime;

	public Vector3[] boxPosittions;
	public int[,] obstacles;
	public int sizeX;
	public int sizeZ;

	public int maxNumEnemy;

	void Start ()
	{
		maxNumEnemy = 10;
		sizeX = 6;
		sizeZ = 6;
		boxPosittions = new Vector3[sizeX * sizeZ];
		obstacles = new int[sizeX, sizeZ];
		blocked = new bool[spawnPoints.Length];
		currentTime = 2.0f;
		for (int i = 0; i<blocked.Length; i++)
			blocked [i] = false;
		for (int i = 0; i<spawnPoints.Length; i++) {
			Quaternion quate = Quaternion.identity;
		
				quate.eulerAngles = new Vector3(90, 0, 0);
			Instantiate (spawner, new Vector3(spawnPoints [i].x,0.5f,spawnPoints[i].z), quate);
		
		}//Spawn();

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
		GameObject [] EObject = GameObject.FindGameObjectsWithTag ("Enemy");
		int size = EObject.Length;
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		for (int i = 0; i<spawnPoints.Length; i++) {
			if(blocked[i]==false&&size<maxNumEnemy)
				Instantiate (enemy, spawnPoints[i], Quaternion.identity);
			//Instantiate (enemy, new Vector3 (0, 1.0f, 0), Quaternion.identity);
		}
	}

	void updateBoxPosition()
	{
		GameObject [] boxObject = GameObject.FindGameObjectsWithTag ("Rock");
		int size = 0;
		for (int i = 0; i<boxObject.Length; i++) {
			boxPosittions[i] = boxObject[i].transform.position;
			size = i;
		}
		
		//update box Position
		for (int i = 0; i<sizeX; i++)
			for (int j = 0; j<sizeZ; j++)
				obstacles [i, j] = 0;
		for(int i = 0;i<=size;i++)
		{
			int X = (int)( boxPosittions[i].x);
			int Z = (int)( boxPosittions[i].z);
			obstacles[X,Z] = 1;
		}
		/*
		for (int i = 0; i<sizeX; i++)
			for (int j = 0; j<sizeZ; j++)
				obstacles [i, j] = 0;
		for (int i = 0; i<boxPosittions.Length; i++) {
			int X = (int)boxPosittions[i].x;
			int Z = (int)boxPosittions[i].z;
			obstacles[X,Z] = 1;

		}
		*/

	}
}