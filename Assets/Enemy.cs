using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public GameObject player; 
	public float speed;
	public float enemyHealth;
	public bool playerInSight;
	public Vector3 targetPosition;
	public bool[,] visitedGrid;
	public int [,] direction;
	public int [,] searchDirection;

	// Use this for initialization
	void Start () {
		speed = 1.0f;
		enemyHealth = 10.0f;
		playerInSight = false;
		direction = new int[4, 2];

		direction [0,0] = -1;
		direction [0,1] = 0;

		direction [1,0] = 0;
		direction [1,1] = 1;

		direction [2,0] = 1;
		direction [2,1] = 0;

		direction [3,0] = 0;
		direction [3,1] = -1;

	}
	
	// Update is called once per frame
	void Update () {
		//if enemy dies destroy
		if (enemyHealth <= 0.0f)
			Destroy (gameObject);

		//
		if (playerInSight == true)
			Attack ();
		else
			Patrol ();

	}


	//Attack
	void Attack()
	{
		//chase player

	}
	//巡邏
	void Patrol()
	{
		//walk to target position

	}

	void WalkGrid()
	{
		Quaternion dir = gameObject.transform.rotation;

	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject == player)
		{
			playerInSight = false;
		}
			
	}

}
