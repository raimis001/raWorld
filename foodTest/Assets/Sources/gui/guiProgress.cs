using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiProgress : MonoBehaviour {

	float _progress = 0;
	public float Progress {
		get { return _progress; }
		set {
			_progress = value;
			if (_progress > 1) _progress = 1;
			if (_progress < 0) _progress = 0;

			Slider slider = GetComponent<Slider>();

			if (slider) slider.value = _progress + 0.1f;

		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
