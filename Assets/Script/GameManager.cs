using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public GameState gamesState;
	public int gridSizeX, gridSizeZ;
	public GameObject levelClearUI;

	public enum GameState{
		MainMenu,
		InLevel,
		GameSucceed,
		GameOver
	};
	
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
		EventManager.OnReturnMenu += ReturnMenu;
		EventManager.OnGameStart += GameStart;
		EventManager.OnGameSucceed += GameSucceed;
		EventManager.OnGameOver += GameOver;
	}

	void OnDisable(){
		EventManager.OnReturnMenu -= ReturnMenu;
		EventManager.OnGameStart -= GameStart;
		EventManager.OnGameSucceed -= GameSucceed;
		EventManager.OnGameOver -= GameOver;
	}	

	void ReturnMenu(){
		gamesState = GameState.MainMenu;
	}

	void GameStart(){
		gamesState = GameState.InLevel;
	}

	void GameSucceed(){
		gamesState = GameState.GameSucceed;
		levelClearUI.SetActive(true);
	}

	void GameOver(){
		gamesState = GameState.GameOver;
		AudioManager.PlaySound(AudioManager.AudioName.GameOver);
	}

    public void LoadScene(int n)
    {
        Application.LoadLevel(n);
    }
}

