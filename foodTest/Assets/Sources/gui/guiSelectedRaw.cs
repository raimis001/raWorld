using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiSelectedRaw : MonoBehaviour {

	public Text CaptionText;
	public Text DescriptionText;
	public Text TypeText;
	public Image Icon;

	public guiParamsPanel FoodParams;

	public Inventory CurrentInventory;
	
	int _selectedTab = 0;

	ItemFood _selectedItem = null;
	public int SelectedItem {
		get { return _selectedItem.item_id; }
		set {
			_selectedItem = ItemsManager.GetItem<ItemFood>(value);

			if (CaptionText) CaptionText.text = _selectedItem.name;
			if (DescriptionText) DescriptionText.text = _selectedItem.description;
			if (Icon) Icon.sprite = _selectedItem.Image;

			OnTabChange(_selectedTab);
		}
	}

	public FoodClass Food {
		set {
			if (FoodParams) FoodParams.Food = value;
		}
	}

	// Use this for initialization
	void Start () {
		if (TypeText) TypeText.text = "Raw data";
		guiTabList.OnTabClick += OnTabChange;
	}
	void OnEnable() {
		//Debug.Log("Enable tabs");
	}
	void OnDisable() {
		//Debug.Log("Disable tabs");
		//guiTabList.OnTabClick -= OnTabChange;
	}


	void OnTabChange(int tab) {
		_selectedTab = tab;

		switch (tab) {
			case 0:
				if (TypeText) TypeText.text = "Raw data";
				if (_selectedItem != null) Food = _selectedItem.Raw;
				break;
			case 1:
				if (TypeText) TypeText.text = "Boil data";
				if (_selectedItem != null) Food = _selectedItem.Boil;
				break;
			case 2:
				if (TypeText) TypeText.text = "Bake data";
				if (_selectedItem != null) Food = _selectedItem.Bake;
				break;
		}

		if (!gameObject.activeSelf) return;
		if (CurrentInventory) CurrentInventory.Recalc(tab);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void AddToInventory() {
		if (!CurrentInventory) return;
		CurrentInventory.Add(_selectedItem, 1, _selectedTab);

	}
}
