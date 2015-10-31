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
	}
}

public class Inventory : MonoBehaviour {

	public InventoryType Type;
	public int Size = 16;
	public int MaxAmount = 99;

	private Dictionary<int, InventoryItem> Items = new Dictionary<int,InventoryItem>();

	// Use this for initialization
	void Start () {
		for (int i = 0; i < Size; i++) {
			InventoryItem item = new InventoryItem(i);
			item.Panel.transform.SetParent(transform);
			Items.Add(i, item);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool Add(int itemID, int amount, int index = 0) {
		ItemClass item = ItemsManager.GetItem<ItemClass>(itemID);
		return Add(item, amount, index);
	}

	public bool Add(ItemClass item, int amount, int index = 0) {
		if (Type == InventoryType.MULTIPLE) {
			if (index < 0 || index >= Size) return false;
			if (Items[index].item_id > -1 &&  Items[index].item_id != item.item_id) return false;

			Items[index].Update(item.item_id, amount);

			return true;

		}

		foreach (InventoryItem inve in Items.Values) {
			if (inve.item_id != item.item_id) continue;
			inve.Update(item.item_id, amount);
			return true;
		}

		foreach (InventoryItem inve in Items.Values) {
			if (inve.item_id != -1) continue;
			inve.Update(item.item_id, amount);
			return true;
		}


		return false;

	}
}
