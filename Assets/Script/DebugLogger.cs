#define DEBUG
using UnityEngine;
using System.Collections;

public class DebugLogger : MonoBehaviour {

	public static void Log(object msg){
		#if DEBUG
		Debug.Log(msg);
		#endif
	}

	public static void LogWarning(object msg){
		#if DEBUG
		Debug.LogWarning(msg);
		#endif
	}
	
	public static void LogError(object msg){
		#if DEBUG
		Debug.LogError(msg);
		#endif
	}
}
