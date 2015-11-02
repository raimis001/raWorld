using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum InventoryType {
	SINGLETON,
	MULTIPLE
}

public class InventoryItem {
	public int index;
	public int item_id = -1;
	public int Amount = 0;
	public guiIconPanel Panel;

	public InventoryItem(int idx) {
		index = idx;
		Panel = Game.CreateResource<guiIconPanel>("interface/Panel");
	}

	public void Update(int itemID, int amount) {
		if (item_id != itemID) {
			item_id = itemID;
			Panel.ItemID = item_id;
		}
		Amount += amount;
		Panel.Amount = Amount;
		if (Amount < 1) {
			item_id = -1; 
			Panel.ItemID = -1;
		}
	}
}

public class Inventory : MonoBehaviour {

	public InventoryType Type;
	public int Size = 16;
	public int MaxAmount = 99;

	public guiParamsPanel FoodParams;
	public guiTaste TasteParam;

	private Dictionary<int, InventoryItem> Items = new Dictionary<int,InventoryItem>();
	private FoodCollection Food = new FoodCollection();

	// Use this for initialization
	void Start () {
		for (int i = 0; i < Size; i++) {
			InventoryItem item = new InventoryItem(i);
			item.Panel.transform.SetParent(transform);
			item.Panel.Index = i;
			item.Panel.OnPanelClick = OnPanelClick;

			Items.Add(i, item);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnPanelClick(int itemID, int index) {
		if (itemID < 0) return;

		InventoryItem item = Items[index];
		if (item == null) return;

		item.Update(itemID, -1);
		Food.Remove(itemID);

		if (FoodParams) FoodParams.Food = Food;
		if (TasteParam) TasteParam.Progress = Food.Taste;
	}

	void AddCollection(ItemClass item, int amount, int foodType) {
		FoodClass food = null;
		switch (foodType) {
			case 0: //Raw food
				food = ((ItemFood)item).Raw;
				break;
			case 1: //boiled food
				food = ((ItemFood)item).Boil;
				break;
			case 2: //Raw food
				food = ((ItemFood)item).Bake;
				break;
		}
		if (food == null) return;

		Food.Add(food, amount);

		if (FoodParams) FoodParams.Food = Food;
		if (TasteParam) TasteParam.Progress = Food.Taste;
	}

	public bool Add(int itemID, int amount, int foodType, int index = 0) {
		ItemClass item = ItemsManager.GetItem<ItemClass>(itemID);
		return Add(item, amount, index);
	}

	public bool Add(ItemClass item, int amount, int foodType, int index = 0) {
		if (Type == InventoryType.MULTIPLE) {
			if (index < 0 || index >= Size) return false;
			if (Items[index].item_id > -1 &&  Items[index].item_id != item.item_id) return false;

			Items[index].Update(item.item_id, amount);

			AddCollection(item, amount, foodType);
			return true;

		}

		foreach (InventoryItem inve in Items.Values) {
			if (inve.item_id != item.item_id) continue;
			inve.Update(item.item_id, amount);
			AddCollection(item, amount, foodType);
			return true;
		}

		foreach (InventoryItem inve in Items.Values) {
			if (inve.item_id != -1) continue;
			inve.Update(item.item_id, amount);
			AddCollection(item, amount, foodType);
			return true;
		}

		return false;

	}

	public void Recalc(int foodType) {
		Food.Clear();

		foreach (InventoryItem inve in Items.Values) {
			if (inve.item_id < 0) continue;
			ItemClass item = ItemsManager.GetItem<ItemClass>(inve.item_id);
			AddCollection(item,inve.Amount, foodType);
		}

		if (FoodParams) FoodParams.Food = Food;
		if (TasteParam) TasteParam.Progress = Food.Taste;
	}

	public void MakeMeal() {
		if (Food.Count() < 1) return;
	}
}
