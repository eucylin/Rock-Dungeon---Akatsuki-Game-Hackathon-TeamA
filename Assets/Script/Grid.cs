using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public int sizeX, sizeZ, nodeInterval = 1;
	public GameObject nodePrefab;

	public static Node [,] nodes;

	void Start(){
		CreateGrid();
	}

	bool CreateGrid(){
		nodes = new Node[sizeX, sizeZ];

		if(sizeX > 0 && sizeZ > 0){
			for(int i = 0; i < sizeX; i += nodeInterval){
				for(int j = 0; j < sizeZ; j += nodeInterval){
					GameObject obj;

					obj = (GameObject)Instantiate(nodePrefab, new Vector3(i, 0.0f, j), Quaternion.identity);
					nodes[i, j] = obj.GetComponent<Node>();
					nodes[i, j].indexX = i;
					nodes[i, j].indexZ = j;
				}
			}
			return true;
		}
		else{
			DebugLogger.LogError("Grid : CreateGrid failed! sizeX or sizeY isn't setted well.");
			return false;
		}
	}
}
