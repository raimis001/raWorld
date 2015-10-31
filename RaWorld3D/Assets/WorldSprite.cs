using UnityEngine;
using System.Collections;

public class WorldSprite : MonoBehaviour {

	public int status = WorldData.TILE_STATUS_READY;
	float growTime;

	int _tileID = -1;
	DataTile tile;
	public int tileID {
		get { return _tileID; }
		set {
			_tileID = value;
			
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			if (sr == null) return;
			BoxCollider2D bc = GetComponent<BoxCollider2D>();
			if (bc) bc.enabled = false;
			
			tile = WorldData.tiles[_tileID];
		
			switch (tile.type) {
				case WorldData.TILE_TYPE_PLANT:
					growTime = tile.growTime;
					status = WorldData.TILE_STATUS_GROW;
					transform.localScale = new Vector3(0f, 0f, 1f);
					sr.sprite = tile.sprite;
					break;
				case WorldData.TILE_TYPE_FURNITURE:
					status = WorldData.TILE_STATUS_GROW;
					sr.sprite = tile.sprite;
					break;
				case WorldData.TILE_TYPE_WALL:
					status = WorldData.TILE_STATUS_GROW;
					gameObject.AddComponent<Wall>();
					gameObject.GetComponent<Wall>().onTileCreate();
					bc.enabled = true;
					break;
				default:
					sr.sprite = tile.sprite;
					break;
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (status == WorldData.TILE_STATUS_GROW) {
			growTime -= Time.deltaTime;
			if (growTime <= 0) {
				status = WorldData.TILE_STATUS_READY;
				growTime = 0f;
				transform.localScale = Vector3.one;
			} else {
				transform.localScale = Vector3.one * (1f - (growTime / tile.growTime) * 0.8f);
			}
		}
	}
}
