using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item {
	public int itemID;
	public string name;

	public Item(int id, SimpleJSON.JSONNode node) {
		itemID = id;
		name = node["name"];
	}

}

public class ItemsManager  {

	public static Dictionary<int, Item> Items = new Dictionary<int,Item>();

	public static void Init(SimpleJSON.JSONNode node) {

		for (int i = 0; i < node.Count; i++) {
			int item_id = int.Parse(node.AsObject.keyAt(i));
			Items.Add(item_id, new Item(item_id, node[i]));
		}

	}

}
