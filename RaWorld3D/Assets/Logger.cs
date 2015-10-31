using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Logger : MonoBehaviour {

	public Text loggerText;
	public Scrollbar loggerScroll;

	private static Logger _instance;
	private static bool _update = false;
	
	private static List<string> textList = new List<string>();

	// Use this for initialization
	void Start () {
		_instance = this; 
	}
	
	// Update is called once per frame
	void Update () {
		if (_update) {
			loggerScroll.value = 1f;
			_update = false;
		}
	}
	public void changeText() {
		Debug.Log("Changess");
	}

	public static void addLog(string text) {
		if (_instance == null) return;
		if (_instance.loggerText == null) return;
		
		if (textList.Count > 10) textList.RemoveRange(10,1);
		textList.Insert(0,text);
		
		Text txt = _instance.loggerText;
		txt.text = "";
		
		for (int i = 0; i < textList.Count; i++) {
			txt.text += textList[i] + "\n";	
		}
		/*
		Text txt = _instance.loggerText;
		
		string tmp = txt.text;
		txt.text = text;
		if (tmp != "") {
			txt.text += "\n" + tmp;
		}
		
		Debug.Log(txt.GetComponent<RectTransform>().sizeDelta);
		*/
		txt.GetComponent<RectTransform>().sizeDelta = new Vector2(200,txt.preferredHeight); 
		//txt.GetComponent<RectTransform>().anchoredPosition = new Vector2(7f,70f);
		_update = true;
		
		
	}
}
