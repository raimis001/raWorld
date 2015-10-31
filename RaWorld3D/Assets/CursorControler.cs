using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorControler : MonoBehaviour {

	static CursorControler _instance;
	public static DataReward reward = new DataReward(-1,0);
	public GameObject cursorImage;

	bool _inArea = false;
	public bool inArea {
		get { return _inArea; }
		set {
			_inArea = value;
			
			//if (animator == null) return;
			//animator.SetInteger("enabled",_inArea ? 1 : 0);
			
			if (spriteRenderer == null) return;
			if (_inArea) {
				spriteRenderer.color = Color.white;
			} else {
				spriteRenderer.color = Color.red;// new Color(1f,0f,0f);
			}
			
		}
	}

	//Animator animator;
	SpriteRenderer spriteRenderer;
	Image imageSprite;

	// Use this for initialization
	void Start () {
		_instance = this;
		
		//animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		
		if (cursorImage != null) imageSprite = cursorImage.GetComponent<Image>();
		
		if (imageSprite != null) imageSprite.enabled = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;
		Vector2 alignMouse = World.alignPos(mousePos);
		transform.position = new Vector3(alignMouse.x, alignMouse.y);
		cursorImage.transform.position = Input.mousePosition;// + new Vector3(65f, 65f);
		//cursorImage.transform.position = mousePos;
	}
	
	public void setReward(int itemID, int count) {
		reward = new DataReward(itemID, count);
		
		if (imageSprite == null) return;
		
		if (reward.id < 0 || reward.count < 1) {
			imageSprite.enabled = false;
		} else {
			DataTile tile = WorldData.tiles[reward.id];
			if (tile != null) {
				imageSprite.sprite = tile.sprite;
				imageSprite.enabled = true;
			} else {
				imageSprite.enabled = false;
			}
		}
	}
		
	public void setReward(DataReward rew) {
		setReward(rew.id, rew.count);		
	}
	
	public static void SetReward(DataReward rew) {
		_instance.setReward(rew);
	}
	public static void SetReward(int itemID, int count) {
		_instance.setReward(itemID, count);
	}
}
