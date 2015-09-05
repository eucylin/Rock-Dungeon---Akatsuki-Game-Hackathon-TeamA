using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public int sizeX, sizeZ, nodeInterval = 1;
	public GameObject nodePrefab, nodeHolderPrefab;
	public Node [,] nodes;
	
	public bool CreateGrid(){
		Transform nodeHolder;

		nodes = new Node[sizeX, sizeZ];

		if(sizeX > 0 && sizeZ > 0){
			nodeHolder = Instantiate(nodeHolderPrefab).transform;
			nodeHolder.name += sizeX.ToString() + "x" + sizeZ.ToString();

			for(int i = 0; i < sizeX; i += nodeInterval){
				for(int j = 0; j < sizeZ; j += nodeInterval){
					GameObject obj;

					obj = (GameObject)Instantiate(nodePrefab, new Vector3(i, 0.0f, j), Quaternion.identity);
					obj.transform.parent = nodeHolder;
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

	void DeleteGrid(){
		if(nodes != null){
			for(int i = 0; i < sizeX; i += nodeInterval){
				for(int j = 0; j < sizeZ; j += nodeInterval){
					DestroyImmediate(nodes[i, j].gameObject);
				}
			}
		}
	}
}
