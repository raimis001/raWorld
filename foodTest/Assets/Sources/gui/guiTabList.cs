using UnityEngine;
using System.Collections;

public class guiTabList : MonoBehaviour {

	public delegate void TabClick(int tab);
	public static event TabClick OnTabClick;

	public guiTab[] Tabs;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < Tabs.Length; i++) {
			Tabs[i].ID = i;
			Tabs[i].Selected = i == 0;
			Tabs[i].Callback = OnTabClicked;
		}
	}

	void OnTabClicked(int tag) {
		//Debug.Log("Tab clicked");
		for (int i = 0; i < Tabs.Length; i++) {
			Tabs[i].Selected = i == tag;
		}

		if (OnTabClick != null) OnTabClick(tag);
	}

	// Update is called once per frame
	void Update () {
	
	}


}
