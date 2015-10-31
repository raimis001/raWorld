using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerData  {
	private static PlayerData _instance;

	public guiProgress HealthProgress;
	public guiProgress TiredProgress;

	static float _health = 1;
	public static float Health {
		get {  return _health; }
		set {
			_health = value;
			if (_instance == null) return;
			if (_instance.HealthProgress) _instance.HealthProgress.Progress = _health;
		}
	}
	static float _tired = 0;
	public static float Tired {
		get { return _tired; }
		set {
			_tired = value;
			if (_instance == null) return;
			if (_instance.TiredProgress) _instance.TiredProgress.Progress = _tired;
		}
	}
	public float Water = 100;
	public float Hunger = 0;
	public float Hapiness = 50;

	public float TiredRate {
		get {
			float rate = 1;
			return (12f * Game.HourTime) / rate;
		}
	}

	public PlayerData() {
		_instance = this;
	}
}

public class PlayerManager  {


}
