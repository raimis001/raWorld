using UnityEngine;
using System.Collections;

public class Mobject : MonoBehaviour {


	public static Mobject Create(string prefabName, Vector3 pos) {
		Object prefab = Resources.Load(prefabName);

		GameObject obj = Instantiate(prefab) as GameObject;

		GameObject parent = GameObject.Find("World");
		obj.transform.parent = parent.transform;
		obj.transform.localPosition = pos;
		obj.name = "vaga_" + PlayerCharacter.Index(pos);

		return obj.GetComponent<Mobject>();

	}

	public GameObject Player;
	public GameObject Image;

	// Use this for initialization
	void Start () {
		Collider pCollider = Player.GetComponent<Collider>();
		Collider cCollider = GetComponent<Collider>();

		if (pCollider && pCollider.enabled && cCollider && cCollider.enabled) { 
			Physics.IgnoreCollision(pCollider, cCollider);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		Debug.Log("Mouse click");
		Terrain t;
	
	}
}
