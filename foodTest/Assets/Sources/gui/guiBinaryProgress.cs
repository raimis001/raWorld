using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiBinaryProgress : MonoBehaviour {

	public Slider NegativeSlider;
	public Slider PositiveSlider;

	float _value = 0;
	public float Value {
		get { return _value; }
		set {
			_value = value;

			if (_value >= 0) {
				PositiveSlider.value = Mathf.Abs(_value);
				NegativeSlider.value = 0;

			} else {
				PositiveSlider.value = 0;
				NegativeSlider.value = Mathf.Abs(_value); 
			}

			Color cN = NegativeSlider.fillRect.gameObject.GetComponent<Image>().color;
			Color cP = PositiveSlider.fillRect.gameObject.GetComponent<Image>().color;

			if (_value < 0) {
				cN.a =_value > -0.2f ? _value / 0.2f : 1;
				cP.a = 0;
			} else {
				cP.a = _value > 0.1f && _value < 0.2f ? _value / 0.2f : 1;
				cN.a = 0;
			}

			NegativeSlider.fillRect.gameObject.GetComponent<Image>().color = cN;
			PositiveSlider.fillRect.gameObject.GetComponent<Image>().color = cP;
			
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
