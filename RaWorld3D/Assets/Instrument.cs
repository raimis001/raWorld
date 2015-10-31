using UnityEngine;
using System.Collections;

public class Instrument : MonoBehaviour {

	public float speed = 0.075f;

	float alpha = 1f;
	float delta = -0.1f;
	
	SpriteRenderer spriteRenderer;
	
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		delta = -1 * speed;
	}
	
	// Update is called once per frame
	void Update () {
		alpha += delta;
		if (delta < 0 && alpha <= 0f) {
			delta = speed;
		} 
		if (delta > 0 && alpha >= 1f) {
			delta = -1 * speed;
		} 
		
		spriteRenderer.color = new Color(1f,1f,1f,alpha);
	}
	
	
}
