using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiParamsPanel : MonoBehaviour {

	public Text PrepareTimeText;

	public guiBinaryProgress[] ProgressBars;

	public FoodClass Food {
		set {

			ProgressBars[0].Value = (float)value.Health / 100f;
			ProgressBars[1].Value = (float)value.Calorie / 100f;

			ProgressBars[2].Value = (float)value.Salt;
			ProgressBars[3].Value = (float)value.Sweet;
			ProgressBars[4].Value = (float)value.Sour;
			ProgressBars[5].Value = (float)value.Spice;
			ProgressBars[6].Value = (float)value.Bitter;

			PrepareTimeText.text = value.Time.ToString("0") + "s";

		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
