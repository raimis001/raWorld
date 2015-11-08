using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour {

	public float speed = 3f;


	public static int InstrumentMax = 4;
	public static int InstrumentCurrent = 0;
	public static float WorkingTime = 1f;

	public static Dictionary<string, Mobject> ObjectCloud = new Dictionary<string, Mobject>();

	[Flags]
	public enum Actions {
		IDLE = 0x0,
		SPADE = 0x1,
		UP = 0x2,
		DOWN = 0x4,
		LEFT = 0x8,
		RIGHT = 0x10,
		UP_RIGHT = 0x20,
		UP_LEFT = 0x40,
		DOWN_LEFT = 0x80,
		DOWN_RIGHT = 0x100,
	}

	public static Actions Action = Actions.IDLE;

	Rigidbody _rigibody;
	Animator _animator;

	
	float _workingTime = 0;

	float _cameraFOV = 0;

	public static string Index(float x, float y) {
		return x.ToString("0") + ":" + y.ToString("0");
	}

	public static string Index(Vector3 pos) {
		return Index(pos.x, pos.y);
	}

	// Use this for initialization
	void Start () {
		_rigibody = GetComponent<Rigidbody>();
		_animator = GetComponent<Animator>();

		_cameraFOV = Camera.main.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {


		if (Input.mouseScrollDelta.y != 0) {
			_cameraFOV += Input.mouseScrollDelta.y * 2f;
			if (_cameraFOV < 1) _cameraFOV = 1f;
		}

		if (Mathf.Abs(Camera.main.fieldOfView - _cameraFOV) > 0) {
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _cameraFOV, 0.2f);
			if (Mathf.Abs(Camera.main.fieldOfView - _cameraFOV) < 0.2f) {
				Camera.main.fieldOfView = _cameraFOV;
			}
		}
		

		if (Action == Actions.IDLE) {

			if (InstrumentCurrent != 0 && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
				Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mouse = new Vector3(Mathf.RoundToInt(mouse.x - 0.5f), Mathf.RoundToInt(mouse.y - 0.5f), 0);

				string idx = Index(mouse);

				if (ObjectCloud.ContainsKey(idx)) {
					ShowError("Here is object @" + idx);
					return;
				}


				switch (InstrumentCurrent) {
					case 1:
						Action = Actions.SPADE;
						_animator.SetTrigger("working");
						_workingTime = WorkingTime;
						ObjectCloud.Add(idx, mobjVaga.Create(mouse));
						break;
				}

				return;
			}
			

			float x = Input.GetAxis("Horizontal") * Time.smoothDeltaTime * speed * 100f;
			float y = Input.GetAxis("Vertical") * Time.smoothDeltaTime * speed * 100f;

			//transform.Translate(x, 0, y);

			_rigibody.velocity = new Vector3(x, 0, y);


			return;
		}

		_workingTime -= Time.smoothDeltaTime;
		if (_workingTime <= 0) {
			_animator.SetTrigger("idle");
			Action = Actions.IDLE;
		}

	}

	void ShowError(string message) {
		Debug.Log(message);
	}
}
