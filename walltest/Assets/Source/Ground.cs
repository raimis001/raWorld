using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Ground : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown() {
		if (EventSystem.current.IsPointerOverGameObject()) return;

		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouse.y = 0;
		mouse.x = (int)(mouse.x + 0.5f);
		mouse.z = (int)(mouse.z + 0.5f);
		//Debug.Log("Mose down x:" + mouse.x.ToString("0.00") + " y:" + mouse.z.ToString("0.00"));
	}
}
