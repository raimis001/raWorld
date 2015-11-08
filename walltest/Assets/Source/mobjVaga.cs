using UnityEngine;
using System.Collections;

public class mobjVaga : Mobject {


	public static Mobject Create(Vector3 pos) {
		return Mobject.Create("vaga",pos);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
