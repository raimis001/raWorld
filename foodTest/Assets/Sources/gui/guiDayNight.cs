using UnityEngine;
using System.Collections;

public class guiDayNight : MonoBehaviour {


	Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();

		animator.Play("DayNight", -1, Game.CurrentTime / 24f);
		animator.speed = 0f;

	}

	void OnEnable() {
		Game.OnChangeHour += OnHourChange;
	}

	void OnDisable() {
		Game.OnChangeHour -= OnHourChange;
	}

	// Update is called once per frame
	void Update () {
		animator.Play("DayNight", -1, Game.CurrentTime / 24f);
		animator.speed = 0f;
	}

	void OnHourChange() {


		Debug.Log(Game.CurrentTime.ToString("0.00"));
	}
}
