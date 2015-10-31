using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
	using UnityEditor;
	[InitializeOnLoad]
#endif
public class Main {

	public Main() {
		ItemsManager.Init();

		Debug.Log("Main init");
	}


#if UNITY_EDITOR
	static Main() {
		//new Main();
	}
#endif
}
