using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEffectHandler : MonoBehaviour {

    public enum EffectName : int {
        Empower1 = 0,
        Explosion1 = 1,
        Empower2 = 2,
        Explosion2 = 3,
        Empower3 = 4,
        Explosion3 = 5,
        HitFire = 6
    }

    //不是每個特效都剛好生成在我們想要的位置上， 我們需要根據腳色現在的位置再做些微調
    public Dictionary<EffectName, Vector3> adjust = new Dictionary<EffectName, Vector3>(){
        {EffectName.Empower1 , new Vector3(0, 1, 0)},
        {EffectName.Explosion1 , new Vector3(0, 0, 0)},
        {EffectName.Empower2 , new Vector3(0, 1, 0)},
        {EffectName.Explosion2 , new Vector3(0, 1, 0)},
        {EffectName.Empower3 , new Vector3(0, 1, 0)},
        {EffectName.Explosion3 , new Vector3(0, 0.7f, 0)},
        {EffectName.HitFire , new Vector3(0, 0, 0)},
    };

    public GameObject[] effectPrefabArray;
    UnityChan.UnityChanControlScriptWithRgidBody player;
    GameObject lastPlayEffect;

    void Start() {
        player = GetComponent<UnityChan.UnityChanControlScriptWithRgidBody>();
    }

    public void PlayEffect(EffectName effectName, Vector3 pos, float lifetime = 1f) {
        GameObject clone = (GameObject)Instantiate(effectPrefabArray[(int)effectName], pos + adjust[effectName], Quaternion.identity);
        lastPlayEffect = clone;
        Destroy(clone, lifetime);
        Debugger.Log(effectName.ToString());

        //Play effect's sound
        if (AudioManager.HaveAudio(effectName.ToString())) {
            AudioManager.AudioName effectAudio = (AudioManager.AudioName)System.Enum.Parse(typeof(AudioManager.AudioName), effectName.ToString());
            AudioManager.PlaySound(effectAudio);
        }
    }

    public void DestroyLastEffect() {
        Destroy(lastPlayEffect);
    }
}
