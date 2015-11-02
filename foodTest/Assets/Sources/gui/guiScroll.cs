using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class guiScroll : MonoBehaviour {
	public enum ScrollDirection {
		HORIZONTAL,
		VERTICAL
	}

	public RectTransform Content;
	public ScrollDirection Direction = ScrollDirection.HORIZONTAL;
	public float Step;
	public GameObject Prefab;

	public Button buttonUP;
	public Button buttonDOWN;

	Vector2 Destination;
	float ContentLength;

	List<GameObject> _itemsList = new List<GameObject>();

	void Awake() {
		//Debug.Log(Content.sizeDelta);
		ContentLength = Step;
		Content.sizeDelta = (Direction == ScrollDirection.HORIZONTAL ? new Vector2(ContentLength, Content.sizeDelta.y) : new Vector2( Content.sizeDelta.x, ContentLength));
		//Debug.Log(Content.sizeDelta);

	}

	// Use this for initialization
	void Start () {
		Destination = Content.anchoredPosition;

		if (buttonDOWN) buttonDOWN.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector2.Distance(Destination, Content.anchoredPosition);
		if (dist > 0.01f) {
			Vector2 pos = Content.anchoredPosition;
			pos = Vector2.Lerp(pos, Destination, 0.1f);
			Content.anchoredPosition = pos;
		} else if (dist != 0) {
			Content.anchoredPosition = Destination;
		}
	}

	GameObject AddItem(GameObject prefab = null) {
		if (prefab == null) prefab = Prefab;

		GameObject result = Instantiate(prefab);
		result.transform.SetParent(Content);
		result.transform.localScale = Vector3.one;
		_itemsList.Add(result);

		ContentLength = (Direction == ScrollDirection.HORIZONTAL ? Content.rect.width : Content.rect.height) + Step;

		//Debug.Log(Content.sizeDelta);

		Content.sizeDelta = (Direction == ScrollDirection.HORIZONTAL ? new Vector2(ContentLength, Content.sizeDelta.y) : new Vector2(Content.sizeDelta.x, ContentLength));

		Vector2 pos = Content.anchoredPosition;
		if (Direction == ScrollDirection.HORIZONTAL) {
			pos.x = 0;
		} else {
			pos.y = 0;
		}

		Content.anchoredPosition = pos;

		return result;
	}

	public T AddItem<T>(GameObject prefab = null) {
		GameObject result = AddItem(prefab);
		return result.GetComponent<T>();

	}
	public void ClearItems() {
		_itemsList.Clear();

		while (Content.childCount > 0) {
			GameObject.DestroyImmediate(Content.GetChild(0).gameObject);
		}

		Content.sizeDelta = new Vector2(0, 0);
		ContentLength = 0;

	}
	public T GetItem<T>(int itemID) {
		if (_itemsList[itemID] == null) return default(T);
		return (T)_itemsList[itemID].GetComponent<T>();
	}

	public void SliderChange(float value) {
		Destination = Content.anchoredPosition;
		Vector2 size = Content.sizeDelta;

		if (Direction == ScrollDirection.HORIZONTAL) {
			Destination.x = (size.x - Step) * value;
		} else {
			Destination.y = (size.y - Step) * value;
		}
		Content.anchoredPosition = Destination;


	}

	#region INTERFACE BUTTONS
	public void ButtonUP() {
		Vector2 st = Direction == ScrollDirection.HORIZONTAL ? new Vector2(Step, 0) : new Vector2(0, Step);
		float check = Direction == ScrollDirection.HORIZONTAL ? Destination.x : Destination.y;

		if (check > (-ContentLength + Step)) {
			Destination -= st;

			if (buttonDOWN) buttonDOWN.interactable = true;
			if (Destination.x <= (-ContentLength + Step)) {
				if (buttonUP) buttonUP.interactable = false;
			}

		}
	}
	public void ButtonDOWN() {
		Vector2 st = Direction == ScrollDirection.HORIZONTAL ? new Vector2(Step, 0) : new Vector2(0, Step);
		float check = Direction == ScrollDirection.HORIZONTAL ? Destination.x : Destination.y;

		if (check <= -Step) {

			Destination += st;

			if (buttonUP) buttonUP.interactable = true;
			if (Destination.x > -Step) {
				if (buttonDOWN) buttonDOWN.interactable = false;
			}

		}
	}
	public void ButtonADD() {
		AddItem(Prefab);
	}
	public void ButtonCLEAR() {
		ClearItems();
	}


	#endregion

}
