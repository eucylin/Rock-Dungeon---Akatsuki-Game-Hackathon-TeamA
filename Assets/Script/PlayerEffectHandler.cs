using UnityEngine;
using System.Collections;

public class PlayerEffectHandler : MonoBehaviour {

    public enum EffectName : int {
        empower1 = 0,
        explosion1 = 1,
        empower2 = 2,
        explosion2 = 3,
        empower3 = 4,
        explosion3 = 5,
        hitFire = 6
    }

    public GameObject[] effectPrefabArray;
    UnityChan.UnityChanControlScriptWithRgidBody player;

    void Start() {
        player = GetComponent<UnityChan.UnityChanControlScriptWithRgidBody>();
    }

    void Update() {

    }

    void PlayEffect(EffectName effectName) {

    }
}
