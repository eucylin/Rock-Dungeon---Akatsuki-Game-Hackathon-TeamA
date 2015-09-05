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


	void Start ()
	{
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
			Spawn();

		}
	}
	void Spawn ()
	{
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		Instantiate (enemy, new Vector3(0,1.0f,0), Quaternion.identity);
	}
}