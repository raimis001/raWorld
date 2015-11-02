using UnityEngine;
using System.Collections;

public class guiTaste : MonoBehaviour {

	public RectTransform Viewport;

	int _progress = 0;
	public int Progress = 0;

	Vector2 endPos;
	Vector2 startPos;
	// Use this for initialization
	void Start () {
		endPos = Viewport.anchoredPosition;
		startPos = Viewport.anchoredPosition;
		Debug.Log(endPos);
		//29.6 - 330.4
	}
	
	// Update is called once per frame
	void Update () {

		if (_progress != Progress) {
			_progress = Progress;
			endPos.x = startPos.x - _progress * 32f;
		}

		if (Vector2.Distance(Viewport.anchoredPosition, endPos) < 0.1f) {
			Viewport.anchoredPosition = endPos;
			return;
		}

		Vector2 pos = Viewport.anchoredPosition;

		pos.x = Mathf.Lerp(pos.x, endPos.x, 0.1f);
		Viewport.anchoredPosition = pos;
	}
}
