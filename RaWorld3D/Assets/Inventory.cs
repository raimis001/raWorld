using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	public PlayerControler player;

	public const int slotCount = 16;
	public static DataReward[] rewards = new DataReward[slotCount];

	static InventoryPanel[] slots = new InventoryPanel[slotCount];
	
	public delegate void InventoryChange(int itemID, int count);
	public static event InventoryChange OnInventoryChange;
	

	// Use this for initialization
	void Start () {
		
	
		int x = 36;
		int y = 36;
		for (int i = 0; i < slotCount; i++) {
			if (i > 0 && i % 4 == 0) {
				x = 36;
				y += 66;
			}
			
			createPanel(i, x, y);
			x += 66;
		}
			
		addReward(46,500);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void createPanel(int tag,int x, int y) {
		rewards[tag] = new DataReward(-1,0);
		
		GameObject slot = Instantiate (Resources.Load ("PanelItem")) as GameObject;
			slot.transform.SetParent(transform,false);
			slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -y);
			slot.GetComponent<Button>().onClick.AddListener(delegate{onPanelClick(tag);});
		
		slots[tag] = slot.GetComponent<InventoryPanel>();
		slots[tag].setReward(-1,0);
	}
	
	public void onPanelClick(int panel) {
		Debug.Log("Panelclicked " + panel.ToString());
		
		if (CursorControler.reward.id < 0) {
			player.putinHand(rewards[panel]);
			rewards[panel].reset();
			slots[panel].setReward(-1,0);
		} else {
		
			if (rewards[panel].id < 0 || rewards[panel].id == CursorControler.reward.id) {
				rewards[panel].id = CursorControler.reward.id;
				rewards[panel].count += CursorControler.reward.count;
				slots[panel].setReward(rewards[panel]);
				CursorControler.SetReward(-1,0);
				player.putinHand(-1,0);
			}
		
		}
	}
	
	
	public static bool addReward(int tileID, int count) {
		DataTile tile = WorldData.tiles[tileID];
		if (tile == null) return false;
	
	
		for (int i = 0; i < slotCount; i++) {
			if (rewards[i].id == tileID) {
				rewards[i].count += count;
				fireEvent(rewards[i].id, rewards[i].count, count);
				
				if (rewards[i].count < 1) {
					rewards[i].id = -1;
					rewards[i].count = 0;
					slots[i].setReward(-1);
				} else {
					slots[i].setReward(rewards[i]);
				}
				
				return true;
			}
		}
		if (count < 0) {
			return false;
		}
		for (int i = 0; i < slotCount; i++) {
			if (rewards[i].id < 0) {
				rewards[i].id = tileID;
				rewards[i].count = count;
				slots[i].setReward(rewards[i]);
				fireEvent(rewards[i].id, rewards[i].count, count);
				return true;
			}
		}
		
		return false;
			
	}
	
	static void fireEvent(int itemID, int count, int changes) {
		if(OnInventoryChange != null)
			OnInventoryChange(itemID, count);
		DataTile tile = WorldData.tiles[itemID];
		if (tile == null) return;
		Logger.addLog("Added " + tile.name + " count " + changes.ToString());
	}
	
	public static bool addReward(DataReward rew) {
		return addReward(rew.id, rew.count);
	}
	
	public static int getCount(int itemID) {
		int count = 0;
		for (int i = 0; i < slotCount; i++) {
			if (rewards[i].id == itemID) count += rewards[i].count;
		}
		return count;	
	}
	
	public static bool checkCount(int itemID, int count) {
		return getCount(itemID) >= count;	
	}
	public static bool checkStorage(int itemID, int count) {
		for (int i = 0; i < slotCount; i++) {
			if (rewards[i].id == itemID || rewards[i].id < 0) return true;
		}
		return false;
	}			
}
