using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    //Player Handler
    public UnityChan.UnityChanControlScriptWithRgidBody player;

    protected void StartEmpower() {
        player.InitEmpower();
    }

    protected void Empowering() {
        player.Empowering();
    }

    protected void Push() {
        player.DoPush();
    }

    protected void Pull() {
        player.DoPull();
    }

    protected void Movement(float h, float v) {
        player.MoveControl(h, v);
    }
}
