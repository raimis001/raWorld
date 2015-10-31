using UnityEngine;
using System.Collections;

public class TerrainControler : MonoBehaviour {

	public static Transform _transform;

	// Use this for initialization
	void Start () {
		
		World.initWorld();
		
		//transform.position = new Vector3(World.sizeX / 2 - 0.5f, World.sizeY / 2 , 0.1f);
		//transform.localScale = new Vector3(World.sizeX, World.sizeY, 1f);
	
		_transform = transform;
		
		World.generateWorld();

		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static GameObject createObject(float x, float y, string spriteName = "TileSprite") {
		GameObject obj = Instantiate (Resources.Load (spriteName),new Vector3(x, y,0),Quaternion.identity) as GameObject;
			obj.transform.parent = _transform;
		
		return obj;
	}
}
