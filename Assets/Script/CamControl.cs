using UnityEngine;
using System.Collections;

public class CamControl : MonoBehaviour {


    public Transform target;

    Vector3 deltaPos;
	// Use this for initialization
	void Start () {
        deltaPos = target.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = target.position - deltaPos;
	}
}
