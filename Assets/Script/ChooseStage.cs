using UnityEngine;
using System.Collections;

public class ChooseStage : MonoBehaviour {

    public void LoadStage(int stageNum)
    {
        Application.LoadLevel(Application.loadedLevel + stageNum + 1);
    }
}
