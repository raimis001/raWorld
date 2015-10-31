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

			float v = _value + 0.15f;
			Color c = NegativeSlider.fillRect.gameObject.GetComponent<Image>().color;
			if (v >= 0 && v < 0.3f) {
				c.a =1 - (v  / 0.3f);
			} else {
				c.a = _value < 0 ? 1f : 0;
			}
			NegativeSlider.fillRect.gameObject.GetComponent<Image>().color = c;
			
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
