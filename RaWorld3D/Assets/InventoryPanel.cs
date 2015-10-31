using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour {


	public int itemID;
	public int count;

	GameObject _icon;
	GameObject _text;
	GameObject _status;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void setImage(Sprite sprite) {
		if (_icon == null) {
			if (transform.childCount < 1) return;
			
			Transform fnd = transform.FindChild("ImageIcon");
			if (fnd == null) return;
			_icon = fnd.gameObject;
			
			if (_icon == null) return;
		}
	
		if (sprite == null) {
			_icon.GetComponent<Image>().enabled = false;		
		} else {
			_icon.GetComponent<Image>().sprite = sprite;
			_icon.GetComponent<Image>().enabled = true;
		}
	}
	public void setText(string text) {
		if (_text == null) {
			if (transform.childCount < 1) return;
			Transform fnd = transform.FindChild("TextCount");
			if (fnd == null) return;
			
			_text = fnd.gameObject;
			if (_text == null) return;
		}		
		
		_text.GetComponent<Text>().text = text;
	}
	
	
	public void setReward(int itemID, int count = 0) {
		this.itemID = itemID;
		this.count = count;
		
		if (itemID < 0) {
			setImage(null);
			setText("");
		} else {
			DataTile tile = WorldData.tiles[itemID];
			
			setImage(tile.sprite);
			setText(count > 0 ? count.ToString() : "");
		}
	}
	
	public void setStatus(int status) {
		if (_status == null) {
			if (transform.childCount < 1) return;
			Transform fnd = transform.FindChild("ImageStatus");
			if (fnd == null) return;
			_status = fnd.gameObject;
			if (_status == null) return;
		}
		
		if (status == 0) {
			_status.GetComponent<Image>().enabled = false;
		} else {
			_status.GetComponent<Image>().enabled = true;
			if (status == 1) {//Enabled sprite
				_status.GetComponent<Image>().sprite = World.interfaceSprites[5];
			} else {
				_status.GetComponent<Image>().sprite = World.interfaceSprites[4];
			}
		}
		
	}
	
	public void setReward(DataReward rew) {
		setReward(rew.id, rew.count);
	}
		
	// Update is called once per frame
	void Update () {
	
	}
	
	
}
