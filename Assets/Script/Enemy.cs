using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public GameObject player; 


	public Grid gird1;
	//enemy's speed
	public float speed;

	public bool falling;
	public float fallingTime;
	public float dyingTime;

	//check enemy's health
	public float enemyHealth;
	public float damageTime;


	//check player in sight or not
	public bool playerInSight;
	public bool isDied;

	public float interval;
	public int sizeX;
	public int sizeZ;
	public bool[,] visitedGrid;
	public int [,] direction;
	public int [,] searchDirection;
	public int [,] searchQueue;
	public int [,] obstacles;//障礙物

	public float updateTime;
	public float currentTime;

	public Vector3 targetPosition;
	//temp move position
	public Vector3 tempPosition;

	public Vector3 gridPosition;
	public Vector3 playerPosition;
	public Vector3 faceDirection;

	public Vector3[] boxPosition;
	public Vector3 []possiblePositions;
	public int sizeOfPositions;

	public int animationState;//0:Idle,1:damaged,2:die
	public float animationTime;

	public bool firstUsed;
	// Use this for initialization
	void Start () {
		dyingTime = 3.0f;
		falling = false;
		fallingTime = 2.0f;

		currentTime = 0.0f;
		updateTime = 1.0f;
		firstUsed = true;

		speed = 1.0f;
		enemyHealth = 10.0f;
		damageTime = 0.0f;
		playerInSight = false;
		isDied = false;
		direction = new int[4, 2];

		interval = 1;
		sizeX = GameManager.instance.gridSizeX;
		sizeZ = GameManager.instance.gridSizeZ;
		visitedGrid = new bool[sizeX, sizeZ];
		searchDirection = new int[sizeX, sizeZ];
		obstacles = new int[sizeX, sizeZ];
		searchQueue = new int[sizeX*sizeZ,2];
		possiblePositions = new Vector3[sizeX * sizeZ];
		boxPosition = new Vector3[sizeX * sizeZ];

		direction [0,0] = -1;
		direction [0,1] = 0;

		direction [1,0] = 0;
		direction [1,1] = 1;

		direction [2,0] = 1;
		direction [2,1] = 0;

		direction [3,0] = 0;
		direction [3,1] = -1;

		//set random TargetPosition
		targetPosition = new Vector3 (3.0f, 0, 3.0f);
		gridPosition = gameObject.transform.position;
		gridPosition.y = 0;
		//initialize
		//tempPosition = targetPosition;

		animationState = 0;
		animationTime = 0.0f;

		tempPosition = new Vector3 (0, 0, 0);
		faceDirection = new Vector3 (0, 0, 0);

	}
	
	// Update is called once per frame
	void Update () {
		if (isDied == true) {
			Destroy(gameObject,3.0f);
			gameObject.GetComponent<Collider>().enabled = false;
		}
		//掉落
		if(falling)
		{

			fallingTime-=Time.deltaTime;
			faceDirection = new Vector3(0,0,0);
			if(fallingTime<=0.0)
			{
				Destroy(gameObject);
			}
		}





		animationTime -= Time.deltaTime;
		if (animationTime <= 0.0f) {
			animationTime = 0.0f;
			animationState = 0;
		}

		switch (animationState) {
		case 0:

			break;
		case 1:

			break;
		case 2:

			break;
		}

		//DebugLogger.Log ("Update");
		//update obstacles position
		//currentTime += Time.deltaTime;
		//if (currentTime > updateTime) {
			//currentTime = 0.0f;
			UpdateObstacles ();
			CalGridPosition ();
		//}
		damageTime -= Time.deltaTime;
		if (damageTime <= 0.0f)
			damageTime = 0.0f;
		//if enemy dies destroy
		if (enemyHealth <= 0.0f) {
			isDied = true;
			//Destroy (gameObject);
		}

		if (playerInSight == true)
			Attack ();
		else 
			Patrol ();

		if (isDied==true) {
			//do Die thing
			Destroy(gameObject);
		}

	}

	//Update Obstacles Position
	void UpdateObstacles()
	{
		GameObject [] boxObject = GameObject.FindGameObjectsWithTag ("Rock");
		//DebugLogger.Log (boxObject.Length);
		int size = 0;
		for (int i = 0; i<boxObject.Length; i++) {
			boxPosition[i] = boxObject[i].transform.position;
			size = i;
		}

		//update box Position
 		for (int i = 0; i<sizeX; i++)
			for (int j = 0; j<sizeZ; j++)
				obstacles [i, j] = 0;
		for(int i = 0;i<=size;i++)
		{
			int X = (int)( boxPosition[i].x);
			int Z = (int)( boxPosition[i].z);
			obstacles[X,Z] = 1;
		}
		sizeOfPositions = 0;
		for (int i = 0; i<sizeX; i++)
			for (int j = 0; j<sizeZ; j++)
				if (obstacles [i, j] == 0) {
				possiblePositions[sizeOfPositions].x = i;
				possiblePositions[sizeOfPositions].y = 0;
				possiblePositions[sizeOfPositions].z = j;
				sizeOfPositions++;
			}
	
			int indexP = Random.Range (0, sizeOfPositions);
			targetPosition = possiblePositions [indexP];
			targetPosition.y = 0;

	}

	void CalGridPosition()
	{
		Vector3 pos = gameObject.transform.position;

		int X = (int)(pos.x+0.5f);
		int Z = (int)(pos.z+0.5f);

		gridPosition.x = (float)X;
		gridPosition.z = (float)Z;
		if (X < 0 || X >= sizeX || Z < 0 || Z >= sizeZ) {
			isDied = true;
			falling = true;
			return ;
		}
		if (obstacles [X,Z] == 2)
			isDied = true;
	}


	//Attack
	void Attack()
	{
		if (firstUsed) {
			findTarget (playerPosition.x, playerPosition.z);
			firstUsed = false;
		}
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
			CalGridPosition();
			//DebugLogger.Log ("NO");
			
		}
		WalkGrid ();

	}

	//巡邏
	void Patrol()
	{
		if (firstUsed) {
			findTarget (playerPosition.x, playerPosition.z);
			firstUsed = false;
		}
		Vector3 diffFromTarget = targetPosition - gameObject.transform.position;
		diffFromTarget.y = 0;
		if (diffFromTarget.magnitude <= 0.1) {
			//DebugLogger.Log (targetPosition);
			//DebugLogger.Log (gridPosition);
			//DebugLogger.Log (diffFromTarget);
			return;
		} 




		Vector3 diffTemp = tempPosition - gameObject.transform.position;
		diffTemp.y = 0;
		if (diffTemp.magnitude <= 0.1) {

			findTarget(targetPosition.x,targetPosition.z);
			//gridPosition = tempPosition;
			CalGridPosition();
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
		int targetX = (int)(x);
		int targetZ = (int)(z);

		//Enemy X,Z
		int sourceX=(int)(gridPosition.x);
		int sourceZ=(int)(gridPosition.z);

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

			for(int i = 0;i<sizeX;i++)
				for(int j = 0;j<sizeZ;j++)
				if(visitedGrid[i,j]==true){
					targetX = i;
					targetZ = j;
					targetPosition.x = (float)targetX;
					targetPosition.z = (float)targetZ;
				}
			return ;
		
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
		if (tempPosition.x < 0) {
			tempPosition.x *= -1.0f;
			dirIndex = (dirIndex+2)%4;
		}
		if(tempPosition.z<0)
			tempPosition.z*=-1.0f;
		faceDirection = new Vector3 ((float)direction [dirIndex, 0], 0, (float)direction [dirIndex, 1]);
		faceDirection.y = 0;
		tempPosition.y = gameObject.transform.position.y;
		gameObject.transform.LookAt (tempPosition);
		//DebugLogger.Log (tempPosition);
		//DebugLogger.Log (faceDirection);
	}

	//Take Damage
	public void TakeDamage()
	{
		if (damageTime <= 0.1f) {
			enemyHealth -= 5.0f;
			damageTime = 1.0f;
			animationState = 1;
			animationTime = 1.0f;
		}
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.tag == "Player")
		{
			playerInSight = true;
			playerPosition = other.gameObject.transform.position;
			//DebugLogger.Log("SeeStay!!");
			//DebugLogger.Log(playerPosition);
		}
			
	}

	//Check if enemy seeing player
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player")
		{
			playerInSight = true;
			playerPosition = other.gameObject.transform.position;
			//DebugLogger.Log("See!!");
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
