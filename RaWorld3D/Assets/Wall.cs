using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall : MonoBehaviour {


	int x;
	int y;
	
	int indexID = -1;
	string oldKey = "";
	
	Sprite[] sprites;
	 
	Dictionary <string, int> keyList = new Dictionary<string, int>();
	// Use this for initialization
	void Start () {
		x = (int)transform.position.x;
		y = (int)transform.position.y;
		
		keyList.Add("0000",12);
		keyList.Add("1000",4);
		keyList.Add("0100",14);
		keyList.Add("0010",8);
		keyList.Add("0001",13);
		keyList.Add("1100",6);
		keyList.Add("0011",9);
		keyList.Add("1111",3);
		keyList.Add("1010",0);
		keyList.Add("1001",5);
		keyList.Add("0110",10);
		keyList.Add("0101",15);
		keyList.Add("1011",1);
		keyList.Add("0111",11);
		keyList.Add("1110",2);
		keyList.Add("1101",7);
		
		sprites = World.wallSprites;
		
		World.OnTileCreate += onTileCreate;
		onTileCreate();
	}
	void OnDestroy() {
		World.OnTileCreate -= onTileCreate;
	}
	
	public void onTileCreate() {
		WorldTile obj;
		DataTile tile;
		
		string x1 = "0";
		obj = World.getTile(x-1, y);
		if (obj != null) {
			tile = WorldData.tiles[obj.tileID];
			if (tile.type == WorldData.TILE_TYPE_WALL) x1 = "1";
		}	
		
		string x2 = "0";
		obj = World.getTile(x+1, y);
		if (obj != null) {
			tile = WorldData.tiles[obj.tileID];
			if (tile.type == WorldData.TILE_TYPE_WALL) x2 = "1";
		}
		
		string x3 = "0";
		obj = World.getTile(x, y - 1);
		if (obj != null) {
			tile = WorldData.tiles[obj.tileID];
			if (tile.type == WorldData.TILE_TYPE_WALL) x3 = "1";
		}
		
		string x4 = "0";
		obj = World.getTile(x, y + 1);
		if (obj != null) {
			tile = WorldData.tiles[obj.tileID];
			if (tile.type == WorldData.TILE_TYPE_WALL) x4 = "1";
		}
		
		string key = x1 + x2 + x3 + x4;
		
		if (oldKey != key) {
			int spr;
			if (keyList.TryGetValue(key, out spr)) {
				oldKey = key;
				GetComponent<SpriteRenderer>().sprite = sprites[spr];
			}
		}	
	}
	
	
	// Update is called once per frame
	void Update () {
	}
}
