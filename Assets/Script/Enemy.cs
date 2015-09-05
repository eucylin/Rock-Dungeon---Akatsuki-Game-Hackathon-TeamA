using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public GameObject player; 

	public Grid gird1;
	//enemy's speed
	public float speed;

	public Vector3 faceDirection;
	//check enemy's health
	public float enemyHealth;

	//check player in sight or not
	public bool playerInSight;
	public Vector3 targetPosition;
	public bool[,] visitedGrid;
	public int [,] direction;
	public int [,] searchDirection;
	public int [,] searchQueue;
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
		//walk to target position
		findTarget ();
		WalkGrid ();
		DebugLogger.Log ("Patrol");

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
		int targetX = 3;
		int targetZ = 3;

		//Enemy X,Z
		int sourceX=0;
		int sourceZ=0;


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
					if(moveX==targetX&&moveZ==targetZ)
					{
						qf = qe+1;
						break;
					}
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
			dirIndex = (index+2)%2;

		}

		Vector3 tempPosition = new Vector3 ((float)direction [dirIndex,0], 0, (float)direction [dirIndex,1]);
		faceDirection = tempPosition - gameObject.transform.position;
		faceDirection.y = 0;
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
