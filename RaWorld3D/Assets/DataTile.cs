using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataTile  {
	
	public int id = 0;
	public List<DataReward> rewards = new List<DataReward>();
	public DataReward result;
	public float speed = 1f;
	public int type = 0;
	public int subType = 0;
	public string name;
	public int sprite_id = 0;
	public float growTime = 5f;
	public Sprite sprite = null;
	
	public DataTile(int id, int spriteID, int type, int subType = 0) {
		this.id = id;
		this.sprite_id = spriteID;
		this.type = type;
		this.subType = subType;
		
		switch (this.type) {
			case WorldData.TILE_TYPE_NONE:
			case WorldData.TILE_TYPE_PLANT:
			case WorldData.TILE_TYPE_FURNITURE:
				sprite = World.terrainSprites[sprite_id];
				break;
			case WorldData.TILE_TYPE_RESOURCE:
			case WorldData.TILE_TYPE_CRAFTING:
				sprite = World.itemsSprites[sprite_id];
				break;
			case WorldData.TILE_TYPE_WEAPON:
				sprite = World.guiSprites[sprite_id];
				break;
		}
		
	}
	
	public void addReward(int item, int value) {
		rewards.Add(new DataReward(item,value));
	}
	
}
