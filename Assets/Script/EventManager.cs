using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
	public delegate void DefaultEventHandler();
	public static event DefaultEventHandler OnGameStart, OnGameSucceed, OnGameOver, OnReturnMenu;
}
