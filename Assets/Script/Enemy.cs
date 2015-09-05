﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public GameObject player; 


	public Grid gird1;
	//enemy's speed
	public float speed;

	//check enemy's health
	public float enemyHealth;

	//check player in sight or not
	public bool playerInSight;


	public int sizeX;
	public int sizeZ;
	public bool[,] visitedGrid;
	public int [,] direction;
	public int [,] searchDirection;
	public int [,] searchQueue;
	public int [,] obstacles;//障礙物


	public Vector3 targetPosition;
	//temp move position
	public Vector3 tempPosition;

	public Vector3 gridPosition;
	public Vector3 playerPosition;
	public Vector3 faceDirection;

	public Vector3[] boxPosition;


	// Use this for initialization
	void Start () {
		speed = 1.0f;
		enemyHealth = 10.0f;
		playerInSight = false;
		direction = new int[4, 2];
		sizeX = 6;
		sizeZ = 6;
		visitedGrid = new bool[sizeX, sizeZ];
		searchDirection = new int[sizeX, sizeZ];
		obstacles = new int[sizeX, sizeZ];
		searchQueue = new int[sizeX*sizeZ,2];

		direction [0,0] = -1;
		direction [0,1] = 0;

		direction [1,0] = 0;
		direction [1,1] = 1;

		direction [2,0] = 1;
		direction [2,1] = 0;

		direction [3,0] = 0;
		direction [3,1] = -1;

		//set TargetPosition
		targetPosition = new Vector3 (4.0f, 0, 1.0f);
		gridPosition = gameObject.transform.position;
		gridPosition.y = 0;
		//initialize
		//tempPosition = targetPosition;

	}
	
	// Update is called once per frame
	void Update () {
		//update obstacles position
		UpdateObstacles ();

		//if enemy dies destroy
		if (enemyHealth <= 0.0f)
			Destroy (gameObject);


		if (playerInSight == true)
			Attack ();
		else
			Patrol ();

	}

	//Update Obstacles Position
	void UpdateObstacles()
	{
		//update box Position
 		for (int i = 0; i<sizeX; i++)
			for (int j = 0; j<sizeZ; j++)
				obstacles [i, j] = 0;
		for(int i = 0;i<boxPosition.Length;i++)
		{
			int X = (int)boxPosition[i].x;
			int Z = (int)boxPosition[i].z;
			obstacles[X,Z] = 1;
		}
		obstacles [2, 1] = 1;
	}

	//Attack
	void Attack()
	{
		//chase player
		//playerPosition = player.transform.position;
		Vector3 diffFromTarget = playerPosition - gameObject.transform.position;
		diffFromTarget.y = 0;
		if (diffFromTarget.magnitude <= 0.1) {

			return;
		} 
		Vector3 diffTemp = tempPosition - gameObject.transform.position;
		diffTemp.y = 0;
		if (diffTemp.magnitude <= 0.1) {
			
			findTarget(playerPosition.x,playerPosition.z);
			gridPosition = tempPosition;
			//DebugLogger.Log ("NO");
			
		}

	}

	//巡邏
	void Patrol()
	{
		Vector3 diffFromTarget = targetPosition - gameObject.transform.position;
		diffFromTarget.y = 0;
		if (diffFromTarget.magnitude <= 0.1) {
			DebugLogger.Log (targetPosition);
			DebugLogger.Log (gridPosition);
			DebugLogger.Log (diffFromTarget);
			return;
		} 




		Vector3 diffTemp = tempPosition - gameObject.transform.position;
		diffTemp.y = 0;
		if (diffTemp.magnitude <= 0.1) {

			findTarget(targetPosition.x,targetPosition.z);
			gridPosition = tempPosition;
			//DebugLogger.Log ("NO");

		}
		//findTarget();
		//DebugLogger.Log (tempPosition);
		WalkGrid ();

		//walk to target position
		//findTarget ();

		//DebugLogger.Log ("Patrol");

	}
	//Walk one Grid
	void WalkGrid()
	{
	
		Vector3 yourDir = gameObject.transform.rotation * Vector3.forward;
		gameObject.transform.position += speed * Time.deltaTime * faceDirection;

	}

	void findTarget(float x,float z)
	{
		//Target
		int targetX = (int)x;
		int targetZ = (int)z;

		//Enemy X,Z
		int sourceX=(int)gridPosition.x;
		int sourceZ=(int)gridPosition.z;

		//DebugLogger.Log (sourceX);
		//DebugLogger.Log (sourceZ);
		//DebugLogger.Log (targetX);
		//DebugLogger.Log (targetZ);

		for (int i = 0; i<sizeX; i++)
			for (int j = 0; j<sizeZ; j++) {
			visitedGrid[i,j] = false;
			}
		visitedGrid [sourceX, sourceZ] = true;
		int qf = 0;
		int qe = 0;
		searchQueue [qf, 0] = sourceX;
		searchQueue [qf, 1] = sourceZ;
		qe++;
		while (qf<qe) {
			int tempX = searchQueue[qf,0];
			int tempZ = searchQueue[qf,1];
			for(int i = 0;i<4;i++)
			{
				int moveX = tempX+direction[i,0];
				int moveZ = tempZ+direction[i,1];

				//check moveX and moveZ
				if(moveX>=0&&moveX<sizeX&&moveZ>=0&&moveZ<sizeZ&&visitedGrid[moveX,moveZ]==false)
				{
					if(obstacles[moveX,moveZ]==0)
					{
						visitedGrid[moveX,moveZ] = true;
						searchDirection[moveX,moveZ] = (i+2)%4;
						searchQueue[qe,0] = moveX;
						searchQueue[qe,1] = moveZ;
						qe++;
					}
					//if(moveX==targetX&&moveZ==targetZ)
					//{
						//qf = qe+1;
						//break;
					//}
				}

			}
			qf++;
		}
		if (visitedGrid [targetX, targetZ] == false) {
			DebugLogger.LogError ("QQ");
		}

		int TempX = targetX;
		int TempZ = targetZ;
		int dirIndex = 0;
		while (!(TempX==sourceX&&TempZ==sourceZ)) {

			int index = searchDirection[TempX,TempZ];

			TempX+=direction[index,0];
	
			TempZ+=direction[index,1];
			dirIndex = (index+2)%4;

		}

		tempPosition = gridPosition + new Vector3 ((float)direction [dirIndex,0], 0, (float)direction [dirIndex,1]);
		faceDirection = new Vector3 ((float)direction [dirIndex, 0], 0, (float)direction [dirIndex, 1]);
		faceDirection.y = 0;
		//DebugLogger.Log (tempPosition);
		//DebugLogger.Log (faceDirection);
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.tag == "Player")
		{
			playerInSight = true;
			playerPosition = other.gameObject.transform.position;
			DebugLogger.Log("SeeStay!!");
		}
			
	}

	//Check if enemy seeing player
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player")
		{
			playerInSight = true;
			playerPosition = other.gameObject.transform.position;
			DebugLogger.Log("See!!");
		}
			
	}

	//Check if enemy seeing player
	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Player")
		{
			playerInSight = false;
		}
			
	}
}
