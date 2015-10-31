using UnityEngine;
using System.Collections;

public class guiTab : MonoBehaviour {

	public delegate void ButtonCallback(int tag);
	public ButtonCallback Callback;

	public int ID;

	bool _selected = false;
	public bool Selected {
		get { return _selected; }
		set {
			if (_selected == value) return;
			
			_selected = value;

			Animator anim = GetComponent<Animator>();
			anim.SetTrigger(_selected ? "Open" : "Close");


		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseClick() {
		if (Callback != null) Callback(ID); 
	}

	public void OnMouseOver() {
		if (_selected) return;
		//Debug.Log("Mouse over");

		Animator anim = GetComponent<Animator>();
		anim.SetTrigger("Select");

	}
	public void OnMouseOut() {
		if (_selected) return;
		//Debug.Log("Mouse out");
		Animator anim = GetComponent<Animator>();
		anim.SetTrigger("Deselect");
	}

}
