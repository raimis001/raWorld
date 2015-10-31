using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiIconPanel : MonoBehaviour {

	public Image IconImage;
	public Text AmountText;

	public int Amount {
		set {
			if (AmountText == null) return;

			AmountText.gameObject.SetActive(true);
			AmountText.text = value.ToString();
		}
	}

	int _itemID = 0;
	public int ItemID {
		get { return _itemID; }
		set {
			_itemID = value;
			ItemClass data = ItemsManager.GetItem<ItemClass>(_itemID);

			if (data == null) {
				Debug.Log("Item not fount ID:" + _itemID.ToString());
				IconImage.gameObject.SetActive(false);
				return;
			}

			if (IconImage != null) {
				IconImage.sprite = data.Image;
				IconImage.gameObject.SetActive(true);
			}
		}
	}

	public delegate void PanelClick(int tag);
	public PanelClick OnPanelClick;


	void Awake() {
		IconImage.gameObject.SetActive(false);
		AmountText.gameObject.SetActive(false);
	}
	// Use this for initialization
	void Start () {
	}

	
	// Update is called once per frame
	void Update () {
	}

	public void OnMouseClick() {
		if (OnPanelClick != null) OnPanelClick(ItemID);
	}
}
