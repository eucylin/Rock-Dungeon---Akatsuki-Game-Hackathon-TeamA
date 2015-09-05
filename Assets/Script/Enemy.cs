using UnityEngine;
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

	public bool[,] visitedGrid;
	public int [,] direction;
	public int [,] searchDirection;
	public int [,] searchQueue;

	public Vector3 targetPosition;
	//temp move position
	public Vector3 tempPosition;

	public Vector3 gridPosition;
	public Vector3 playerPosition;
	public Vector3 faceDirection;




	// Use this for initialization
	void Start () {
		speed = 1.0f;
		enemyHealth = 10.0f;
		playerInSight = false;
		direction = new int[4, 2];

		visitedGrid = new bool[6, 6];
		searchDirection = new int[6, 6];
		searchQueue = new int[6*6,2];

		direction [0,0] = -1;
		direction [0,1] = 0;

		direction [1,0] = 0;
		direction [1,1] = 1;

		direction [2,0] = 1;
		direction [2,1] = 0;

		direction [3,0] = 0;
		direction [3,1] = -1;

		//set TargetPosition
		targetPosition = new Vector3 (3.0f, 0, 3.0f);
		gridPosition = gameObject.transform.position;
		gridPosition.y = 0;
		//initialize
		//tempPosition = targetPosition;

	}
	
	// Update is called once per frame
	void Update () {
		//if enemy dies destroy
		if (enemyHealth <= 0.0f)
			Destroy (gameObject);


		if (playerInSight == true)
			Attack ();
		else
			Patrol ();

	}


	//Attack
	void Attack()
	{
		//chase player
		Vector3 playerPosition = player.transform.position;



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

			findTarget();
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

	void findTarget()
	{
		//Target
		int targetX = (int)targetPosition.x;
		int targetZ = (int)targetPosition.z;

		//Enemy X,Z
		int sourceX=(int)gridPosition.x;
		int sourceZ=(int)gridPosition.z;

		//DebugLogger.Log (sourceX);
		//DebugLogger.Log (sourceZ);
		//DebugLogger.Log (targetX);
		//DebugLogger.Log (targetZ);

		for (int i = 0; i<6; i++)
			for (int j = 0; j<6; j++) {
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
				if(moveX>=0&&moveX<6&&moveZ>=0&&moveZ<6&&visitedGrid[moveX,moveZ]==false)
				{
					visitedGrid[moveX,moveZ] = true;
					searchDirection[moveX,moveZ] = (i+2)%4;
					searchQueue[qe,0] = moveX;
					searchQueue[qe,1] = moveZ;
					qe++;
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
		if (other.gameObject == player)
		{
			playerInSight = true;
		}
			
	}

	//Check if enemy seeing player
	void OnTriggerEnter (Collider other) {
		if (other.gameObject == player)
		{
			playerInSight = true;
		}
			
	}

	//Check if enemy seeing player
	void OnTriggerExit (Collider other) {
		if (other.gameObject == player)
		{
			playerInSight = false;
		}
			
	}
}
