#define LOG_DEBUG
#define LOG_ERROR
#define LOG_INFO

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LogInterface  {

	static Dictionary<string, string> LogMap = new Dictionary<string,string>();

	static LogInterface _instance = null;
	public static LogInterface Instance {
		get {
			if (_instance == null) _instance = new LogInterface();
			return _instance;
		}
	}

	public static void DebugLog(string msg) {
		Instance.Debug(msg);
	}
	public static void InfoLog(string msg) {
		Instance.Info(msg);
	}
	public static void ErrorLog(string msg, Exception exception) {
		Instance.Error(msg, exception);
	}

	public static void InitMap(string values, string colors) {
		string[] maps = values.Split(' ');
		string[] color = colors.Split(' ');

		for (int i = 0; i < maps.Length; i++) {
			if (LogMap.ContainsKey(maps[i])) continue;
			string c = "blue";
			if (i < color.Length) c = color[i];

			LogMap.Add(maps[i],c);

		}
	}

	void Debug(string msg) {
		#if LOG_DEBUG
				UnityEngine.Debug.Log(msg);
		#endif
	}
	void Info(string msg) {
		#if LOG_INFO
			
			string color = "";
			foreach (string map in LogMap.Keys) {
				if (msg.IndexOf(map) == 0) {
							color = LogMap[map];
				}
			}

			if (color == "") return;

			UnityEngine.Debug.Log("<color=" + color + ">" + msg + "</color>");
		#endif
	}

	void Error(string msg, Exception exception) {
			#if LOG_ERROR
					UnityEngine.Debug.Log("<clor=red>" + msg + "</color>/n" + exception != null ? exception.Message : "");
			#endif
	}


}
