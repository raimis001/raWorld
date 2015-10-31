using UnityEngine;
using System.Collections.Generic;

public class World {

	public const int sizeX = 200;
	public const int sizeY = 200;
	
	public static Sprite[] terrainSprites; //Terrain sprites
	public static Sprite[] guiSprites;
	public static Sprite[] itemsSprites;
	public static Sprite[] interfaceSprites;
	public static Sprite[] wallSprites;
	
	public static int currentWeapon = 0;
	public static List<int> weapons = new List<int>();
	
	public static Dictionary<string, WorldTile> tiles = new Dictionary<string, WorldTile>();
	
	static int[] randomChunk = {40,5,5,3,10,1};
	
	public delegate void CreateTile();
	public static event CreateTile OnTileCreate;
	
	public static void initWorld() {
		
		terrainSprites 	= Resources.LoadAll<Sprite>("Textures/terrain");
		guiSprites 		= Resources.LoadAll<Sprite>("Textures/gui");
		itemsSprites 	= Resources.LoadAll<Sprite>("Textures/resource");
		interfaceSprites= Resources.LoadAll<Sprite>("Textures/interface");
		wallSprites		= Resources.LoadAll<Sprite>("Textures/Rock_Atlas");
		
		WorldData.initTiles();
		
		weapons.Add(16);
		weapons.Add(17);
		weapons.Add(19);
		weapons.Add(27);
	}
	
	public static void generateWorld() {
		
		int r, t, c;
		string key;
		for (int y = 0; y < sizeY; y++) {
			for (int x = 0; x < sizeX; x++) {
				key = getKey(x,y);
				
				r = Random.Range(0,100);		
				t = -1;
				c = 0;
				for (int rnd = 0; rnd < randomChunk.Length; rnd++) {
					c += randomChunk[rnd];
					if (r < c) {
						t = rnd;
						break;
					}
				}
				if (t < 0) {
					tiles.Add(key,null);
					continue;
				}
				
				tiles.Add(key, new WorldTile(t, x, y));
				
			}
			
		}	
	}
	
	public static WorldTile createTile(int tileID, Vector2 position) {
		string key = getKey(position);
		if (!tiles.ContainsKey(key)) {
			Debug.Log("Wrong tile");
			return null;
		}
		
		if (tiles[key] != null) {
			Debug.Log("Tile not empty");
			return null;
		}
	
		DataTile tile = WorldData.tiles[tileID];
		if (tile.result == null || tile.result.id < 0) {
			Debug.Log("Result not defined");
			return null;
		}
		
		
		
		tiles[key] = new WorldTile(tile.result.id, position);
		
		if (OnTileCreate != null) OnTileCreate();
		
		return tiles[key];
	}
	
	public static Sprite nextWeapon(int sign = 1) {
		
		if (sign > 0) {
			currentWeapon++;
			if (currentWeapon >= weapons.Count) currentWeapon = 0;
		} else {
			currentWeapon--;
			if (currentWeapon < 0) currentWeapon = weapons.Count - 1;
		}
		
		return getWeaponSprite();
	}
	
	public static Sprite getWeaponSprite(int weaponID = -1) {
		if (weaponID < 0) weaponID = currentWeapon;
		DataTile tile = WorldData.tiles[weapons[weaponID]];
		
		return tile.sprite;
	}
	
	public static WorldTile getTile(float x, float y) {
		string key = getKey(alignPos(x,y));
		if (tiles.ContainsKey(key)) {
			return tiles[key];
		} else {
			return null;
		}
	}
	public static WorldTile getTile(Vector3 pos) {
		return getTile(pos.x, pos.y);
	}
	public static WorldTile getTile(Vector2 pos) {
		return getTile(pos.x, pos.y);
	}
	
	public static string getKey(float x, float y) {
		return ((int)x).ToString() + "," + ((int)y).ToString();
	}
	public static string getKey(Vector3 pos) {
		return getKey(pos.x, pos.y);
	}
	public static string getKey(Vector2 pos) {
		return getKey(pos.x, pos.y);
	}
	public static Vector2 alignPos(float xx, float yy) {
		Vector2 result = new Vector2();
		result.x = Mathf.CeilToInt(Mathf.FloorToInt((xx + 0.5f) * 64f) / 64);
		result.y = Mathf.CeilToInt(Mathf.FloorToInt(yy * 64f) / 64);
		return result;
	}	
	public static Vector2 alignPos(Vector3 pos) {
		return alignPos(pos.x, pos.y);
	}
	public static Vector2 alignPos(Vector2 pos) {
		return alignPos(pos.x, pos.y);
	}
	
}
