using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	public static ScoreManager instance;
	public int currentTimeRecord, highestTimeRecord;

	void Awake () {
		if(instance == null){
			instance = this;
		}
		else if(instance != this){
			Destroy(this);
		}
		
		DontDestroyOnLoad(gameObject);
	}

	void OnEnable(){
		EventManager.OnGameSucceed += GameSucceed;
	}
	
	void OnDisable(){
		EventManager.OnGameSucceed -= GameSucceed;
	}	

	void GameSucceed(){
		currentTimeRecord = Mathf.RoundToInt(Time.timeSinceLevelLoad);
		DebugLogger.Log("CurrentTimeRecord : " + TimeToString(currentTimeRecord));

		highestTimeRecord = PlayerPrefs.GetInt("Level" + Application.loadedLevel + "TimeRecord", 1000000);

		if(currentTimeRecord < highestTimeRecord){
			highestTimeRecord = currentTimeRecord;
			PlayerPrefs.SetInt("Level" + Application.loadedLevel + "TimeRecord", Mathf.RoundToInt(highestTimeRecord));
		}
		DebugLogger.Log("Level" + Application.loadedLevel + "TimeRecord : " + TimeToString(highestTimeRecord));
	}

	string TimeToString(float time){
		int minute, second;

		minute = Mathf.RoundToInt(time) / 60;
		second = Mathf.RoundToInt(time) % 60;

		return string.Format("{0:00} : {1:00}", minute, second);
	}
}
