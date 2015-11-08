using UnityEngine;
using System.Collections;

public class guiTools : MonoBehaviour {

	public RectTransform ToolsList;


	private float _duration = 0.15f;
	private bool _sliding = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ListUp() {
		if (!ToolsList) return;
		if (PlayerCharacter.InstrumentCurrent == 0) return;

		PlayerCharacter.InstrumentCurrent--;
		if (!_sliding) StartCoroutine(Slide(1));
	}
	public void ListDown() {
		if (!ToolsList) return;
		if (PlayerCharacter.InstrumentCurrent >= PlayerCharacter.InstrumentMax - 1) return;

		PlayerCharacter.InstrumentCurrent++;
		if (!_sliding) StartCoroutine(Slide(1));
	}

	IEnumerator Slide(float direction) {

		//Debug.Log("Start move " + Time.time);
		//float capture = Time.time;

		_sliding = true;
		float c = ToolsList.anchoredPosition.x;
		float index = 0;
		while (index < _duration) {
			float dest = PlayerCharacter.InstrumentCurrent * -64f;
			float y = Mathf.Lerp(c, dest, index / _duration);
			ToolsList.anchoredPosition = new Vector2(y, 0);
			index += Time.smoothDeltaTime;
			yield return null;
		}

		ToolsList.anchoredPosition = new Vector2(PlayerCharacter.InstrumentCurrent * -64, 0);
		_sliding = false;
		//Debug.Log("End move " + (Time.time - capture).ToString("0.000"));
	}
}
