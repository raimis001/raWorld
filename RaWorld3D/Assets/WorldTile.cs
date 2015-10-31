using UnityEngine;
using System.Collections;

public class WorldTile {

	public int tileID;
	public int x;
	public int y;
	public string key;
	
	public float workingTime = 0.75f;
	public string name;
	
	public float speed = 1f;

	GameObject gameObject;
	WorldSprite sprite;
	DataTile tile;

	public WorldTile(int tileID, int x, int y) {
		createTile(tileID, x, y);
	}
	public WorldTile(int tileID, Vector2 position) {
		createTile(tileID, position.x, position.y);
	}
	
	private void createTile(int tileID, float x, float y) {
		this.tileID = tileID;
		this.x = (int)x;
		this.y = (int)y;
		this.key = World.getKey(this.x,this.y);
		
		tile = WorldData.tiles[tileID];
		name = tile.name;
		
		create();
	}
		
	public void create() {
		gameObject = TerrainControler.createObject(x,y);
		sprite = gameObject.GetComponent<WorldSprite>();
		sprite.tileID = tileID;
		
		//gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		
	}

	public bool doWork (bool working) {
		//Debug.Log("Do work " + working.ToString() + " on  " + name);
		if (!working) return false;
		
		if (tile.type == WorldData.TILE_TYPE_FURNITURE) return false;
		if (tile.type == WorldData.TILE_TYPE_WALL) return false;
		
		if (sprite.status != WorldData.TILE_STATUS_READY) return false;
		
		return true;
	}
	
	public void endWork() {
		if (tile.type == WorldData.TILE_TYPE_FURNITURE || tile.type == WorldData.TILE_TYPE_WALL) {
			if (sprite.status == WorldData.TILE_STATUS_GROW) {
				sprite.status = WorldData.TILE_STATUS_READY;
			}
			return;
		}
		
		if (sprite.status == WorldData.TILE_STATUS_READY) {
			MonoBehaviour.Destroy(gameObject,0.075f);
			World.tiles[key] = null;
			
			foreach (DataReward rew in tile.rewards) {
				for  (int c = 0; c < rew.count; c++) {
					GameObject reward = TerrainControler.createObject(x,y, "TileResource");
					reward.GetComponent<Resource>().tileID = rew.id;
				}
			}
		}
		
	}
}
