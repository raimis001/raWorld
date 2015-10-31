using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControler : MonoBehaviour {
	
	public int speed = 12;
	public int workDistance = 4;
	public CursorControler cursor;
	public GameObject instrument;
	public GameObject inventory;
	public GameObject weapon;
	public InventoryPanel hand;
	
	public Animator character;

	BoxCollider2D boxCollider;
	WorldTile _workingTile;
	float _workingTime = 0f;
	int terrainLayer;
	
	// Use this for initialization
	void Start () {
		terrainLayer = 1 << LayerMask.NameToLayer("terrain");
		
		boxCollider = GetComponent<BoxCollider2D>();

		if (weapon != null) 		
			weapon.GetComponent<InventoryPanel>().setImage(World.getWeaponSprite());
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_workingTile != null) {
			_workingTime += Time.smoothDeltaTime;
			if (_workingTime > _workingTile.workingTime) {
				_workingTile.endWork();
				_workingTile = null;
				_workingTime = 0f;
				if (instrument != null) {
					instrument.GetComponent<SpriteRenderer>().enabled = false;
				}
				
			}
			return;
		}
	
		if (Input.GetKeyDown(KeyCode.I)) {
			if (inventory.activeSelf) {
				inventory.SetActive(false);
			} else {
				inventory.SetActive(true);
			}
			return;
		}
		
		WorldTile tile;
	
		float ix = Input.GetAxis ("Horizontal");
		float iy = Input.GetAxis ("Vertical");
		
		if (ix != 0f || iy != 0f) {
			
			//Debug.Log(transform.position);
			//Debug.Log(World.alignPos(transform.position ));
			
			RaycastHit2D hit;
		
			float tileSpeed = 1f;
			
			tile = World.getTile(transform.position );
			if (tile != null) tileSpeed = tile.speed;
			
			Vector2 direction = new Vector2(ix, iy) * speed * Time.smoothDeltaTime * tileSpeed;
			Vector3 newPos = transform.position + new Vector3(direction.x, direction.y);
			if (newPos.x < 0 || newPos.y < 0) return;
				
			Vector2 dir = Vector2.zero;
			if (boxCollider == null) {
				transform.Translate(direction.x, direction.y, 0);
				dir = direction;
			} else {
				//boxCollider.
				Vector2 center = new Vector2(transform.position.x, transform.position.y) + boxCollider.offset;
				hit = Physics2D.BoxCast(center, boxCollider.size, 0, new Vector2(0, direction.y), Mathf.Abs(direction.y),terrainLayer);
				//Debug.DrawRay(transform.position, direction * 10, Color.yellow);
				
				if (hit.collider == null) {
					transform.Translate(0, direction.y, 0);
					dir.y = iy;
				} else {
					
				}
				
				hit = Physics2D.BoxCast(center, boxCollider.size, 0, new Vector2(direction.x,0), Mathf.Abs(direction.x),terrainLayer);
				if (hit.collider == null) {
					transform.Translate(direction.x, 0, 0);
					dir.x = ix;
				}
				
			}
			Debug.Log(dir);
			if (dir.y > 0) {
				character.SetInteger("direction",3);
			} else if (dir.x > 0) {
				character.SetInteger("direction",2);
			} else if (dir.x < 0) {
				character.SetInteger("direction",1);
			} else {
				character.SetInteger("direction",0);
			}
		} else {
			character.SetInteger("direction",0);
		}
		
		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
				if (Camera.main.orthographicSize > 1) Camera.main.orthographicSize--;
			} else {
				nextWeapon();
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
				//if (Camera.main.orthographicSize < 12) 
					Camera.main.orthographicSize++;
			} else {
				nextWeapon(-1);
			}
		}
		
		
		Vector3 currentPos = new Vector3(transform.position.x, transform.position.y);
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		Vector2 alignMouse = World.alignPos(mousePos);
		
		float dist = Vector2.Distance(currentPos,alignMouse);
		bool working = dist < workDistance;
		
		cursor.inArea = working;
		
		if (Input.GetMouseButtonDown(1) && CursorControler.reward.id > -1) {
			if (Inventory.addReward(CursorControler.reward)) {
				putinHand(-1,0);
			}
			return;
		}
		
		
		tile = World.getTile(mousePos);
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			
			if (tile == null) {//&& hand.itemID > -1
				if (working ) {
					 tile = World.createTile(hand.itemID, alignMouse);
					 if (tile != null) {
						if (hand.count > 1) {
							int cnt = hand.count - 1;
							hand.setReward(hand.itemID, cnt);
							cursor.setReward(hand.itemID, cnt);
						} else {
							hand.setReward(-1);
							cursor.setReward(-1,0);
						}
						_workingTime = 0f;
						_workingTile = tile;
						instrument.GetComponent<SpriteRenderer>().enabled = true;
					}
				}
			} else {
				if (tile.doWork(working)) {
					_workingTime = 0f;
					_workingTile = tile;
					instrument.GetComponent<SpriteRenderer>().enabled = true;
				}
			}
		}
		
	}
	
	void nextWeapon(int sign = 1) {
		Sprite sprite = World.nextWeapon(sign);
		
		if (weapon == null) return;
		weapon.GetComponent<InventoryPanel>().setImage(sprite);
	}
	
	public void inventoryClose() {
		Debug.Log("Inventory close");
		inventory.SetActive(false);
	}
	
	public void weaponClick() {
		Debug.Log("Click on weapon panel");
	}
	public void putinHand(int itemID, int count) {
		hand.setReward(itemID,count);
		cursor.setReward(itemID,count);
	}
	
	public void putinHand(DataReward rew) {
		putinHand(rew.id, rew.count);
	}
	
}
