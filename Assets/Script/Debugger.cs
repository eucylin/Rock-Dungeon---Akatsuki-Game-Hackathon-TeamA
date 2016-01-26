using UnityEngine;
using System.Collections;

public class Debugger : MonoBehaviour {

	public static void Log(object msg){
		#if DEBUG
		Debug.Log(msg);
		#endif
	}

    public static void Log(object msg, Object context) {
        #if DEBUG
        Debug.Log(msg, context);
        #endif
    }

    public static void LogWarning(object msg){
		#if DEBUG
		Debug.LogWarning(msg);
		#endif
	}

    public static void LogWarning(object msg, Object context) {
        #if DEBUG
        Debug.LogWarning(msg, context);
        #endif
    }

    public static void LogError(object msg){
		#if DEBUG
		Debug.LogError(msg);
		#endif
	}

    public static void LogError(object msg, Object context) {
        #if DEBUG
        Debug.LogError(msg);
        #endif
    }
}
