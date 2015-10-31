using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CraftingControler : MonoBehaviour {


	public static int selectedCrafting = -1;

	static Dictionary<int, InventoryPanel> slots = new Dictionary<int, InventoryPanel>();

	public InventoryPanel resultSlot;
	public InventoryPanel[] rewardSlots;

	// Use this for initialization
	void Start () {
	
		int y = 35;
		foreach (DataTile tile in WorldData.tiles.Values) {
			if (tile.type != WorldData.TILE_TYPE_CRAFTING) continue;
			
			createPanel(tile.id,36, y);
			
			if (selectedCrafting < 0) selectedCrafting = tile.id;
			
			y+=66;
		}
		
		if (y < 270) y = 270;
		
		GetComponent<RectTransform>().sizeDelta = new Vector2(70,y);
		
		setRecepie();	
		
		Inventory.OnInventoryChange += onInventoryChange;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void createPanel(int itemID, int x, int y) {
		
		GameObject slot = Instantiate (Resources.Load ("PanelItem")) as GameObject;
			slot.transform.SetParent(transform,false);
			slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -y);
			slot.GetComponent<Button>().onClick.AddListener(delegate{onPanelClick(itemID);});
		
			InventoryPanel panel = slot.GetComponent<InventoryPanel>();
			panel.setReward(itemID,0);
			//setReward(WorldData.tiles[itemID].result);
		
		slots.Add(itemID, panel);
	}
	
	void setRecepie() {
		DataTile tile = WorldData.tiles[selectedCrafting];
		if (tile == null) return;
		
		resultSlot.setReward(selectedCrafting,tile.result.count);
		for (int i = 0; i < rewardSlots.Length; i++) {
			if (i < tile.rewards.Count && tile.rewards[i] != null) {
				rewardSlots[i].setReward(tile.rewards[i]);
				
				if (Inventory.checkCount(tile.rewards[i].id, tile.rewards[i].count)) {
					rewardSlots[i].setStatus(1);
				} else {
					rewardSlots[i].setStatus(-1);
				}
				
			} else {
				rewardSlots[i].setReward(-1);
				rewardSlots[i].setStatus(0);
			}
		}
	}
	
	void onInventoryChange(int itemID, int count) {
		//Debug.Log("Invetory change:" + itemID + " count:" + count);
		for (int i = 0; i < rewardSlots.Length; i++) {
			if (rewardSlots[i].itemID == itemID) {
			
				if (rewardSlots[i].count <= count) {
					rewardSlots[i].setStatus(1);
				} else {
					rewardSlots[i].setStatus(-1);
				}
				
			}
		}
	}
	
	public void onPanelClick(int itemID) {
		//Debug.Log("Panelclicked " + itemID.ToString());
		selectedCrafting = itemID;
		setRecepie();	
	}
	
	public void onCraftingClick() {
		Debug.Log("Start crafting");
		if (selectedCrafting < 0) return;
		DataTile tile = WorldData.tiles[selectedCrafting];
		if (tile == null) return;
		
		if (!Inventory.checkStorage(tile.result.id, tile.result.count)) return;
		
		bool canCraft = true;
		for (int i = 0; i < tile.rewards.Count; i++) {
			if (!Inventory.checkCount(tile.rewards[i].id, tile.rewards[i].count)) {
				canCraft = false;
				break;
			}
		}
		
		if (!canCraft) return;
		
		Inventory.addReward(tile.result);
		for (int i = 0; i < tile.rewards.Count; i++) {
			Inventory.addReward(tile.rewards[i].id, -tile.rewards[i].count);
		}
	}
}
