using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour {
	

	int _tileID = 0;
	float _destroyTime = 2f;
	bool markDestroy = false;
	public int tileID {
		get { return _tileID; }
		set {
			_tileID = value;
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			if (sr == null) return;

			DataTile tile = WorldData.tiles[_tileID];
			sr.sprite = tile.sprite;
		}
	}

	Vector3 endPos;

	// Use this for initialization
	void Start () {
		
		float angle = Random.Range(0f, Mathf.PI * 2);
		float dist = Random.Range(0.5f, 1.5f);
		
		float x = transform.position.x + Mathf.Sin(angle) * dist;
		float y = transform.position.y + Mathf.Cos(angle) * dist;
		
		endPos =  new Vector3(x, y, transform.position.z);
		_destroyTime = Random.Range(3f,4f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position, endPos) > 0.01f) {
			float step = 3f * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, endPos, step);
			return;
		} 
		
		if (_destroyTime > 0) {
			_destroyTime -= Time.deltaTime;
		} 
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Rect main = new Rect(transform.position.x - 0.5f,transform.position.y - 0.5f,1f,1f);
		if (main.Contains(new Vector2(mousePos.x, mousePos.y))) {
			_destroyTime = 0;
		}
		
		
		if (!markDestroy &&_destroyTime <= 0) {
			destroyResource();
		}
		
	}
	void OnMouseEnter() {
		//_destroyTime = 0;
	}
	
	void destroyResource() {
		markDestroy = true;
		Inventory.addReward(_tileID, 1);
		Destroy(gameObject,0.1f);
	}
	
}
