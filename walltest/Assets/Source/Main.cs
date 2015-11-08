using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
	using UnityEditor;
	[InitializeOnLoad]
#endif

public class Main {

	public Main() {
		LogInterface.InitMap("INFO:","gray");

		SimpleJSON.JSONNode node;

		//Read items
		node = SimpleJSON.JSONNode.Parse(Resources.Load<TextAsset>("data/world").text);
		ItemsManager.Init(node["items"]);

		LogInterface.InfoLog("INFO: Main init");
	}

	static Main() {
		new Main();
	}

}
