using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
	public delegate void DefaultEventHandler();
	public static event DefaultEventHandler OnGameStart, OnGameSucceed, OnGameOver, OnReturnMenu;

	public static void GameStart(){
		OnGameStart();
	}

	public static void GameSucceed(){
		OnGameSucceed();
	}

	public static void GameOver(){
		OnGameOver();
	}

	public static void ReturnMenu(){
		OnReturnMenu();
	}
}
