using UnityEngine;
using System.Collections;

public class KeyboardInput : PlayerInput {

    //Movement
    private KeyCode upKey = KeyCode.W;
    private KeyCode downKey = KeyCode.S;
    private KeyCode leftKey = KeyCode.A;
    private KeyCode rightKey = KeyCode.D;

    //Pull & Push
    private KeyCode pushKey = KeyCode.Space;
    private KeyCode pullKey = KeyCode.LeftControl;

    void FixedUpdate() {
        if (Input.GetKeyDown(pullKey) && !Input.GetKey(pushKey)) {
            Pull();
        }
        if (Input.GetKeyDown(pushKey) && !Input.GetKeyDown(pullKey)) {
            StartEmpower();
        }
        if(Input.GetKey(pushKey)) {
            Empowering();
        }
        if(Input.GetKeyUp(pushKey)) {
            Push();
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Movement(h, v);
    }
}
