using UnityEngine;
using System.Collections;

public class CamControl : MonoBehaviour {


    public Transform target;

    Vector3 deltaVec;
	// Use this for initialization
	void Start () {
        deltaVec = target.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float sqrDistance = (target.position - transform.position).sqrMagnitude;
        if (Mathf.Abs(sqrDistance - deltaVec.sqrMagnitude) > 0.05f) {
            SmoothMoveToPos(target.position - deltaVec, 1.5f);
            //transform.position = target.position - deltaVec;
        }
	}

    void SmoothMoveToPos(Vector3 pos, float speed) {
        Vector3 newPos = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
        transform.position = newPos;
    }
}
