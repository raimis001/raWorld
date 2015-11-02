using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public guiSelectedRaw Selected;

	private static Game _instance = null;

	public static float CurrentTime = 0;
	public delegate void ChangeHour();
	public static event ChangeHour OnChangeHour;

	public guiScroll ItemsScroll;
	public PlayerData Player;

	public guiDayNight DayNight;

	public const float HourTime = 2;

	void Awake() {
		_instance = this;
		ItemsManager.Init();
	}

	// Use this for initialization
	void Start () {
		Application.runInBackground = true;

		foreach (ItemClass itm in ItemsManager.Items.Values) {
			if (!(itm is ItemFood)) continue;

			guiIconPanel panel = ItemsScroll.AddItem<guiIconPanel>();
			panel.ItemID = itm.item_id;
			panel.Amount = 10;
			panel.OnPanelClick = OnPanelClick;
			
		}

		PlayerData.Health = 1;
		
		Selected.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		CurrentTime += Time.deltaTime / HourTime;
		if (CurrentTime >= 24) CurrentTime = 0;

		PlayerData.Health -= Time.deltaTime / Player.TiredRate;

		//Debug.Log(CurrentTime.ToString() + " " + PlayerData.Health);

	}


	void OnPanelClick(int itemID, int index) {
		//Debug.Log("Click on icon ID:" + itemID.ToString());

		Selected.SelectedItem = itemID;
		Selected.gameObject.SetActive(true);

	}

	public static T CreateResource<T>(string name) {
		GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>(name));

		return obj.GetComponent<T>();

	}
}
