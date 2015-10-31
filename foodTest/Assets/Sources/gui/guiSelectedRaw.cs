using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiSelectedRaw : MonoBehaviour {

	public Text CaptionText;
	public Text DescriptionText;
	public Text TypeText;
	public Text PrepareTimeText;

	public guiBinaryProgress[] ProgressBars;

	public Inventory CurrentInventory;
	
	int _selectedTab = 0;
	ItemFood _selectedItem = null;
	public int SelectedItem {
		get { return _selectedItem.item_id; }
		set {
			_selectedItem = ItemsManager.GetItem<ItemFood>(value);

			if (CaptionText) CaptionText.text = _selectedItem.name;
			if (DescriptionText) DescriptionText.text = _selectedItem.description;

			OnTabChange(_selectedTab);
		}
	}

	public FoodClass Food {
		set {
			ProgressBars[0].Value = (float)value.Calorie / 100f;
			ProgressBars[1].Value = (float)value.Health / 100f;
			ProgressBars[2].Value = (float)value.Sweet / 100f;
			ProgressBars[3].Value = (float)value.Salt / 100f;
			ProgressBars[4].Value = (float)value.Sour / 100f;
			ProgressBars[5].Value = (float)value.Bitter / 100f;

			PrepareTimeText.text = value.Time.ToString("0") + "s";
		}
	}

	// Use this for initialization
	void Start () {
		if (TypeText) TypeText.text = "Raw data";
	}
	void OnEnable() {
		guiTabList.OnTabClick += OnTabChange;
	}
	void OnDisable() {
		guiTabList.OnTabClick -= OnTabChange;
	}


	void OnTabChange(int tab) {
		_selectedTab = 0;
		switch (tab) {
			case 0:
				if (TypeText) TypeText.text = "Raw data";
				Food = _selectedItem.Raw;
				break;
			case 1:
				if (TypeText) TypeText.text = "Boil data";
				Food = _selectedItem.Boil;
				break;
			case 2:
				if (TypeText) TypeText.text = "Bake data";
				Food = _selectedItem.Bake;
				break;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void AddToInventory() {
		if (!CurrentInventory) return;
		CurrentInventory.Add(_selectedItem, 1);

	}
}
